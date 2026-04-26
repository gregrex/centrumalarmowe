# 03 — Plan naprawczy: 112 Centrum Alarmowe

> Plan oparty na aktualnym stanie kodu, a nie na historycznych brakach.

---

## Etap 1 — Stabilizacja źródeł prawdy

**Cel:** zsynchronizować README, runbooki i audit z realnym stanem repo.

- odświeżyć `README.md`
- poprawić `RUN_LOCAL.md`, `RUN_DOCKER.md`, `DEPLOY.md`, `QA_GUIDE.md`
- utrzymywać kod jako źródło prawdy, dokumentację jako odbicie stanu kodu

---

## Etap 2 — Hardening operacyjny API

**Cel:** czytelna gotowość środowiskowa i lepsze kontrakty operacyjne.

- utrzymywać osobne endpointy:
  - `/health/live`
  - `/health/ready`
  - `/health`
- używać readiness do Docker/integracji, a liveness do prostych probe procesowych
- dopiąć wspólne informacje o store/content readiness

---

## Etap 3 — Content pipeline i smoke readiness

**Cel:** wykrywać realne problemy z danymi wejściowymi, nie tylko brak kilku plików.

- walidować krytyczne katalogi `reference`, `config`, `content`, `ui`, `art`, `audio`
- walidować krytyczne bundla bazowe
- raportować błędy w ścieżkach względnych
- używać `smoke-v26.ps1` zarówno do statycznych checków, jak i do HTTP smoke, gdy API działa

---

## Etap 4 — Deployment readiness

**Cel:** przewidywalne uruchomienie local/dev/stage/prod.

- utrzymywać dev jako szybki start z `InMemorySessionStore`
- stage/prod uruchamiać z:
  - `ASPNETCORE_ENVIRONMENT=Production`
  - `REQUIRE_AUTH=true`
  - silnym `JWT_SIGNING_KEY`
  - skonfigurowanym `ADMIN_USERNAME` / `ADMIN_PASSWORD`
- dopracować runbook dla Docker i reverse proxy
- w kolejnym kroku zautomatyzować migracje DB

---

## Etap 5 — Production scope beyond showcase

**Cel:** przejść z repo showcase/demo do rzeczywistego produktu.

- zastępować demo-data flows produkcyjnymi use-case'ami
- rozszerzać session state machine i reconnect/takeover
- rozbudować panel admina o prawdziwe operacje liveops/content/session management
- dopiąć publiczny surface produktu: landing page i user-facing dashboard
- rozszerzać QA na end-to-end dla scenariuszy operator/admin

---

## Priorytety

| Poziom | Co realizować najpierw |
|---|---|
| **Blocker** | produkcyjna logika sesji, migracje, gotowość wdrożeniowa |
| **High** | health/readiness, content validation, dokumentacja operacyjna |
| **Medium** | admin UX, rozszerzone smoke/e2e, bootstrap demo |
| **Low** | starsze backlogi/historyczne checklisty |
