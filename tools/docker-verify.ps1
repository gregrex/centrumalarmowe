#!/usr/bin/env pwsh
# docker-verify.ps1 — Docker build + up + smoke + down verification gate
# Usage: .\tools\docker-verify.ps1
# Requires: Docker Desktop running, ports from .env or defaults

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$failures = [System.Collections.Generic.List[string]]::new()

function Write-Step([string]$msg) { Write-Host "`n==> $msg" -ForegroundColor Cyan }
function Write-Ok([string]$msg)   { Write-Host "  OK: $msg" -ForegroundColor Green }
function Write-Fail([string]$msg) {
    Write-Host "  FAIL: $msg" -ForegroundColor Red
    $script:failures.Add($msg)
}
function Write-Dim([string]$msg)  { Write-Host "  $msg" -ForegroundColor DarkGray }

function Invoke-Compose([string[]]$Arguments)
{
    $output = & docker compose -f infra/docker-compose.yml @Arguments 2>&1
    return @{
        ExitCode = $LASTEXITCODE
        Output = @($output)
    }
}

function Wait-ForHttpJsonOk([string]$Label, [string]$Url, [int]$MaxSeconds = 30)
{
    Write-Step "Wait for $Label to become healthy (max ${MaxSeconds}s)"
    $attempts = [Math]::Ceiling($MaxSeconds / 2)
    for ($i = 0; $i -lt $attempts; $i++) {
        Start-Sleep -Seconds 2
        try {
            $resp = Invoke-RestMethod -Uri $Url -TimeoutSec 3 -ErrorAction Stop
            if ($resp.ok -eq $true) {
                Write-Ok "$Label health OK at $Url"
                return $true
            }
        } catch { }
    }

    Write-Fail "$Label did not become healthy within ${MaxSeconds}s at $Url"
    return $false
}

# Load infra/.env if present
$envFile = Join-Path $root "infra\.env"
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match "^\s*([^#=]+)=(.*)$") {
            [System.Environment]::SetEnvironmentVariable($Matches[1].Trim(), $Matches[2].Trim())
        }
    }
}
$apiPort = $env:API_PORT ?? "5080"
$adminPort = $env:ADMIN_PORT ?? "5081"
$adminUsername = $env:ADMIN_USERNAME ?? "admin"
$adminPassword = $env:ADMIN_PASSWORD ?? "AdminDemoPass_12345"
$adminAuthHeader = "Basic " + [Convert]::ToBase64String([Text.Encoding]::UTF8.GetBytes("$adminUsername`:$adminPassword"))

