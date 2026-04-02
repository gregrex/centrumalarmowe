$root = Split-Path -Parent $PSScriptRoot
Write-Host "[boot-local] validating content"
& "$root/tools/content-verify.ps1"
Write-Host "[boot-local] endpoints (expected after app start):"
Write-Host "  API:     http://localhost:$($env:API_PORT ?? 8080)"
Write-Host "  Admin:   http://localhost:$($env:ADMIN_PORT ?? 8081)"
Write-Host "  Swagger: http://localhost:$($env:API_PORT ?? 8080)/swagger"
Write-Host "[boot-local] next step: start API and admin, then call /api/lobbies/demo"
