#!/usr/bin/env bash
set -euo pipefail

if sudo mysqladmin ping --silent >/dev/null 2>&1; then
  exit 0
fi

sudo mkdir -p /var/run/mysqld
sudo chown mysql:mysql /var/run/mysqld

sudo mysqld_safe \
  --datadir=/var/lib/mysql \
  --socket=/var/run/mysqld/mysqld.sock \
  --pid-file=/var/run/mysqld/mysqld.pid \
  >/tmp/mysqld_safe.log 2>&1 &

for i in $(seq 1 30); do
  if sudo mysqladmin ping --silent >/dev/null 2>&1; then
    exit 0
  fi
  sleep 1
done

echo "MySQL failed to start" >&2
exit 1
