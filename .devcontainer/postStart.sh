#!/usr/bin/env bash
set -euo pipefail

# Wait for MySQL container to be ready
for i in $(seq 1 30); do
  if mysqladmin ping -h db -u root -prootpassword --silent >/dev/null 2>&1; then
    exit 0
  fi
  sleep 1
done

echo "MySQL container failed to start" >&2
exit 1
