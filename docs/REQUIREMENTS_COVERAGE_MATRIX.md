# Requirements Coverage Matrix — 112: Centrum Alarmowe

> Generowano: 2026-03-29  
> Wersja repo: v26 (Real Android Build + Bugfix Freeze)

## Legenda statusów
| Status | Opis |
|--------|------|
| ✅ DONE | Zaimplementowane i zweryfikowane |
| 🟡 PARTIAL | Zaimplementowane częściowo — szkielet / stub |
| ❌ MISSING | Brak implementacji |
| 🔲 OUT-OF-SCOPE | Celowo wykluczone z aktualnego zakresu (uzasadnienie w kolumnie Uwagi) |

---

## 1. Backend API (`src/Alarm112.Api`)

| Wymaganie | Backend | Status | Pliki / kod | Test | Uwagi |
|-----------|---------|--------|-------------|------|-------|
| Health endpoint | API | ✅ DONE | `Program.cs:/health` | smoke-v26 | wersja pokazuje v25 (powinno v26) |
| Swagger UI | API | ✅ DONE | `Program.cs` | brak | ok dla dev |
| Sesja demo (POST /api/sessions/demo) | API | ✅ DONE | `SessionService.cs` | brak | in-memory |
| Pobranie sesji (GET /api/sessions/{id}) | API | ✅ DONE | `SessionService.cs` | brak | in-memory |
| Akcja na sesji (POST /api/sessions/{id}/actions) | API | 🟡 PARTIAL | `SessionService.cs:ApplyActionAsync` | brak | TODO: real action dispatch |
| SignalR: JoinSession | API | ✅ DONE | `SessionHub.cs` | brak | |
| SignalR: session.envelope push | API | ✅ DONE | `Program.cs` | brak | |
| Reference data | API | ✅ DONE | `ReferenceDataService.cs` | brak | |
| Theme pack / menu flow | API | ✅ DONE | `ThemePackService.cs` | brak | |
| Home hub / campaign overview | API | ✅ DONE | `HomeFlowService.cs` | brak | |
| Campaign chapters | API | ✅ DONE | `CampaignEntryService.cs` | brak | |
| Mission briefing | API | ✅ DONE | `MissionFlowService.cs` | brak | |
| Mission runtime | API | ✅ DONE | `MissionRuntimeService.cs` | brak | |
| Playable runtime map | API | ✅ DONE | `PlayableRuntimeService.cs` | brak | |
| Dispatcher loop | API | ✅ DONE | `PlayableRuntimeService.cs` | brak | |
| Recovery decision cards | API | ✅ DONE | `QuasiProductionDemoService.cs` | brak | |
| Post-round report | API | ✅ DONE | `MissionFlowService.cs` | brak | |
| City map | API | ✅ DONE | `CityMapService.cs` | brak | |
| Dispatch command | API | ✅ DONE | `CityMapService.cs` | brak | |
| Lobby create/get | API | ✅ DONE | `LobbyService.cs` | brak | |
| Quick play bootstrap/start | API | ✅ DONE | `QuickPlayService.cs` | brak | |
| Content validation | API | 🟡 PARTIAL | `ContentValidationService.cs` | brak | dwie referencje do nieistniejących plików |
| Real Android build endpoints | API | ✅ DONE | `RealAndroidBuildService.cs` | brak | |
| Near-final slice flow | API | ✅ DONE | `NearFinalSliceService.cs` | brak | |
| Showcase demo flow | API | ✅ DONE | `ShowcaseDemoService.cs` | brak | |
| Internal test pack | API | ✅ DONE | `InternalTestService.cs` | brak | |
| Final handoff | API | ✅ DONE | `FinalHandoffService.cs` | brak | |
| **CORS** | API | ❌ MISSING | — | — | brak AddCors/UseCors |
| **Error handling middleware** | API | ❌ MISSING | — | — | brak obsługi wyjątków |
| **Rate limiting** | API | ❌ MISSING | — | — | |
| **Structured logging / correlation ID** | API | ❌ MISSING | — | — | tylko default logging |
| **Versioning API** | API | 🟡 PARTIAL | — | — | /api/ bez numerów wersji |
| **Input validation** | API | 🟡 PARTIAL | — | — | brak FluentValidation / DataAnnotations |

---

## 2. AI Boty (`IBotDirector` / `BotTickHostedService`)

