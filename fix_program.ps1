# Quick PowerShell script to fix the malformed Program.cs
$file = 'c:\projekty\centrumalarmowe\src\Alarm112.Api\Program.cs'
$content = Get-Content $file -Raw

# Find and remove the malformed duplicate code after line 70
# The offending lines start with "    // When Security:RequireAuth=false" and end before "// CORS"
$pattern = "(?m)    // When Security:RequireAuth=false.+?{.+?Build\(\);.+?}.+?\n\n    options\.AddPolicy.*?\".+?RequireUserAttribute.\+\);.+?\}\);\n"
$result = [regex]::Replace($content, $pattern, "")

# If that didn't work, try a simpler approach: remove lines 71-92 manually
$lines = $content -split "`n"
$newLines = @()
$skipUntilCors = $false
for ($i = 0; $i -lt $lines.Count; $i++) {
    $line = $lines[$i]
    
    # Start skipping at the malformed "// When Security" line
    if ($line -like "    // When Security:RequireAuth=false*") {
        $skipUntilCors = $true
    }
    # Stop skipping at the "// CORS" comment
    if ($line -like "// CORS*") {
        $skipUntilCors = $false
    }
    # Add the line if not in the skip zone, or if it's the CORS line itself
    if (!$skipUntilCors) {
        $newLines += $line
    }
}

$newContent = $newLines -join "`n"
Set-Content -Path $file -Value $newContent
Write-Host "Fixed Program.cs"
