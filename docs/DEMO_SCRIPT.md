# DEMO_SCRIPT.md — Scenariusz prezentacji 112: Centrum Alarmowe

> Wersja: v26  
> Czas trwania demo: ~10-15 minut  
> Cel: zademonstrowanie pełnego przepływu od uruchomienia API do działającej sesji gry

---

## Wymagania przed demo

- .NET 10 SDK zainstalowany
- Node.js 20+ (opcjonalnie — dla E2E)
- Docker Desktop (opcjonalnie — dla pełnego stack demo)
- Przeglądarka internetowa

---

## CZĘŚĆ 1 — Uruchomienie lokalne (5 min)

### Krok 1: Zbuduj i uruchom API

```powershell
cd c:\projekty\centrumalarmowe

# Weryfikacja budowania
dotnet build Alarm112.sln --nologo

# Uruchom API
dotnet run --project src/Alarm112.Api
```

**Oczekiwany wynik:** API startuje na `http://localhost:5080`

### Krok 2: Sprawdź health

Otwórz przeglądarkę lub uruchom:

```powershell
Invoke-RestMethod http://localhost:5080/health
```

**Oczekiwany wynik:**
```json
{ "ok": true, "service": "Alarm112.Api", "version": "v26" }
```

### Krok 3: Otwórz Swagger UI

Otwórz w przeglądarce: **http://localhost:5080/swagger**

Pokaż dostępne endpointy (40+): sessions, reference-data, home-hub, city-map, mission-briefing, booty, itp.

---

## CZĘŚĆ 2 — Przepływ gry (5 min)

### Krok 4: Utwórz sesję demo

W Swagger lub terminal:

```powershell
$session = Invoke-RestMethod -Method POST http://localhost:5080/api/sessions/demo
$sessionId = $session.sessionId
Write-Host "Session ID: $sessionId"
```

**Oczekiwany wynik:** Obiekt sesji z `sessionId`, listą incydentów i jednostek.

### Krok 5: Odczytaj stan sesji

```powershell
$snap = Invoke-RestMethod "http://localhost:5080/api/sessions/$sessionId"
$snap.incidents | Format-Table -Property incidentId, category, severity, status
$snap.units | Format-Table -Property unitId, role, status
```

**Pokaż:** 4 incydenty, jednostki (straż, pogotowie, policja) w różnych statusach.

### Krok 6: Wyślij komendę dyspozycji

```powershell
$action = @{
    sessionId = $sessionId
    actorId = "player-1"
    role = "Dispatcher"
    actionType = "dispatch"
    payloadJson = '{"incidentId":"inc-demo-1","unitId":"unit-1"}'
    correlationId = [Guid]::NewGuid().ToString("N")
} | ConvertTo-Json

Invoke-RestMethod -Method POST "http://localhost:5080/api/sessions/$sessionId/actions" -Body $action -ContentType "application/json"
```

**Oczekiwany wynik:** `{ "success": true, ... }`

### Krok 7: Mapa miasta

```powershell
Invoke-RestMethod http://localhost:5080/api/city-map | ConvertTo-Json -Depth 3
```

Pokaż węzły mapy, połączenia, incydenty na mapie.

### Krok 8: Briefing misji

```powershell
Invoke-RestMethod http://localhost:5080/api/mission-briefing/demo | ConvertTo-Json -Depth 3
```

Pokaż strukturę misji: cele, scenariusz, role.

---

## CZĘŚĆ 3 — Panel Admina (2 min)

### Krok 9: Uruchom AdminWeb

Otwórz nowe okno terminala:

```powershell
dotnet run --project src/Alarm112.AdminWeb
```

Otwórz w przeglądarce: **http://localhost:5081**

**Pokaż karty dashboardu:**
- Status API → GET /health
- Walidacja contentu → GET /api/content/validate
- Sesje demo
- Reference data
- Home Hub
- City Map
- Mission Briefing

### Krok 10: Content Validation

W przeglądarce kliknij kartę "Walidacja contentu" lub:

```powershell
Invoke-RestMethod http://localhost:5080/api/content/validate
```

**Oczekiwany wynik:** `{ "isValid": true, "checkedFiles": [...] }`

---

## CZĘŚĆ 4 — Testy (2 min)

### Krok 11: Uruchom testy integracyjne

```powershell
dotnet test tests/Alarm112.Api.Tests --logger "console;verbosity=quiet" --nologo
```

**Oczekiwany wynik:** 24/24 passed, 0 failed.

### Krok 12: Uruchom E2E Playwright (opcjonalnie, wymaga działającego API)

```powershell
cd tests/e2e
npm install
npx playwright install chromium
npm test
```

**Oczekiwany wynik:** 50+ testów green.

---

## CZĘŚĆ 5 — Docker Stack (opcjonalnie, 2 min)

### Krok 13: Uruchom pełny stack

```powershell
cd c:\projekty\centrumalarmowe
docker compose -f infra/docker-compose.yml up --build -d
```

Sprawdź:
```powershell
docker compose -f infra/docker-compose.yml ps
Invoke-RestMethod http://localhost:5080/health
```

Zatrzymaj:
```powershell
docker compose -f infra/docker-compose.yml down
```

---

## Kluczowe punkty do omówienia podczas demo

1. **Architektura DDD**: Domain → Application → API z unidirektionalnymi zależnościami
2. **Content-driven**: dane ładowane z JSON bundli, nie hardkodowane
3. **AI Bot fallback**: BotTickHostedService automatycznie obsługuje nieobsadzone role
4. **SignalR real-time**: eventy sesji wysyłane do klientów Unity przez WebSocket
5. **In-Memory store**: na v26 sesje przechowywane w pamięci; PostgreSQL docelowo
6. **Rate limiting + CORS**: zabezpieczenia produkcyjne włączone
7. **Structured error handling**: ProblemDetails dla wszystkich błędów

---

## Dane demo / seed

Sesja demo (`/api/sessions/demo`) tworzy:
- 4 incydenty (Fire, Medical, Police, Traffic) w różnych statusach
- 6 jednostek (PSP, PRM, Policja, MPD) 
- 4 role (CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer)
- Bot profiles ładowane z `data/config/bot_profiles.json`

Mapa demo: `data/content/city-map.v1.json` — Centrum, Śródmieście, Praga, Mokotów, Żoliborz
