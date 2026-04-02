#!/usr/bin/env pwsh
<#
RBAC Smoke Test for Alarm112 v26
Tests JWT auth, role generation, and protected endpoints
#>

$port = 5099
$apiUrl = "http://localhost:$port"
$repoRoot = Split-Path -Path (Split-Path -Path $MyInvocation.MyCommandPath -Parent) -Parent

Write-Host "=== RBAC Smoke Test v26 ===" -ForegroundColor Cyan

# Setup env
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:SECURITY__REQUIREAUTH = "true"
$env:SECURITY__ENABLEDEVTOKENENDPOINT = "true"
$env:SECURITY__JWT__SIGNINGKEY = "dev-test-key-32chars-minimum-length-here"

# Kill existing process
Write-Host "Cleaning port $port..." -ForegroundColor Yellow
$existing = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
if ($null -ne $existing) {
    Stop-Process -Id $existing.OwningProcess -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 1
}

# Start API
Write-Host "Starting API with auth..." -ForegroundColor Yellow
Push-Location $repoRoot
$proc = Start-Process `
    -FilePath dotnet `
    -ArgumentList "run", "--project", "src/Alarm112.Api", "--urls", "http://localhost:$port" `
    -PassThru `
    -NoNewWindow

if ($null -eq $proc) {
    Write-Host "Failed to start" -ForegroundColor Red
    exit 1
}

$pid = $proc.Id
Write-Host "Process started (PID: $pid), waiting..." -ForegroundColor Gray
Start-Sleep -Seconds 3

# Test health
Write-Host "`n[Test 1] Health endpoint (public)..." -ForegroundColor Yellow
try {
    $h = Invoke-WebRequest "$apiUrl/health" -ErrorAction SilentlyContinue
    Write-Host "✓ /health returned $($h.StatusCode)" -ForegroundColor Green
} catch {
    Write-Host "✗ Health endpoint failed" -ForegroundColor Red
}

# Test without token
Write-Host "`n[Test 2] Protected endpoint WITHOUT token (should 401)..." -ForegroundColor Yellow
try {
    Invoke-WebRequest "$apiUrl/api/reference-data" -ErrorAction SilentlyContinue
    Write-Host "✗ Should have failed with 401" -ForegroundColor Red
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    if ($code -eq 401) {
        Write-Host "✓ Got 401 (expected)" -ForegroundColor Green
    } else {
        Write-Host "✗ Got $code instead of 401" -ForegroundColor Red
    }
}

# Get token for Dispatcher
Write-Host "`n[Test 3] Generate token (Dispatcher role)..." -ForegroundColor Yellow
try {
    $body = '{"subject":"qa","role":"Dispatcher"}'
    $resp = Invoke-WebRequest "$apiUrl/auth/dev-token" `
        -Method POST `
        -ContentType "application/json" `
        -Body $body `
        -ErrorAction SilentlyContinue
    $json = $resp.Content | ConvertFrom-Json
    $token = $json.accessToken
    Write-Host "✓ Generated Dispatcher token" -ForegroundColor Green
    Write-Host "  Role: $($json.role), Expires: $($json.expiresIn)s" -ForegroundColor Gray
} catch {
    Write-Host "✗ Token generation failed: $($_.Exception.Message)" -ForegroundColor Red
    $token = $null
}

# Test with token
if ($token) {
    Write-Host "`n[Test 4] Protected endpoint WITH token..." -ForegroundColor Yellow
    try {
        $h = @{ Authorization = "Bearer $token" }
        $r = Invoke-WebRequest "$apiUrl/api/reference-data" `
            -Headers $h `
            -ErrorAction SilentlyContinue
        Write-Host "✓ /api/reference-data returned $($r.StatusCode)" -ForegroundColor Green
    } catch {
        Write-Host "✗ Failed: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Test invalid token
Write-Host "`n[Test 5] Protected endpoint with INVALID token (should 401)..." -ForegroundColor Yellow
try {
    $h = @{ Authorization = "Bearer invalid-token-here" }
    Invoke-WebRequest "$apiUrl/api/reference-data" `
        -Headers $h `
        -ErrorAction SilentlyContinue
    Write-Host "✗ Should have failed with 401" -ForegroundColor Red
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    if ($code -eq 401) {
        Write-Host "✓ Got 401 with invalid token (expected)" -ForegroundColor Green
    } else {
        Write-Host "✗ Got $code instead of 401" -ForegroundColor Red
    }
}

# Test different roles
Write-Host "`n[Test 6] Generate tokens for all 4 roles..." -ForegroundColor Yellow
$roles = @("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer")
foreach ($r in $roles) {
    try {
        $body = "{`"subject`":`"user`",`"role`":`"$r`"}"
        $resp = Invoke-WebRequest "$apiUrl/auth/dev-token" `
            -Method POST `
            -ContentType "application/json" `
            -Body $body `
            -ErrorAction SilentlyContinue
        Write-Host "✓ Token for $r" -ForegroundColor Green
    } catch {
        Write-Host "✗ Failed for $r" -ForegroundColor Red
    }
}

# Cleanup
Write-Host "`n" -ForegroundColor Cyan
Write-Host "=== RBAC Tests Complete ===" -ForegroundColor Cyan

Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
Pop-Location
Write-Host "API stopped." -ForegroundColor Gray
