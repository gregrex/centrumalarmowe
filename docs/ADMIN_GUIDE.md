# ADMIN_GUIDE.md — Przewodnik administratora

> Wersja: v26

---

## Panel Admina — dostęp

Panel Admina jest dostępny pod adresem:
- **Lokalnie:** http://localhost:5081
- **Docker:** http://localhost:5081 (zob. `.env` → `ADMIN_PORT`)

Panel nie wymaga logowania w v26. **Plan:** autentykacja w v27.

---

## Funkcje panelu

### Dashboard

Strona główna wyświetla karty z linkami do wszystkich kluczowych endpointów API:

| Karta | Endpoint | Opis |
|---|---|---|
| Status API | GET /health | Zdrowie serwera API |
| Walidacja contentu | GET /api/content/validate | Sprawdź poprawność JSON bundli |
| Sesje demo | POST /api/sessions/demo | Utwórz sesję testową |
| Reference data | GET /api/reference-data | Pełny bundle referencyjny v26 |
| Home Hub | GET /api/home-hub | Dane ekranu głównego gracza |
| City Map | GET /api/city-map | Mapa miasta |
| Mission Briefing | GET /api/mission-briefing/demo | Briefing misji demo |
| Quick Play | GET /api/quickplay/bootstrap | Bootstrap trybu quick play |
| Theme Pack | GET /api/theme-pack | Motyw graficzny |
| Settings | GET /api/settings-bundle | Ustawienia gry |
| Operator/Dispatcher | GET /api/operator-dispatcher-showcase | Showcase ról |
| Android Build | GET /api/real-android-build | Status build Android |

---

## Zarządzanie contentem

### Walidacja JSON bundli

```
GET /api/content/validate
```

Zwraca wynik walidacji wszystkich kluczowych plików JSON w `data/`.

Przykładowa odpowiedź:
```json
{
  "isValid": true,
  "checkedFiles": ["data/reference/reference-data.v26.json", ...],
  "issues": []
}
```

### Lokalny skrypt walidacji

```powershell
.\tools\content-verify.ps1
```

---

## Zarządzanie sesjami

### Tworzenie sesji demo

```
POST /api/sessions/demo
```

Odpowiedź: `SessionSnapshotDto` z `sessionId`, incydentami i jednostkami.

### Podgląd sesji

```
GET /api/sessions/{sessionId}
```

### Wysyłanie akcji do sesji

```
POST /api/sessions/{sessionId}/actions
Content-Type: application/json

{
  "sessionId": "...",
  "actorId": "admin-1",
  "role": "Dispatcher",
  "actionType": "dispatch",
  "payloadJson": "{\"incidentId\":\"inc-1\",\"unitId\":\"unit-1\"}",
  "correlationId": "..."
}
```

---

## Role gracza

| Rola | ID | Opis |
|------|----|----|
| Operator Centrum | `CallOperator` | Odbiera zgłoszenia, kategoryzuje zdarzenia |
| Dyspozytor | `Dispatcher` | Dysponuje jednostki do zdarzeń |
| Koordynator Operacji | `OperationsCoordinator` | Zarządza zasobami i eskalacją |
| Oficer Kryzysowy | `CrisisOfficer` | Decyzje strategiczne, relacje z mediami |

---

## AI Boty

Boty automatycznie zastępują brakujących graczy.

- Konfiguracja botów: `data/config/bot_profiles.json`
- Konfiguracja ról: `data/config/roles.json`
- `BotTickHostedService` — tyknie co 5 sekund

---

## Zmiane konfiguracji

### Zmiana portów

Edytuj `infra/.env`:
```dotenv
API_PORT=5080
ADMIN_PORT=5081
```

Następnie zrestart:
```powershell
docker compose -f infra/docker-compose.yml restart
```

### Zmiana CORS origins

W `appsettings.json` lub zmiennej środowiskowej:
```json
{
  "Cors": {
    "AllowedOrigins": "http://localhost:3000,http://localhost:5081,https://twoja-domena.pl"
  }
}
```

### Zmiana ścieżki do data/

```
ContentBundles:DataRoot=/ścieżka/do/data
```

---

## Swagger UI

Pełna dokumentacja API dostępna pod:
- http://localhost:5080/swagger

Pozwala na testowanie wszystkich 40+ endpointów bezpośrednio z przeglądarki.

---

## Logi systemu

### Lokalne

```powershell
dotnet run --project src/Alarm112.Api 2>&1 | Tee-Object -FilePath logs/api.log
```

### Docker

```bash
docker compose -f infra/docker-compose.yml logs -f api
docker compose -f infra/docker-compose.yml logs -f admin
```

---

## Diagnostyka

| Problem | Diagnoza | Rozwiązanie |
|---|---|---|
| API nie odpowiada | Port zajęty | `netstat -an | findstr 5080` |
| Content validate fail | Plik JSON brakujący | Sprawdź `data/content/` i `data/reference/` |
| Bot nie działa | BotTickHostedService | Sprawdź logi: `Started BotTickHostedService` |
| SignalR nie działa | CORS | Dodaj origin Unity client do `Cors:AllowedOrigins` |