Push-Location $root
try {
    Write-Step "Docker daemon preflight"
    $dockerVersion = & docker version --format "{{.Server.Version}}" 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "docker daemon unavailable"
        if ($dockerVersion) { $dockerVersion | ForEach-Object { Write-Dim "$_" } }
    } else {
        Write-Ok "docker server $dockerVersion"
    }

    Write-Step "docker compose build"
    $build = Invoke-Compose @("build", "--quiet")
    if ($build.ExitCode -ne 0) {
        Write-Fail "docker compose build failed"
        $build.Output | Select-Object -Last 20 | ForEach-Object { Write-Dim "$_" }
    } else {
        Write-Ok "build"
    }

    Write-Step "docker compose up -d"
    $up = Invoke-Compose @("up", "-d")
    if ($up.ExitCode -ne 0) {
        Write-Fail "docker compose up failed"
        $up.Output | Select-Object -Last 20 | ForEach-Object { Write-Dim "$_" }
    } else {
        Write-Ok "up"
    }

    $apiUrl = "http://localhost:$apiPort/health"
    $adminHealthUrl = "http://localhost:$adminPort/health"
    $null = Wait-ForHttpJsonOk -Label "API" -Url $apiUrl -MaxSeconds 30
    $null = Wait-ForHttpJsonOk -Label "AdminWeb" -Url $adminHealthUrl -MaxSeconds 30

    Write-Step "Smoke: key API endpoints"
    $endpoints = @(
        "/health",
        "/api/reference-data",
        "/api/home-hub"
    )
    foreach ($ep in $endpoints) {
        try {
            $r = Invoke-WebRequest -Uri "http://localhost:$apiPort$ep" -TimeoutSec 5 -ErrorAction Stop
            if ($r.StatusCode -eq 200) { Write-Ok "GET $ep → 200" }
            else { Write-Fail "GET $ep → $($r.StatusCode)" }
        } catch {
            Write-Fail "GET $ep → exception: $_"
        }
    }

    Write-Step "Smoke: real session action flow"
    try {
        $created = Invoke-RestMethod -Method POST -Uri "http://localhost:$apiPort/api/sessions/demo" -TimeoutSec 5 -ErrorAction Stop
        $sessionId = [string]$created.sessionId
        if ([string]::IsNullOrWhiteSpace($sessionId)) {
            Write-Fail "session flow → missing sessionId from demo session response"
        } else {
            $snapshot = Invoke-RestMethod -Method GET -Uri "http://localhost:$apiPort/api/sessions/$sessionId" -TimeoutSec 5 -ErrorAction Stop
            $incident = $snapshot.incidents | Where-Object { $_.status -eq "pending" } | Select-Object -First 1
            $unit = $snapshot.units | Where-Object { $_.status -eq "available" } | Select-Object -First 1

            if ($null -eq $incident -or $null -eq $unit) {
                Write-Fail "session flow → could not find pending incident and available unit"
            } else {
                $actionBody = @{
                    sessionId     = $sessionId
                    actorId       = "docker-smoke"
                    role          = "Dispatcher"
                    actionType    = "dispatch"
                    payloadJson   = (@{ incidentId = $incident.incidentId; unitId = $unit.unitId } | ConvertTo-Json -Compress)
                    correlationId = [guid]::NewGuid().ToString("N")
                } | ConvertTo-Json -Compress

                $actionResult = Invoke-RestMethod -Method POST -Uri "http://localhost:$apiPort/api/sessions/$sessionId/actions" -ContentType "application/json" -Body $actionBody -TimeoutSec 5 -ErrorAction Stop
                if ($actionResult.success -ne $true) {
                    Write-Fail "session flow → dispatch action returned unsuccessful result"
                } else {
                    $updatedSnapshot = Invoke-RestMethod -Method GET -Uri "http://localhost:$apiPort/api/sessions/$sessionId" -TimeoutSec 5 -ErrorAction Stop
                    $updatedIncident = $updatedSnapshot.incidents | Where-Object { $_.incidentId -eq $incident.incidentId } | Select-Object -First 1
                    $updatedUnit = $updatedSnapshot.units | Where-Object { $_.unitId -eq $unit.unitId } | Select-Object -First 1

                    if ($updatedIncident.status -eq "dispatched" -and $updatedUnit.status -eq "dispatched") {
                        Write-Ok "real session action flow"
                    } else {
                        Write-Fail "session flow → snapshot did not change to dispatched state"
                    }
                }
            }
        }
    } catch {
        Write-Fail "session flow → exception: $_"
    }

    Write-Step "Smoke: AdminWeb public and protected routes"
    $adminChecks = @(
        @{ Url = "http://localhost:$adminPort/"; Label = "landing"; Headers = @{} },
        @{ Url = "http://localhost:$adminPort/app"; Label = "user dashboard"; Headers = @{} },
        @{ Url = "http://localhost:$adminPort/health"; Label = "admin health"; Headers = @{} },
        @{ Url = "http://localhost:$adminPort/admin"; Label = "admin dashboard"; Headers = @{ Authorization = $adminAuthHeader } }
    )
    foreach ($check in $adminChecks) {
        try {
            $r = Invoke-WebRequest -Uri $check.Url -Headers $check.Headers -TimeoutSec 5 -ErrorAction Stop
            if ($r.StatusCode -eq 200) { Write-Ok "GET $($check.Label) → 200" }
            else { Write-Fail "GET $($check.Label) → $($r.StatusCode)" }
        } catch {
            Write-Fail "GET $($check.Label) → exception: $_"
        }
    }

    Write-Step "docker compose ps"
    docker compose -f infra/docker-compose.yml ps
} finally {
    Write-Step "docker compose down"
    $down = Invoke-Compose @("down", "-v")
    if ($down.ExitCode -ne 0) {
        Write-Fail "docker compose down failed"
        $down.Output | Select-Object -Last 20 | ForEach-Object { Write-Dim "$_" }
    } else {
        Write-Ok "down"
    }
    Pop-Location
}

Write-Host "`n===== Docker Verify Summary =====" -ForegroundColor White
if ($failures.Count -eq 0) {
    Write-Host "RESULT: ALL DOCKER GATES PASS" -ForegroundColor Green
    exit 0
} else {
    Write-Host "RESULT: FAILED ($($failures.Count) gate(s)):" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    exit 1
}
