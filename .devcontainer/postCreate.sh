#!/usr/bin/env bash
set -euo pipefail

# Wait for MySQL to be ready
echo "Waiting for MySQL to be ready..."
for i in $(seq 1 30); do
  if mysqladmin ping -h db -u root -prootpassword --silent >/dev/null 2>&1; then
    echo "MySQL is ready!"
    break
  fi
  sleep 1
done

# Install dotnet-ef if not already available
if ! command -v dotnet-ef >/dev/null 2>&1; then
  dotnet tool install --global dotnet-ef
fi

export PATH="$PATH:$HOME/.dotnet/tools"

# Restore and build
dotnet restore

# Run migrations
dotnet ef database update

# Seed data if exists
SEED_FILE="Database/Scripts/seed_data.sql"
if [ -f "$SEED_FILE" ]; then
  echo "Applying seed data from $SEED_FILE..."
  mysql -h db -u root -prootpassword controle_estoque < "$SEED_FILE" || echo "Warning: Seed data may have failed"
  echo "Seed data applied!"
else
  echo "Warning: Seed file not found at $SEED_FILE"
fi