| Wymaganie | Komponent | Status | Pliki / kod | Test | Uwagi |
|-----------|-----------|--------|-------------|------|-------|
| Bot fallback dla każdej roli | Application | 🟡 PARTIAL | `BotDirector.cs` | brak | symuluje bez realnej mutacji stanu |
| BotTick hosted service | API | ✅ DONE | `BotTickHostedService.cs` | brak | cykl 5s |
| Profil bota z JSON | Application | ✅ DONE | `BotDirector.cs` | brak | ładuje bot_profiles.json |
| AI takeover po disconneccie gracza | Application | ❌ MISSING | — | — | brak obsługi reconnect/takeover |
| Bot wykonuje realne akcje na sesji | Application | ❌ MISSING | `BotDirector.cs:ExecuteBotTickAsync` | brak | TODO w kodzie |

---

## 3. Session State Machine

| Wymaganie | Komponent | Status | Pliki / kod | Test | Uwagi |
|-----------|-----------|--------|-------------|------|-------|
| Stany: Draft→Lobby→Countdown→Active→Recovery→Summary→Archived | Domain | 🟡 PARTIAL | `SessionState.cs` | brak | enumy istnieją, brak state machine |
| Idempotentne akcje (ActionId) | API | 🟡 PARTIAL | `SessionActionDto` | brak | pole istnieje, brak deduplication |
| RealtimeEnvelopeDto push po akcji | API | ✅ DONE | `Program.cs` | brak | |
| Heartbeat SignalR | API | ✅ DONE | `SessionHub.cs` | brak | |
| Reconnect / takeover | API | ❌ MISSING | — | — | poza zakresem v26 |

---

## 4. Panel Admina (`src/Alarm112.AdminWeb`)

| Wymaganie | Komponent | Status | Pliki / kod | Test | Uwagi |
|-----------|-----------|--------|-------------|------|-------|
| Health endpoint | AdminWeb | ✅ DONE | `Program.cs` | brak | |
| Dashboard | AdminWeb | ❌ MISSING | — | — | tylko scaffold HTML |
| Zarządzanie scenariuszami | AdminWeb | ❌ MISSING | — | — | |
| Event catalog | AdminWeb | ❌ MISSING | — | — | |
| Zarządzanie rolami/botami | AdminWeb | ❌ MISSING | — | — | |
| Telemetry | AdminWeb | ❌ MISSING | — | — | |
| Zarządzanie sesjami | AdminWeb | ❌ MISSING | — | — | |
| Liveops | AdminWeb | ❌ MISSING | — | — | |
| Autentykacja admina | AdminWeb | ❌ MISSING | — | — | brak auth całkowicie |

---

## 5. Klient Unity

| Wymaganie | Komponent | Status | Pliki / kod | Test | Uwagi |
|-----------|-----------|--------|-------------|------|-------|
| Splash screen | Unity/Menu | 🟡 PARTIAL | `Menu/` (29 plików) | brak | szkielety |
| Main menu / home hub | Unity/Menu | 🟡 PARTIAL | `Menu/` | brak | |
| Lobby / wybór trybu | Unity/Lobby | 🟡 PARTIAL | `Lobby/` (3 pliki) | brak | |
| Wybór roli | Unity/Session | 🟡 PARTIAL | `Session/` (24 pliki) | brak | |
| Briefing scenariusza | Unity/Session | 🟡 PARTIAL | `Session/` | brak | |
| Ekran główny miasta (mapa) | Unity/Map | 🟡 PARTIAL | `Map/` (13 pliki) | brak | |
| Panel zgłoszeń (operator HUD) | Unity/UI/Huds | 🟡 PARTIAL | `UI/Huds/` (4 pliki) | brak | |
| Panel dyspozycji (dispatcher HUD) | Unity/UI/Huds | 🟡 PARTIAL | `UI/Huds/` | brak | |
| Panel aktywnych zdarzeń | Unity/UI | 🟡 PARTIAL | `UI/` | brak | |
| Raport końca zmiany | Unity/UI/Reports | 🟡 PARTIAL | `UI/Reports/` (7 pliki) | brak | |
| AI boty w Unity | Unity/Bots | 🟡 PARTIAL | `Bots/` (4 pliki) | brak | |
| Networking / SignalR | Unity/Networking | 🟡 PARTIAL | `Networking/` (2 pliki) | brak | |
| Coop | Unity/Coop | 🟡 PARTIAL | `Coop/` (1 plik) | brak | minimalny |
| Quick play | Unity/QuickPlay | 🟡 PARTIAL | `QuickPlay/` (2 pliki) | brak | |

---

## 6. Infrastruktura

