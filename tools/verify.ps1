#!/usr/bin/env pwsh
# verify.ps1 — Local build, test and smoke verification gate
# Usage: .\tools\verify.ps1

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$startTime = Get-Date
$failures = [System.Collections.Generic.List[string]]::new()

function Write-Step([string]$msg) { Write-Host "`n==> $msg" -ForegroundColor Cyan }
function Write-Ok([string]$msg)   { Write-Host "  OK: $msg" -ForegroundColor Green }
function Write-Fail([string]$msg) {
    Write-Host "  FAIL: $msg" -ForegroundColor Red
    $script:failures.Add($msg)
}

# 1. Restore
Write-Step "dotnet restore"
dotnet restore "$root/Alarm112.sln" --nologo | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Fail "dotnet restore exited $LASTEXITCODE" } else { Write-Ok "restore" }

# 2. Build
Write-Step "dotnet build"
dotnet build "$root/Alarm112.sln" --no-restore -c Debug --nologo | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Fail "dotnet build exited $LASTEXITCODE" } else { Write-Ok "build (0 errors)" }

# 3. Test
Write-Step "dotnet test"
dotnet test "$root/tests/Alarm112.Api.Tests/Alarm112.Api.Tests.csproj" --no-build --logger "console;verbosity=quiet" --nologo
if ($LASTEXITCODE -ne 0) { Write-Fail "dotnet test exited $LASTEXITCODE" } else { Write-Ok "all tests green" }

# 4. Smoke v26
Write-Step "smoke-v26"
try {
    & "$PSScriptRoot/smoke-v26.ps1"
    Write-Ok "smoke-v26"
} catch {
    Write-Fail "smoke-v26: $_"
}

# 5. Content verify
Write-Step "content-verify"
try {
    & "$PSScriptRoot/content-verify.ps1"
    Write-Ok "content-verify"
} catch {
    Write-Fail "content-verify: $_"
}

# Summary
$elapsed = (Get-Date) - $startTime
Write-Host "`n===== Verify Summary =====" -ForegroundColor White
Write-Host "Elapsed: $($elapsed.ToString('mm\:ss'))" -ForegroundColor Gray
if ($failures.Count -eq 0) {
    Write-Host "RESULT: ALL GATES PASS" -ForegroundColor Green
    exit 0
} else {
    Write-Host "RESULT: FAILED ($($failures.Count) gate(s)):" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    exit 1
}
