# Audit Plan — 112: Centrum Alarmowe

> Data: 2026-03-29  
> Wersja: v26  
> Autor: GitHub Copilot Autonomous Agent

---

## Priorytety

- **P0** — Blokuje zielony pipeline lub narusza fundamentalne wymagania produkcyjne. Naprawić natychmiast.
- **P1** — Poważna luka funkcjonalna lub jakościowa. Naprawić przed zamknięciem wersji.
- **P2** — Ulepszone, ale nieblokujące. Naprawić gdy czas pozwala.

---

## BLOK A — TESTY I JAKOŚĆ BUDOWANIA

### A-P0-01 — Brakujący projekt testowy API

**Problem:** Brak jakiegokolwiek projektu `*.Tests.csproj`. `verify.ps1` to placeholder.  
**Ryzyko:** Brak zielonego CI gate. Nie wiadomo, czy API działa end-to-end.  
**Rozwiązanie:** Utwórz `tests/Alarm112.Api.Tests/` z `WebApplicationFactory` + `xUnit`. Pokryj `/health`, kluczowe GET endpoints.  
**Estymacja:** 2-3h  
**Status:** ❌ MISSING → do naprawy  

### A-P0-02 — `verify.ps1` jest placeholderem

**Problem:** `tools/verify.ps1` drukuje tylko teksty. Nie wykonuje `dotnet restore`, `dotnet build`, `dotnet test`.  
**Ryzyko:** Brak automatycznej weryfikacji lokalnej.  
**Rozwiązanie:** Zaimplementuj prawdziwy skrypt: restore → build → test → raport exit code.  
**Estymacja:** 30 min  
**Status:** ❌ MISSING → do naprawy  

### A-P0-03 — ContentValidationService odwołuje się do nieistniejących plików

**Problem:** `ContentValidationService.cs` sprawdza `data/art/icon_catalog.json` i `sprite_atlas_manifest.json`. Oba istnieją — ✅. Jednak ścieżki do `data/reference/reference-data.json` wymagają weryfikacji środowiskowej.  
**Ryzyko:** API może zwrócić błędne wyniki walidacji.  
**Rozwiązanie:** Zweryfikować ścieżki dynamicznie. Dodać test integracyjny.  
**Estymacja:** 30 min  

### A-P0-04 — Smoke testy sprawdzają tylko obecność plików

**Problem:** Wszystkie `smoke-vXX.ps1` to tylko `Test-Path` checks. Nie testują rzeczywistego zachowania API.  
**Ryzyko:** Zielony smoke ≠ działający system.  
**Rozwiązanie:** Rozszerzyć `smoke-v26.ps1` o HTTP smoke test (wywołanie `/health` i kilku kluczowych endpointów).  
**Estymacja:** 1h  

### A-P1-05 — Brak E2E Playwright

**Problem:** Brak żadnych testów E2E dla AdminWeb ani API.  
**Ryzyko:** Brak weryfikacji przepływów użytkownika.  
**Rozwiązanie:** Utwórz `tests/e2e/` projekt Playwright z testami: health check, swagger dostępny, key API endpoints zwracają 200.  
**Estymacja:** 2h  

---

## BLOK B — API — POPRAWKI KRYTYCZNE

### B-P0-06 — Brak CORS w API

**Problem:** API nie ma `AddCors` / `UseCors`. Klient Unity (WebGL/Android) nie może wykonać cross-origin requests.  
**Ryzyko:** Unity client nie może się połączyć z API przez HTTP.  
**Rozwiązanie:** Dodaj CORS policy w `Program.cs` z parametryzacją przez appsettings/env.  
**Estymacja:** 30 min  

### B-P0-07 — Brak globalnego exception handler

**Problem:** Nieobsłużony wyjątek w serwisie → 500 Internal Server Error bez struktury DTO.  
**Ryzyko:** Klient dostaje HTML stack trace lub pustą odpowiedź zamiast `{ "error": "..." }`.  
**Rozwiązanie:** Dodaj `UseExceptionHandler` / `IProblemDetailsService` w `Program.cs`.  
**Estymacja:** 30 min  

### B-P0-08 — Health endpoint pokazuje v25 zamiast v26

**Problem:** `/health` zwraca `"version": "v25"`.  
**Ryzyko:** Dezorientacja podczas weryfikacji.  
**Rozwiązanie:** Zmień na `v26` w `Program.cs`.  
**Estymacja:** 5 min  

