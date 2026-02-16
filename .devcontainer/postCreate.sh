#!/usr/bin/env bash
set -euo pipefail

bash .devcontainer/postStart.sh

DB_NAME=$(grep -oP 'Database=\K[^;" ]+' appsettings.Development.json | head -n 1 || true)
DB_USER=$(grep -oP 'User=\K[^;" ]+' appsettings.Development.json | head -n 1 || true)
DB_PASS=$(grep -oP 'Password=\K[^;" ]+' appsettings.Development.json | head -n 1 || true)

DB_NAME=${DB_NAME:-controle_estoque}
DB_USER=${DB_USER:-erikalima}
DB_PASS=${DB_PASS:-erikalima}

sudo mysql -e "CREATE DATABASE IF NOT EXISTS ${DB_NAME} CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;" 
sudo mysql -e "CREATE USER IF NOT EXISTS '${DB_USER}'@'localhost' IDENTIFIED BY '${DB_PASS}';" 
sudo mysql -e "GRANT ALL PRIVILEGES ON ${DB_NAME}.* TO '${DB_USER}'@'localhost'; FLUSH PRIVILEGES;"

if ! command -v dotnet-ef >/dev/null 2>&1; then
  dotnet tool install --global dotnet-ef
fi

export PATH="$PATH:$HOME/.dotnet/tools"

dotnet restore

dotnet ef database update

if [ -f Database/Scripts/seed_data.sql ]; then
  sudo mysql "${DB_NAME}" < Database/Scripts/seed_data.sql
fi
