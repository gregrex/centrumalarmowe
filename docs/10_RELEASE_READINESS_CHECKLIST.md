# 10 — Release Readiness Checklist: 112 Centrum Alarmowe

> Checklist produkcyjny przed deploymentem v26+.  
> Każda pozycja musi być ✅ przed release.

---

## 1. Bezpieczeństwo

### Krytyczne

- [ ] `Security:RequireAuth = true` w `appsettings.Production.json`
- [ ] `Security:EnableDevTokenEndpoint = false` w produkcji
- [ ] JWT Signing Key: losowy, ≥ 32 znaki, **poza repo** (env var / secret manager)
- [ ] JWT Signing Key zmieniony z wartości domyślnej `dev-only-*`
- [ ] Admin panel (`/` na porcie 5081) wymaga uwierzytelniania
- [ ] PostgreSQL: dedykowany user (nie `postgres`), hasło silne, poza repo
- [ ] Redis: hasło ustawione, port nie wystawiony publicznie

### Wysokie

- [ ] Security headers dodane: `X-Frame-Options`, `X-Content-Type-Options`, `Referrer-Policy`, `CSP`
- [ ] Swagger UI wyłączony w produkcji (lub za auth)
- [ ] CORS: ograniczony do konkretnych origin (nie localhost)
- [ ] Rate limiter podpięty do endpointów POST (`RequireRateLimiting("fixed")`)
- [ ] `AllowedHosts` ustawiony na domenę produkcyjną

### Średnie

- [ ] Redis auth skonfigurowany w `docker-compose.yml`
- [ ] Logi bezpieczeństwa włączone (auth failures, rate limit hits)
- [ ] SignalR hub wymaga uwierzytelniania w produkcji

---

## 2. Walidacje

- [ ] `SessionActionDto`: walidacja wszystkich pól (Required, StringLength, AllowedValues)
- [ ] `DispatchCommandDto`: walidacja (Required, Pattern)
- [ ] `QuickPlayStartRequestDto`: walidacja (Mode enum, Role enum)
- [ ] Endpoint `/api/sessions/{sessionId}/*`: walidacja formatu sessionId
- [ ] Payload JSON: limit rozmiaru 1KB
- [ ] Odpowiedzi błędów walidacji: format ProblemDetails (RFC 7807)
- [ ] Brak wycieków stacktrace w odpowiedziach produkcyjnych

---

## 3. Testy

- [ ] Wszystkie testy integracyjne przechodzą: `dotnet test Alarm112.sln`
- [ ] Testy z auth włączonym: 401 na brak tokena
- [ ] Testy walidacji: 400 na błędne DTO
- [ ] Security headers: testy response headers
- [ ] Smoke test v26 przechodzi: `tools/smoke-v26.ps1`
- [ ] Content bundle walidacja: `tools/content-verify.ps1`

---

## 4. Konfiguracja i sekrety

- [ ] `appsettings.Production.json` istnieje z bezpiecznymi defaultami
- [ ] `.env.production.example` istnieje z opisem wymaganych zmiennych
- [ ] `docker-compose.override.yml` z produkcyjnymi secretami (gitignored)
- [ ] `infra/docker-compose.yml`: brak hardcoded haseł
- [ ] `.gitignore` zawiera: `.env`, `*.Production.json`, `docker-compose.override.yml`
- [ ] User secrets NIE trafią do repozytorium

---

## 5. Infrastruktura

- [ ] Docker: non-root user (`USER appuser`) w obu Dockerfile
- [ ] Docker: .NET stable (nie `10.0-preview`) gdy dostępny
- [ ] Health endpoints dostępne i działające: `/health`
- [ ] PostgreSQL podłączony (zastąpiono InMemoryStore)
- [ ] Migracje SQL uruchomione (001–021)
- [ ] Redis połączony z aplikacją (jeśli używany)
- [ ] Backup bazy: automatyczny

---

## 6. CI/CD

- [ ] GitHub Actions `ci.yml`: restore → build → test → content-verify
- [ ] GitHub Actions `docker.yml`: build → push → smoke
- [ ] Pipeline nie commituje sekretów
- [ ] Smoke tests przechodzą w CI

---

## 7. Kod i architektura

- [ ] Build: 0 błędów, 0 warnings: `dotnet build Alarm112.sln`
- [ ] `BotTickHostedService` nie używa hardcoded `"DEMO112"` (pobiera z store)
- [ ] `SessionService.GetSnapshotAsync` zwraca 404 dla nieistniejącej sesji
- [ ] Brak `TODO:` komentarzy w krytycznych ścieżkach

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

- [ ] Logi: structured JSON logging w produkcji
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
