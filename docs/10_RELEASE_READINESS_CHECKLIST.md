# 10 — Release Readiness Checklist: 112 Centrum Alarmowe

> Checklist produkcyjny przed deploymentem v26+.  
> Aktualizacja: Sesja 6 audytowa.

---

## 1. Bezpieczeństwo

### Krytyczne

- [x] `Security:RequireAuth = true` w `appsettings.Production.json` ✅
- [x] `Security:EnableDevTokenEndpoint = false` w produkcji ✅
- [x] JWT Signing Key: losowy, ≥ 32 znaki, **poza repo** (env var / secret manager) ✅
- [x] JWT Signing Key: brak hardcoded wartości w repo (random per-startup gdy brak konfiguracji) ✅
- [x] Admin panel (`/admin` na porcie 5081) wymaga uwierzytelniania (Basic Auth) ✅
- [x] PostgreSQL: dedykowany user (`alarm112app`), hasło z env var, poza repo ✅
- [x] Redis: hasło ustawione (`requirepass ${REDIS_PASSWORD}`), port za siecią internal ✅

### Wysokie

- [x] Security headers dodane: `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy`, `CSP` ✅
- [x] Swagger UI wyłączony w produkcji (`Swagger:Enabled=false` w Production) ✅
- [x] CORS: ograniczony do konkretnych origin (nie `*`), metody ograniczone w prod ✅
- [x] Rate limiter podpięty do endpointów POST (`RequireRateLimiting("fixed")`) ✅
- [x] `AllowedHosts` ustawiony: `localhost;127.0.0.1` (dev), `localhost` (prod) ✅

### Średnie

- [x] Redis auth skonfigurowany w `docker-compose.yml` (`requirepass`) ✅
- [x] Audit logging włączony (AuditLoggingMiddleware — POST/PUT/PATCH/DELETE) ✅
- [x] SignalR hub wymaga uwierzytelniania w produkcji (`[Authorize]` + default policy) ✅
- [x] Network isolation: db/redis w sieci `internal`, API/admin w `external` ✅

---

## 2. Walidacje

- [x] `SessionActionDto`: walidacja wszystkich pól (Required, StringLength, AllowedValues) ✅
- [x] `DispatchCommandDto`: walidacja (Required, Pattern) ✅
- [x] `QuickPlayStartRequestDto`: walidacja (Mode enum, Role enum) ✅
- [x] `SharedActionDto`: walidacja (Required, AllowedValues, Range) ✅
- [x] `RoutePreviewRequestDto`: walidacja (Required, Pattern, AllowedValues) ✅
- [x] Endpoint `/api/sessions/{sessionId}/*`: walidacja formatu sessionId (Regex) ✅
- [x] Payload JSON: limit rozmiaru 1KB (`StringLength(1024)`) ✅
- [x] Odpowiedzi błędów walidacji: format ProblemDetails (ValidationProblem) ✅
- [x] Brak wycieków stacktrace w odpowiedziach produkcyjnych ✅

---

## 3. Testy

- [x] Build: 0 błędów, 0 warnings: `dotnet build Alarm112.sln` ✅
- [x] Wszystkie testy integracyjne przechodzą: 125/125 ✅
- [x] Testy z auth włączonym: 401 na brak tokena ✅
- [x] Testy walidacji: 400 na błędne DTO ✅
- [x] Security headers: testy response headers ✅
- [x] JWT expiry, wrong key, no-role token tests ✅
- [x] Injection payload tests (XSS, SQL injection, path traversal) ✅
- [x] Rate limiting test ✅
- [x] SignalR hub tests ✅
- [x] InMemorySessionStore unit tests ✅
- [x] PostgresSessionStore unit tests ✅
- [x] BotDirector unit tests ✅
- [x] E2E Playwright specs istnieją dla auth, landing, user dashboard i admin route split ✅
- [ ] Smoke test v26 przechodzi: `tools/smoke-v26.ps1` ⚠️ (wymaga działającego API)
- [ ] Content bundle walidacja: `tools/content-verify.ps1` ⚠️ (środowiskowa)

---

