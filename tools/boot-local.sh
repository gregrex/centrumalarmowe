
#!/usr/bin/env bash
set -euo pipefail
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
echo "[boot-local] validating content"
bash "$ROOT_DIR/tools/content-verify.sh"
echo "[boot-local] running vertical slice smoke"
bash "$ROOT_DIR/tools/smoke-vslice.sh"
echo "[boot-local] endpoints (expected after app start):"
echo "  API:     http://localhost:${API_PORT:-8080}"
echo "  Admin:   http://localhost:${ADMIN_PORT:-8081}"
echo "  Swagger: http://localhost:${API_PORT:-8080}/swagger"
echo "[boot-local] next step: start API, call /api/quickplay/bootstrap, then /api/quickplay/start"
