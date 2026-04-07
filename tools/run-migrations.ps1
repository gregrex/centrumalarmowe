param(
  [string]$ComposeFile = '',
  [string]$Service = 'db',
  [string]$Database = '',
  [string]$Username = '',
  [switch]$ListOnly
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$schemaDir = Join-Path $root 'db\schema'

if ([string]::IsNullOrWhiteSpace($ComposeFile)) {
  $ComposeFile = Join-Path $root 'infra\docker-compose.yml'
}

function Get-DotEnvValues {
  param([string]$Path)

  $values = @{}
  if (-not (Test-Path $Path)) {
    return $values
  }

  foreach ($line in Get-Content $Path) {
    $trimmed = $line.Trim()
    if (-not $trimmed -or $trimmed.StartsWith('#')) {
      continue
    }

    $parts = $trimmed -split '=', 2
    if ($parts.Count -ne 2) {
      continue
    }

    $values[$parts[0].Trim()] = $parts[1].Trim()
  }

  return $values
}

$envFile = Join-Path $root 'infra\.env'
$exampleEnvFile = Join-Path $root 'infra\.env.example'
$envValues = Get-DotEnvValues -Path $(if (Test-Path $envFile) { $envFile } else { $exampleEnvFile })

if ([string]::IsNullOrWhiteSpace($Database)) {
  $Database = if ($env:POSTGRES_DB) { $env:POSTGRES_DB } elseif ($envValues.ContainsKey('POSTGRES_DB')) { $envValues['POSTGRES_DB'] } else { 'alarm112' }
}

if ([string]::IsNullOrWhiteSpace($Username)) {
  $Username = if ($env:POSTGRES_USER) { $env:POSTGRES_USER } elseif ($envValues.ContainsKey('POSTGRES_USER')) { $envValues['POSTGRES_USER'] } else { 'alarm112app' }
}

if (-not (Test-Path $schemaDir)) {
  throw "Schema directory not found: $schemaDir"
}

$files = Get-ChildItem -Path $schemaDir -Filter '*.sql' | Sort-Object Name
if ($files.Count -eq 0) {
  throw "No SQL migrations found in: $schemaDir"
}

Write-Host "[run-migrations] compose file: $ComposeFile"
Write-Host "[run-migrations] service: $Service"
Write-Host "[run-migrations] database: $Database"
Write-Host "[run-migrations] username: $Username"
Write-Host "[run-migrations] migrations found: $($files.Count)"

if ($ListOnly) {
  foreach ($file in $files) {
    Write-Host " - $($file.Name)"
  }
  Write-Host "[run-migrations] list-only complete"
  exit 0
}

if (-not (Test-Path $ComposeFile)) {
  throw "Compose file not found: $ComposeFile"
}

foreach ($file in $files) {
  Write-Host "[run-migrations] applying $($file.Name)"
  Get-Content $file.FullName -Raw |
    docker compose -f $ComposeFile exec -T $Service psql -v ON_ERROR_STOP=1 -U $Username -d $Database | Out-Host

  if ($LASTEXITCODE -ne 0) {
    throw "Migration failed: $($file.Name)"
  }
}

Write-Host "[run-migrations] success"
