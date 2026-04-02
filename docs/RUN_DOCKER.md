# RUN_DOCKER.md — Uruchomienie w Dockerze

## Wymagania

- Docker Desktop 4.x+
- Plik `.env` (opcjonalny — są wartości domyślne)

---

## Szybki start

```powershell
cd c:\projekty\centrumalarmowe
docker compose -f infra/docker-compose.yml up --build
```

Po uruchomieniu:
- API: **http://localhost:5080/swagger**
- AdminWeb: **http://localhost:5081**
- PostgreSQL: `localhost:5432`
- Redis: `localhost:6379`

---

## Plik `.env` (opcjonalny)

Skopiuj przykład i dostosuj porty jeśli są zajęte:

```powershell
Copy-Item infra/.env.example infra/.env
```

Dostępne zmienne:

```dotenv
API_PORT=5080
ADMIN_PORT=5081
DB_PORT=5432
REDIS_PORT=6379
GATEWAY_PORT=5090
```

---

## Uruchomienie w tle (detached)

```powershell
docker compose -f infra/docker-compose.yml up -d --build
```

Sprawdzenie stanu kontenerów:

```powershell
docker compose -f infra/docker-compose.yml ps
```

---

## Healthchecks

Wszystkie serwisy mają skonfigurowane healthchecki. API i AdminWeb startują dopiero gdy baza danych i Redis są `healthy`.

| Serwis | Healthcheck |
|---|---|
| `api` | `wget -qO- http://localhost:8080/health` |
| `admin` | `wget -qO- http://localhost:8080/health` |
| `db` | `pg_isready -U postgres -d alarm112` |
| `redis` | `redis-cli ping` |

---

## Weryfikacja po uruchomieniu

```powershell
.\tools\docker-verify.ps1
```

Skrypt wykonuje:
1. Załadowanie `.env`
2. `docker compose build`
3. `docker compose up -d`
4. Polling healthchecków (timeout 30s)
5. Smoke check `/health` na API i Admin
6. Wyświetla `docker compose ps`
7. `docker compose down -v`

---

## Zatrzymanie i sprzątanie

```powershell
# Zatrzymaj kontenery (zachowaj wolumeny)
docker compose -f infra/docker-compose.yml down

# Zatrzymaj i usuń wolumeny (czysta baza)
docker compose -f infra/docker-compose.yml down -v
```

---

## Wolumeny

| Wolumen | Zawartość |
|---|---|
| `db-data` | Dane PostgreSQL (persystentne między restartami) |
| `redis-data` | Dane Redis |

---

## Wysyłanie requestów do skonteneryzowanego API

```powershell
# Health
Invoke-RestMethod http://localhost:5080/health

# Nowa sesja demo
Invoke-RestMethod -Method POST http://localhost:5080/sessions/demo
```

---

## Logi kontenerów

```powershell
# Logi API (na żywo)
docker compose -f infra/docker-compose.yml logs -f api

# Logi wszystkich serwisów
docker compose -f infra/docker-compose.yml logs -f
```

---

## Rebuild po zmianie kodu

```powershell
docker compose -f infra/docker-compose.yml build api
docker compose -f infra/docker-compose.yml up -d api
```

---

## Znane problemy

- **Port zajęty**: uruchom `.\tools\find-free-port.ps1` i dostosuj `.env`
- **Build context**: Dockerfile wymaga katalogu głównego repo jako build context — jest to skonfigurowane prawidłowo (`context: ..` w docker-compose.yml)
- **Brak obrazu preview**: Jeśli `mcr.microsoft.com/dotnet/aspnet:10.0-preview` niedostępny, zaktualizuj tagi w Dockerfile