## 4. Konfiguracja i sekrety

- [x] `appsettings.Production.json` istnieje z bezpiecznymi defaultami ✅
- [x] `infra/.env.example` istnieje z opisem wymaganych zmiennych ✅
- [x] `infra/.env.production.example` istnieje dla stage/prod ✅
- [x] `infra/docker-compose.yml`: brak hardcoded haseł (wszystkie z env vars) ✅
- [x] `docker-compose.override.yml.example` istnieje ✅
- [x] `.gitignore` zawiera: `.env`, `docker-compose.override.yml` ✅
- [ ] Produkcyjne sekrety zarządzane poza repozytorium (Vault / GH Secrets) ⚠️ operacyjne

---

## 5. Infrastruktura

- [x] Docker: non-root user (`USER appuser`) w obu Dockerfile ✅
- [ ] Docker: .NET stable (nie `10.0-preview`) — czekamy na .NET 10 GA ⚠️
- [x] Health endpoints dostępne i działające: `/health` ✅
- [x] `PostgresSessionStore` zaimplementowany i podpięty w DI ✅
- [x] `PostgresSessionStore.TryGet` zaimplementowany ✅
- [ ] Migracje SQL uruchomione przed startem (001–021) ⚠️ operacyjne
- [x] Redis auth skonfigurowany w docker-compose ✅
- [ ] Backup bazy: automatyczny ⚠️ operacyjne/DevOps

---

## 6. CI/CD

- [x] GitHub Actions `ci.yml`: restore → build → test → content-verify ✅
- [x] GitHub Actions: docker-build job (API + AdminWeb) ✅
- [x] GitHub Actions: e2e-api i e2e-admin jobs ✅
- [x] Code coverage upload ✅
- [x] Pipeline nie commituje sekretów ✅
- [ ] Push do registry (ghcr.io) — wymaga konfiguracji produkcyjnej ⚠️

---

## 7. Kod i architektura

- [x] Build: 0 błędów, 0 warnings ✅
- [x] `BotTickHostedService` iteruje store, nie używa hardcoded `"DEMO112"` ✅
- [x] `BotTickHostedService` loguje błędy zamiast swallowing (ILogger) ✅
- [x] `InMemorySessionStore.TryGet` zaimplementowany ✅
- [x] `SessionService` loguje błędy JSON (ILogger, catch(JsonException)) ✅
- [x] AdminWeb JS krytycznych widoków używa bezpiecznych podstawień tekstowych dla danych runtime ✅
- [x] AdminWeb: Basic Auth fail-fast (brak hasła → startup exception) ✅
- [x] AdminWeb dashboard: same-origin `/api/admin/dashboard` + server-side JWT do chronionych metryk API ✅
- [x] Korelacja requestów (X-Correlation-Id middleware) ✅
- [x] `SessionService.GetSnapshotAsync` zwraca 404 dla nieistniejącej sesji ✅
- [x] ContentValidationService: używa IContentBundleLoader zamiast raw paths ✅
- [x] CORS: bezpieczny default ograniczony do `GET,POST` zamiast `AllowAnyMethod` ✅

---

## 8. Dokumentacja

- [x] `README.md` zaktualizowany ✅
- [x] `docs/00_AUDYT_STARTOWY.md` ✅
- [x] `docs/01_ARCHITEKTURA_AKTUALNA.md` ✅
- [x] `docs/02_RYZYKA_BEZPIECZENSTWA.md` ✅ (27 ryzyk sklasyfikowanych)
- [x] `docs/03_PLAN_NAPRAWCZY.md` ✅
- [x] `docs/04_STANDARDY_UI_UX.md` ✅
- [x] `docs/05_STANDARDY_WALIDACJI.md` ✅
- [x] `docs/06_STRATEGIA_TESTOW.md` ✅
- [x] `docs/07_HARDENING_BAZY_I_KONFIGURACJI.md` ✅
- [x] `docs/08_PIPELINE_I_DEPLOY.md` ✅
- [x] `docs/09_BACKLOG_REFAKTORYZACJI.md` ✅
- [x] `docs/10_RELEASE_READINESS_CHECKLIST.md` ✅ (ten plik)
- [x] `docs/11_POSTEP_PRAC.md` — aktualny ✅

