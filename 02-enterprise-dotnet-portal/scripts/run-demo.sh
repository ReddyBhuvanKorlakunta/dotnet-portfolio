#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")/.."
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_USE_POLLING_FILE_WATCHER=1
: "${Auth__Password:?Set Auth__Password to a unique password of at least 14 characters}"
: "${Encryption__Key:?Set Encryption__Key to a base64 encoded random 32-byte key}"
dotnet run --project src/EnterprisePortal.Web -- --urls http://127.0.0.1:5080
