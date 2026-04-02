# E2E Tests — Alarm112 (Playwright)

Playwright E2E test suite for the `112: Centrum Alarmowe` backend API and AdminWeb panel.

## Wymagania

- Node.js 20+ (sprawdź: `node --version`)
- Uruchomione lokalne API na `http://localhost:5080`
- Uruchomiony AdminWeb na `http://localhost:5081` (opcjonalnie)

## Instalacja

```powershell
cd tests/e2e
npm install
npx playwright install chromium
```

## Uruchomienie testów

### Wszystkie testy (API + Admin)
```powershell
npm test
```

### Tylko testy API
```powershell
npm run test:api
```

### Tylko testy AdminWeb
```powershell
npm run test:admin
```

### W trybie headed (widoczna przeglądarka)
```powershell
npm run test:headed
```

## Raporty

Artefakty zapisywane są do `/artifacts/e2e/`:
- `/artifacts/e2e/report/` — HTML report (otwórz `index.html`)
- `/artifacts/e2e/results.json` — wyniki JSON
- `/artifacts/e2e/results/` — traces i screenshoty (tylko przy niepowodzeniu)

Otwórz raport HTML:
```powershell
npm run test:report
```

## Zmienne środowiskowe

| Zmienna | Domyślna | Opis |
|---|---|---|
| `API_BASE` | `http://localhost:5080` | Adres API |
| `ADMIN_BASE` | `http://localhost:5081` | Adres AdminWeb |

Przykład z nadpisaniem:
```powershell
$env:API_BASE="http://localhost:5090"; npm test
```

## Struktura testów

```
tests/e2e/
  tests/
    api/
      health.spec.ts      — /health endpoint (4 testy)
      sessions.spec.ts    — sesja demo, snapshot, akcje (6 testów)
      content.spec.ts     — content/reference data endpoints (20+ testów)
      lobby.spec.ts       — lobby endpoints (4 testy)
      errors.spec.ts      — obsługa błędów, Swagger UI (5 testów)
      flow.spec.ts        — pełny przepływ sesji (3 testy)
    admin/
      admin-panel.spec.ts — AdminWeb dashboard (8 testów)
  playwright.config.ts
  package.json
```

## Konfiguracja dla CI

```yaml
- name: Install e2e deps
  run: cd tests/e2e && npm ci && npx playwright install --with-deps chromium
- name: Run E2E
  run: cd tests/e2e && npm test
  env:
    API_BASE: http://localhost:5080
    ADMIN_BASE: http://localhost:5081
- name: Upload artifacts
  uses: actions/upload-artifact@v4
  with:
    path: artifacts/e2e/
```
