# RUN_LOCAL.md — Uruchomienie lokalne

## Wymagania

- .NET 10 Preview SDK (zob. [BUILD.md](BUILD.md))
- Opcjonalnie: Docker (dla PostgreSQL i Redis)

---

## Szybki start — tylko API (in-memory)

API działa bez bazy danych dzięki `InMemorySessionStore`. To wystarczy do developmentu i testów.

```powershell
cd c:\projekty\centrumalarmowe
dotnet run --project src/Alarm112.Api
```

API startuje na `http://localhost:5080`.

Swagger UI: **http://localhost:5080/swagger**

Health check: **http://localhost:5080/health**

---

## Uruchomienie z infrastrukturą (PostgreSQL + Redis)

Wymaga Docker Desktop.

```powershell
# Uruchom PostgreSQL i Redis w tle
docker compose -f infra/docker-compose.yml up db redis -d

# Uruchom API z connection stringami
$env:ConnectionStrings__Main = "Host=localhost;Port=5432;Database=alarm112;Username=postgres;Password=postgres"
$env:Redis__Connection = "localhost:6379"
dotnet run --project src/Alarm112.Api
```

---

## Panel admina

```powershell
dotnet run --project src/Alarm112.AdminWeb
```

Panel startuje na `http://localhost:5081`. Domyślnie łączy się z API na `http://localhost:5080`.

Aby zmienić adres API:
```powershell
$env:ApiBaseUrl = "http://localhost:5080"
dotnet run --project src/Alarm112.AdminWeb
```

---

## Zmienne środowiskowe

| Zmienna | Domyślna | Opis |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Development` | Środowisko ASP.NET Core |
| `ASPNETCORE_URLS` | `http://localhost:5080` | Adresy nasłuchu API |
| `ContentBundles:DataRoot` | `../../data` (rel. do projektu) | Ścieżka do bundli JSON |
| `ConnectionStrings:Main` | (brak) | PostgreSQL — opcjonalne |
| `Redis:Connection` | (brak) | Redis — opcjonalne |
| `Cors:AllowedOrigins` | `http://localhost:3000,...` | CORS origins |
| `ApiBaseUrl` | `http://localhost:5080` | Używane przez AdminWeb |

---

## Weryfikacja uruchomionego API

```powershell
# Health check
Invoke-RestMethod http://localhost:5080/health

# Utwórz sesję demo
Invoke-RestMethod -Method POST http://localhost:5080/sessions/demo

# Pobierz stan sesji (zastąp SESSION_ID wartością z poprzedniego wywołania)
Invoke-RestMethod http://localhost:5080/sessions/SESSION_ID/snapshot
```

---

## Porty domyślne

| Usługa | Port |
|---|---|
| API | 5080 |
| AdminWeb | 5081 |
| PostgreSQL | 5432 |
| Redis | 6379 |

---

## Sprawdzenie zajętości portów

```powershell
.\tools\find-free-port.ps1
```

---

## Logi

Logi w trybie Development wychodzą na konsolę z poziomem `Debug`. W razie problemów z ładowaniem bundli szukaj linii zawierających `ContentBundleLoader`.

---

## SignalR — połączenie testowe

Hub jest dostępny pod `/hubs/session`. Przykład połączenia przez wscat:

```bash
npm install -g wscat
wscat -c "ws://localhost:5080/hubs/session"
```

Po połączeniu wyślij:
```json
{"protocol":"json","version":1}
```