### B-P1-09 — `SessionService.ApplyActionAsync` ma TODO — brak real action dispatch

**Problem:** Każda akcja gracza zawsze zwraca `true` bez zmiany stanu.  
**Ryzyko:** Cała mechanika akcji gracza niedziałająca.  
**Rozwiązanie:** Zaimplementuj basic dispatch router: `dispatch`, `escalate`, `report`. Eventy przez `SessionHub`.  
**Estymacja:** 2h  

### B-P1-10 — `BotDirector` symuluje ale nie mutuje stanu sesji

**Problem:** Bot tick tworzy `SessionActionDto` ale nigdy go nie aplikuje.  
**Ryzyko:** Boty są dekoracją, nie działają.  
**Rozwiązanie:** Wywołaj `SessionService.ApplyActionAsync` w `BotDirector.ExecuteBotTickAsync`.  
**Estymacja:** 1h  

### B-P1-11 — Brak rate limiting w API

**Problem:** Brak limitowania żądań - ryzyko DoS.  
**Rozwiązanie:** Dodaj `AddRateLimiter` z fixed window policy (np. 100 req/10s per IP).  
**Estymacja:** 30 min  

### B-P1-12 — Brak structured logging z Correlation ID

**Problem:** Logi bez korelacji request→response. Trudne debugowanie.  
**Rozwiązanie:** Dodaj serilog (lub wbudowany enrichment) + `X-Correlation-Id` header middleware.  
**Estymacja:** 1h  

---

## BLOK C — INFRASTRUKTURA

### C-P0-13 — `docker-verify.ps1` bez smoke po up

**Problem:** `docker-verify.ps1` robi `up → ps → down` bez sprawdzenia czy API żyje.  
**Rozwiązanie:** Dodaj `Invoke-RestMethod` na `http://localhost:${API_PORT}/health` po `up`.  
**Estymacja:** 30 min  

### C-P1-14 — Brak healthchecks w docker-compose

**Problem:** Docker nie wie, kiedy API/Admin/DB jest gotowe. Zależności `depends_on` bez warunków.  
**Rozwiązanie:** Dodaj `healthcheck:` dla każdego serwisu + `condition: service_healthy` w `depends_on`.  
**Estymacja:** 30 min  

### C-P1-15 — Brak wolumenu persystencji dla PostgreSQL

**Problem:** Dane bazy giną po `docker compose down`.  
**Rozwiązanie:** Dodaj `volumes:` dla service `db` z named volume.  
**Estymacja:** 10 min  

### C-P1-16 — Dockerfile nie kopiuje `data/` do obrazu

**Problem:** API w Dockerze nie ma dostępu do JSON bundli (ścieżka `../../data` nie istnieje w kontenerze).  
**Ryzyko:** Wszystkie content endpoints zwracają null lub błąd w Dockerze.  
**Rozwiązanie:** Zaktualizuj Dockerfile aby kopiował `data/` do `/app/data`, zaktualizuj `ContentBundles:DataRoot`.  
**Estymacja:** 1h  

### C-P1-17 — Brak Caddyfile reverse proxy

**Problem:** Brak routingu web → api → admin przez jeden port.  
**Rozwiązanie:** Opcjonalne — dodaj Caddy serwis do compose z prostym Caddyfile.  
**Estymacja:** 1h  

### C-P2-18 — Brak `find-free-port.ps1/.sh`

**Rozwiązanie:** Prosty skrypt sprawdzający dostępność portów z `.env`.  
**Estymacja:** 30 min  

---

## BLOK D — PANEL ADMINA

### D-P1-19 — AdminWeb to pusty scaffold

**Problem:** `Program.cs` ma tylko `/health` i placeholder HTML.  
**Ryzyko:** Brak narzędzi admina.  
**Rozwiązanie:** Dodaj podstawowe strony: dashboard (link do `/api/sessions`, `/api/content/validate`), status serwera. Nie wymaga Blazor — prosty HTML wystarczy dla v26.  
**Estymacja:** 1-2h  

---

## BLOK E — CONTENT I WALIDACJA

### E-P1-20 — ContentValidationService sprawdza tylko 4 pliki