| Wymaganie | Komponent | Status | Pliki / kod | Test | Uwagi |
|-----------|-----------|--------|-------------|------|-------|
| Docker Compose (api, admin, db, redis) | infra | ✅ DONE | `docker-compose.yml` | brak | |
| Porty z .env | infra | ✅ DONE | `.env.example`, `docker-compose.yml` | brak | |
| Dockerfile API | API | ✅ DONE | `src/Alarm112.Api/Dockerfile` | brak | |
| Dockerfile AdminWeb | AdminWeb | ✅ DONE | `src/Alarm112.AdminWeb/Dockerfile` | brak | |
| Healthchecks w docker-compose | infra | ❌ MISSING | — | — | brak healthcheck sekcji |
| Wolumeny dla bazy danych | infra | ❌ MISSING | — | — | brak persist volume |
| DB migracje | db | 🟡 PARTIAL | `db/schema/001_init.sql...021` | brak | szkielety, nieaplikowane |
| Redis jako store | infra | ❌ MISSING | — | — | tylko w compose, brak implementacji |
| Caddy reverse proxy | infra | ❌ MISSING | — | — | brak Caddyfile |

---

## 7. Content Pipeline

| Wymaganie | Komponent | Status | Pliki | Test | Uwagi |
|-----------|-----------|--------|-------|------|-------|
| JSON bundles content-driven | data | ✅ DONE | `data/content/*.json` | content-verify.ps1 | |
| Reference data bundles | data | ✅ DONE | `data/reference/*.json` | smoke | |
| Content validation endpoint | API | 🟡 PARTIAL | `ContentValidationService.cs` | brak | sprawdza tylko 4 pliki (2 błędne ścieżki) |
| Content verify script | tools | ✅ DONE | `tools/content-verify.ps1` | — | |

---

## 8. Testy

| Wymaganie | Komponent | Status | Pliki | Wynik | Uwagi |
|-----------|-----------|--------|-------|-------|-------|
| Smoke testy per wersja | tools | ✅ DONE | `tools/smoke-v*.ps1`, `.sh` | PASS | tylko sprawdzają obecność plików |
| API integration tests | tests | ❌ MISSING | — | — | brak projektu testowego |
| E2E Playwright (web) | tests | ❌ MISSING | — | — | brak |
| E2E Appium (mobile) | tests | ❌ MISSING | — | — | brak; OUT-OF-SCOPE v26 |
| Unit tests | tests | ❌ MISSING | — | — | brak |
| verify.ps1 (real build+test) | tools | ❌ MISSING | `tools/verify.ps1` | PLACEHOLDER | tylko printuje stubs |
| docker-verify.ps1 | tools | 🟡 PARTIAL | `tools/docker-verify.ps1` | działa | brak smoke po up |

---

## 9. Role i uprawnienia

| Wymaganie | Komponent | Status | Pliki | Uwagi |
|-----------|-----------|--------|-------|-------|
| 4 role (CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer) | Domain | ✅ DONE | `RoleType.cs` | |
| Role-based HUD | Unity | 🟡 PARTIAL | `UI/Huds/` | |
| Autentykacja API | API | ❌ MISSING | — | brak; uwaga: OUT-OF-SCOPE v26 per docs |

---

## 10. Bezpieczeństwo

| Wymaganie | Status | Uwagi |
|-----------|--------|-------|
| CORS | ❌ MISSING | API bezpośrednio brak konfiguracji |
| Rate limiting | ❌ MISSING | |
| Input sanitization | 🟡 PARTIAL | brak walidacji wejść |
| Sekrety w user-secrets | 🟡 PARTIAL | .env.example istnieje, bez secrets w repo |
| HTTPS | 🟡 PARTIAL | brak konfiguracji, docker EXPOSE 8080 |

---

## Podsumowanie statystyk

| Kategoria | DONE | PARTIAL | MISSING |
|-----------|------|---------|---------|
| Backend API (endpoints) | 26 | 5 | 3 |
| AI Boty | 2 | 1 | 2 |
| Session State | 2 | 2 | 1 |
| Admin Web | 1 | 0 | 8 |
| Unity Client | 0 | 14 | 0 |
| Infrastruktura | 4 | 1 | 5 |
| Content Pipeline | 3 | 1 | 0 |
| Testy | 1 | 1 | 5 |
| Role/Uprawnienia | 2 | 1 | 1 |
| Bezpieczeństwo | 0 | 2 | 3 |
| **RAZEM** | **41** | **28** | **28** |
