$root = Split-Path -Parent $PSScriptRoot
Write-Host "[boot-local] validating content"
& "$root/tools/content-verify.ps1"
$apiPort = if ($env:API_PORT) { $env:API_PORT } else { "5080" }
$adminPort = if ($env:ADMIN_PORT) { $env:ADMIN_PORT } else { "5081" }
Write-Host "[boot-local] endpoints (expected after app start):"
Write-Host "  API:       http://localhost:$apiPort"
Write-Host "  Live:      http://localhost:$apiPort/health/live"
Write-Host "  Ready:     http://localhost:$apiPort/health/ready"
Write-Host "  Admin:     http://localhost:$adminPort"
Write-Host "  Swagger:   http://localhost:$apiPort/swagger"
Write-Host "[boot-local] next step: start API and admin, then call /api/lobbies/demo or run .\\tools\\smoke-v26.ps1"
