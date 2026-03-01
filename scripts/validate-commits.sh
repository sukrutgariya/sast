#!/usr/bin/env bash
set -euo pipefail

REGEX='^(feat|fix|chore|docs|refactor|test|perf)(\([a-z0-9_-]+\))?:\s.+$'

FROM=${1:-origin/main}
TO=${2:-HEAD}

# list commits in range
commits=$(git rev-list --no-merges "$FROM".."$TO" || true)
if [ -z "$commits" ]; then
  echo "No commits to check between $FROM and $TO"
  exit 0
fi

failed=0
while read -r sha; do
  msg=$(git log --format=%B -n 1 "$sha" | sed -n '1p')
  if ! echo "$msg" | grep -Pq "$REGEX"; then
    echo "INVALID: $sha"
    echo "  $msg"
    failed=1
  else
    echo "OK: $sha"
  fi
done <<< "$commits"

if [ "$failed" -ne 0 ]; then
  echo "One or more commit messages failed validation"
  exit 1
fi