---

## 9. Operacyjność

- [x] Logi: structured JSON via Serilog + CompactJsonFormatter w produkcji ✅
- [x] Poziom logów: `Information` (nie `Debug`) w produkcji ✅
- [x] Audit log: method/path/status/actor/role/duration na każdym mutującym request ✅
- [x] Correlation ID: X-Correlation-Id na każdym request ✅
- [x] Monitoring: health check endpoint dostępny (`/health`) ✅
- [x] Graceful shutdown: CancellationToken propagowany ✅
- [x] Structured logging JSON format (Serilog) ✅
- [ ] Metryki Prometheus ⚠️ future

---

## Status ogólny

| Obszar | Gotowość | Blokery |
|--------|---------|---------|
| Bezpieczeństwo | 🟢 95% | SEC-08 CORS częściowe (akceptowalne) |
| Walidacje | 🟢 97% | Brak krytycznych blockerów w API session lookup |
| Testy | 🟢 92% | Playwright runtime zależy od lokalnego bootstrapu serwerów |
| Konfiguracja | 🟢 90% | Produkcyjne sekrety — operacyjne |
| Infrastruktura | 🟡 75% | .NET preview, migracje operacyjne |
| CI/CD | 🟢 85% | Registry push konfiguracja |
| Dokumentacja | 🟢 100% | ✅ |
| **Ogółem** | **🟢 ~93%** | **Gotowy na staging, bliski produkcji** |

**Szacowany nakład do pełnej produkcji:** nadal głównie operacyjny i produktowy: sekrety, migracje, .NET GA, pełniejszy frontend i scenariusze live.

---

## 8. Dokumentacja

- [ ] `README.md` zaktualizowany z instrukcją uruchomienia
- [ ] `docs/00_AUDYT_STARTOWY.md` ✅
- [ ] `docs/01_ARCHITEKTURA_AKTUALNA.md` ✅
- [ ] `docs/02_RYZYKA_BEZPIECZENSTWA.md` ✅
- [ ] `docs/03_PLAN_NAPRAWCZY.md` ✅
- [ ] `docs/04_STANDARDY_UI_UX.md` ✅
- [ ] `docs/05_STANDARDY_WALIDACJI.md` ✅
- [ ] `docs/06_STRATEGIA_TESTOW.md` ✅
- [ ] `docs/07_HARDENING_BAZY_I_KONFIGURACJI.md` ✅
- [ ] `docs/08_PIPELINE_I_DEPLOY.md` ✅
- [ ] `docs/09_BACKLOG_REFAKTORYZACJI.md` ✅
- [ ] `docs/10_RELEASE_READINESS_CHECKLIST.md` ✅ (ten plik)
- [ ] `docs/11_POSTEP_PRAC.md` — aktualny

---

## 9. Operacyjność

- [x] Logi: structured JSON logging w produkcji
- [ ] Poziom logów: `Information` (nie `Debug`) w produkcji
- [ ] Logowanie: brak logowania danych wrażliwych (hasła, tokeny)
- [ ] Monitoring: health check endpoint dostępny dla load balancera
- [ ] Graceful shutdown: CancellationToken propagowany

---

## Status ogólny

| Obszar | Gotowość | Blokery |
|--------|---------|---------|
| Bezpieczeństwo | 🔴 30% | SEC-01 do SEC-05 krytyczne |
| Walidacje | 🟠 20% | Brak DataAnnotations |
| Testy | 🟡 40% | Tylko happy path, brak security/negative |
| Konfiguracja | 🟠 40% | Brak prod appsettings, hardcoded secrets |
| Infrastruktura | 🟡 60% | Docker OK, brak CI/CD, InMemory store |
| Dokumentacja | 🟢 90% | Ten zestaw 11 plików ✅ |
| **Ogółem** | **🔴 40%** | **Nie gotowy na produkcję** |

**Szacowany nakład do produkcji:** ~15 dni deweloperskich
