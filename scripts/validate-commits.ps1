param(
  [string]$From = 'origin/main',
  [string]$To = 'HEAD'
)

$regex = '^(feat|fix|chore|docs|refactor|test|perf)(\([a-z0-9_-]+\))?:\s.+$'

$commits = git rev-list --no-merges "$From..$To" 2>$null
if (-not $commits) {
  Write-Host "No commits to check between $From and $To"
  exit 0
}

$failed = $false
$commits -split "`n" | ForEach-Object {
  $sha = $_.Trim()
  if ($sha) {
    $msg = git log --format=%B -n 1 $sha | Select-Object -First 1
    if (-not ($msg -match $regex)) {
      Write-Host "INVALID: $sha"
      Write-Host "  $msg"
      $failed = $true
    } else {
      Write-Host "OK: $sha"
    }
  }
}

if ($failed) {
  Write-Error "One or more commit messages failed validation"
  exit 1
}