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

# Load .env if present
$envFile = Join-Path $root ".env"
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match "^\s*([^#=]+)=(.*)$") {
            [System.Environment]::SetEnvironmentVariable($Matches[1].Trim(), $Matches[2].Trim())
        }
    }
}
$apiPort = $env:API_PORT ?? "5080"

Write-Step "docker compose build"
Push-Location $root
try {
    docker compose -f infra/docker-compose.yml build --quiet 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { Write-Fail "docker compose build failed" } else { Write-Ok "build" }

    Write-Step "docker compose up -d"
    docker compose -f infra/docker-compose.yml up -d 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) { Write-Fail "docker compose up failed" } else { Write-Ok "up" }

    Write-Step "Wait for API to become healthy (max 30s)"
    $healthy = $false
    $apiUrl = "http://localhost:$apiPort/health"
    for ($i = 0; $i -lt 15; $i++) {
        Start-Sleep -Seconds 2
        try {
            $resp = Invoke-RestMethod -Uri $apiUrl -TimeoutSec 3 -ErrorAction Stop
            if ($resp.ok -eq $true) { $healthy = $true; break }
        } catch { }
    }

    if ($healthy) {
        Write-Ok "API health OK at $apiUrl"
    } else {
        Write-Fail "API did not become healthy within 30s at $apiUrl"
    }

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

    Write-Step "docker compose ps"
    docker compose -f infra/docker-compose.yml ps
} finally {
    Write-Step "docker compose down"
    docker compose -f infra/docker-compose.yml down -v 2>&1 | Out-Null
    Write-Ok "down"
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
