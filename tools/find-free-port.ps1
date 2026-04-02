#!/usr/bin/env pwsh
# find-free-port.ps1 — Check if ports from .env are available, suggest alternatives if not
# Usage: .\tools\find-free-port.ps1

Set-StrictMode -Version Latest

$root = Split-Path -Parent $PSScriptRoot
$envFile = Join-Path $root ".env"

function Test-PortFree([int]$port) {
    $listener = $null
    try {
        $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, $port)
        $listener.Start()
        return $true
    } catch {
        return $false
    } finally {
        $listener?.Stop()
    }
}

function Find-FreePort([int]$startPort) {
    $port = $startPort
    while (-not (Test-PortFree $port)) { $port++ }
    return $port
}

$ports = @{
    API_PORT    = 5080
    ADMIN_PORT  = 5081
    DB_PORT     = 5432
    REDIS_PORT  = 6379
    GATEWAY_PORT = 5090
}

# Override from .env if present
if (Test-Path $envFile) {
    Get-Content $envFile | ForEach-Object {
        if ($_ -match "^\s*([^#=]+)=(\d+)\s*$") {
            $key = $Matches[1].Trim()
            if ($ports.ContainsKey($key)) { $ports[$key] = [int]$Matches[2] }
        }
    }
}

$results = [System.Collections.Generic.List[PSCustomObject]]::new()
$hasConflict = $false

foreach ($name in $ports.Keys | Sort-Object) {
    $desired = $ports[$name]
    $free = Test-PortFree $desired
    $suggestion = if (-not $free) { Find-FreePort $desired } else { $desired }
    if (-not $free) { $hasConflict = $true }

    $results.Add([PSCustomObject]@{
        Variable   = $name
        Desired    = $desired
        Available  = if ($free) { "YES" } else { "NO" }
        Suggestion = $suggestion
    })
}

$results | Format-Table -AutoSize

if ($hasConflict) {
    Write-Host "Port conflicts detected. Update .env with suggested values above." -ForegroundColor Yellow
    exit 1
} else {
    Write-Host "All ports are free. No conflicts." -ForegroundColor Green
    exit 0
}
