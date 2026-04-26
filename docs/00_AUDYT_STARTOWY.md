# 00 — Audyt Startowy: 112 Centrum Alarmowe

> Stan po aktualnym przeglądzie repo i hardeningu operacyjnym v26.

---

## 1. Krótkie podsumowanie stanu projektu

### Co działa
- `Alarm112.sln` buduje się lokalnie.
- Testy .NET w `tests/Alarm112.Api.Tests` przechodzą.
- Istnieje workflow CI (`.github/workflows/ci.yml`) z buildem, testami, walidacją contentu i buildami obrazów Docker.
- API ma już:
  - JWT auth z `RequireAuth` sterowanym konfiguracją,
  - RBAC dla ról,
  - CORS,
  - rate limiting,
  - security headers,
  - correlation ID,
  - ProblemDetails,
  - SignalR hub,
  - Dockerfile i docker-compose.
- AdminWeb działa jako publiczny landing page, dashboard demo gracza oraz panel operacyjny z Basic Auth i serwerowym agregowaniem danych z API.
- Repo zawiera lokalne i dockerowe skrypty weryfikacyjne oraz testy Playwright dla API/AdminWeb.

### Co nie jest jeszcze w pełni produkcyjne
- Duża część gameplay/backendu nadal operuje na demo/showcase data zamiast pełnej logice produkcyjnej.
- Unity client zawiera szkielety i TODO dla części HUD-ów i sieci.
- Domyślny tryb lokalny nadal startuje bez wymuszonego auth (`Development`), co jest dobre dla dev, ale wymaga jawnego przełączenia na `Production`.
- Migracje SQL istnieją, ale nie są automatycznie wykonywane w compose ani przy starcie aplikacji.
- Webowy surface istnieje (`/`, `/app`, `/admin`), ale nadal nie jest pełnym produkcyjnym portalem klienta końcowego.

### Co wymaga pilnej poprawy
1. Utrzymanie spójnej dokumentacji operacyjnej z rzeczywistym kodem.
2. Twardsze readiness checks i jawna gotowość środowiskowa.
3. Dalsze przechodzenie z demo data na rzeczywiste scenariusze produkcyjne.
4. Automatyzacja migracji i pełniejszy deployment behind reverse proxy / HTTPS.

---

## 2. Najważniejsze ustalenia architektoniczne

| Obszar | Stan |
|---|---|
| Backend | Warstwowy układ `Api -> Application -> Domain`, DTO w `Contracts`, persistence w `Infrastructure` |
| API | Minimal API + SignalR, duży zestaw endpointów demo/showcase |
| Persistence | `InMemorySessionStore` dla local/dev, `PostgresSessionStore` aktywowany connection stringiem |
| Content pipeline | JSON bundles w `data/`, ładowane przez `IContentBundleLoader` |
| Admin | Minimal-hosted panel z Basic Auth i dashboardem operacyjnym |
| Infra | Docker Compose dla API, AdminWeb, PostgreSQL i Redis |
| Testy | xUnit + `WebApplicationFactory`, Playwright w `tests/e2e`, GitHub Actions CI |

---

## 3. Ryzyka

### Blocker
- Brak pełnej logiki produkcyjnej dla wielu przepływów sesji; duża część to nadal vertical-slice/demo pack.
- Brak automatycznego procesu migracji DB w runtime/deploy.

### High
- Dokumentacja repo była częściowo nieaktualna względem kodu.
- Health checks były zbyt płytkie; po poprawce istnieją już `/health/live` i `/health/ready`, ale trzeba ich konsekwentnie używać w runbookach i wdrożeniu.
- Content validation wcześniej obejmował tylko kilka plików; po poprawce waliduje krytyczne katalogi i bundli, ale nadal nie sprawdza semantyki domenowej każdego JSON.

### Medium
- Compose nie zawiera reverse proxy ani TLS termination.
- AdminWeb jest funkcjonalny operacyjnie, ale nie jest pełnym systemem administracyjnym CRUD/liveops.
- Testy jakościowe są mocne dla backendu, ale nie pokrywają całości klienta Unity.

### Low
- Część starszych dokumentów w repo nadal opisuje historyczne braki.

---

## 4. Priorytety

| Priorytet | Zakres |
|---|---|
| **Blocker** | produkcyjna logika sesji, migracje i bootstrap środowiska |
| **High** | deploy readiness, spójna dokumentacja, security-by-default dla stage/prod |
| **Medium** | rozszerzenie panelu admina, bogatsze smoke/e2e, pełniejsze demo operacyjne |
| **Low** | porządki w starszych dokumentach i backlogach historycznych |
