#!/usr/bin/env bash
set -euo pipefail
ROOT_DIR="$(cd "$(dirname "$0")/.." && pwd)"
export ROOT_DIR

echo "[smoke-vslice] validating vertical slice files"
python3 - <<'PY2'
import json, pathlib, os, sys
root = pathlib.Path(os.environ['ROOT_DIR'])
required = [
    root / 'data/content/vertical-slice.quickplay.json',
    root / 'data/content/incident-type-catalog.v2.json',
    root / 'data/content/dispatch-unit-roster.json',
    root / 'data/content/status-effects.json',
    root / 'data/content/mission-pack-vslice.json',
    root / 'docs/implementation/06_V5_PLAYABLE_VERTICAL_SLICE_SCOPE.md',
    root / 'docs/ui/10_HOME_QUICKPLAY_ENDREPORT_SCREENS.md',
    root / 'docs/gameplay/11_FIRST_3_INCIDENT_TYPES_E2E.md',
    root / 'src/Alarm112.Contracts/SessionReportDto.cs',
    root / 'src/Alarm112.Application/Services/QuickPlayService.cs',
    root / 'client-unity/Assets/Scripts/Runtime/Menu/HomeMenuController.cs',
]
missing = [str(p) for p in required if not p.exists()]
if missing:
    print("\n".join(missing))
    sys.exit(1)
for file in required:
    if file.suffix == '.json':
        json.loads(file.read_text(encoding='utf-8'))
        print(f'OK JSON {file.name}')
    else:
        print(f'OK FILE {file.name}')
print('[smoke-vslice] success')
PY2