**Problem:** Setki plików JSON w `data/` nie są walidowane.  
**Rozwiązanie:** Rozszerzyć walidację o kluczowe pliki content bundli v26.  
**Estymacja:** 1h  

---

## BLOK F — DOKUMENTACJA

### F-P1-21 — Brak `/docs/BUILD.md`

### F-P1-22 — Brak `/docs/RUN_LOCAL.md`

### F-P1-23 — Brak `/docs/RUN_DOCKER.md`

### F-P1-24 — Brak `/docs/ARCHITECTURE.md`

### F-P1-25 — Brak `/docs/QA_GUIDE.md`

### F-P1-26 — Brak `/docs/DEMO_SCRIPT.md`

### F-P2-27 — Brak `/docs/DEPLOY.md`

### F-P2-28 — Brak `/docs/ADMIN_GUIDE.md`

### F-P2-29 — Brak `/docs/USER_GUIDE.md`

### F-P2-30 — Brak `/docs/CHANGELOG.md`

**Rozwiązanie dla wszystkich:** Wygenerować z aktualnej wiedzy o repo.  
**Estymacja łączna:** 2-3h  

---

## Macierz priorytetów

| ID | Priorytet | Kategoria | Tytuł | Estymacja | Ryzyko |
| ---- | --------- | --------- | ----- | --------- | ------ |
| A-P0-01 | P0 | Testy | Projekt testowy API | 2-3h | HIGH |
| A-P0-02 | P0 | Testy | verify.ps1 placeholder | 30min | HIGH |
| A-P0-03 | P0 | Content | ContentValidation ścieżki | 30min | MED |
| A-P0-04 | P0 | Testy | Smoke testy rozszerzone | 1h | HIGH |
| B-P0-06 | P0 | API | Brak CORS | 30min | HIGH |
| B-P0-07 | P0 | API | Brak error handler | 30min | HIGH |
| B-P0-08 | P0 | API | Health v25→v26 | 5min | LOW |
| C-P0-13 | P0 | Docker | docker-verify bez smoke | 30min | MED |
| A-P1-05 | P1 | Testy | E2E Playwright | 2h | HIGH |
| B-P1-09 | P1 | API | ApplyAction TODO | 2h | HIGH |
| B-P1-10 | P1 | Boty | BotDirector nie mutuje stanu | 1h | HIGH |
| B-P1-11 | P1 | API | Rate limiting | 30min | MED |
| B-P1-12 | P1 | API | Structured logging | 1h | MED |
| C-P1-14 | P1 | Docker | Healthchecks | 30min | MED |
| C-P1-15 | P1 | Docker | Volume DB | 10min | MED |
| C-P1-16 | P1 | Docker | Dockerfile data/ copy | 1h | HIGH |
| C-P1-17 | P1 | Docker | Caddy reverse proxy | 1h | LOW |
| D-P1-19 | P1 | Admin | AdminWeb real content | 1-2h | MED |
| E-P1-20 | P1 | Content | ContentValidation rozszerzenie | 1h | LOW |
| F-P1-21..26 | P1 | Docs | Dokumentacja BUILD/RUN/ARCH/QA | 2-3h | LOW |
| C-P2-18 | P2 | Docker | find-free-port | 30min | LOW |
| F-P2-27..30 | P2 | Docs | Docs DEPLOY/ADMIN/USER/CHANGELOG | 2h | LOW |

---

## Definition of Done (DoD) — v26 Closure

- [x] `dotnet build` — 0 errors, 0 warnings
- [x] `dotnet test` — all tests pass
- [x] `smoke-v26.ps1` — PASS
- [x] `content-verify.ps1` — PASS
- [x] `verify.ps1` — real build+test, exit 0
- [x] `docker-verify.ps1` — up + health smoke + down
- [x] API: CORS, error handler, rate limiting aktywne
- [x] API: `/health` zwraca v26
- [x] API integration tests: co najmniej `/health`, `/api/sessions/demo`, `/api/reference-data`, `/api/content/validate`
- [x] E2E Playwright: co najmniej health + swagger + 3 key endpoints
- [x] Dockerfile: data/ kopiowane do obrazu
- [x] docker-compose: healthchecks + volumes
- [x] Docs: BUILD.md, RUN_LOCAL.md, RUN_DOCKER.md, ARCHITECTURE.md, QA_GUIDE.md, DEMO_SCRIPT.md
- [x] DEMO_REPORT.md — komplet
