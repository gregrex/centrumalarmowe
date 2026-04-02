# DEPLOY.md — Wdrożenie produkcyjne

> Wersja: v26  
> Dotyczy: wdrożenie na serwer Linux z Docker Compose

---

## Wymagania środowiska produkcyjnego

| Składnik | Wersja | Uwagi |
| --- | --- | --- |
| Docker Engine | 24.x+ | |
| Docker Compose | 2.x | plugin, nie standalone |
| Serwer | Ubuntu 22.04 / Debian 12 | 2 CPU / 4GB RAM minimum |
| Domena | opcjonalna | wymagana do TLS |

---

## Przygotowanie serwera

```bash
# Zainstaluj Docker
curl -fsSL https://get.docker.com | sh
sudo usermod -aG docker $USER

# Klonuj repo
git clone <repo-url> /opt/alarm112
cd /opt/alarm112
```

---

## Konfiguracja środowiska

Skopiuj i dostosuj zmienne:

```bash
cp infra/.env.example infra/.env
nano infra/.env
```

Obowiązkowe do zmiany:

```dotenv
# Porty (zmień jeśli są zajęte)
API_PORT=5080
ADMIN_PORT=5081
DB_PORT=5432
REDIS_PORT=6379

# WAŻNE: zmień hasło bazy danych!
POSTGRES_PASSWORD=<silne-haslo>

# Security (zalecane dla produkcji)
SECURITY__REQUIREAUTH=true
SECURITY__ENABLEDEVTOKENENDPOINT=false
SECURITY__JWT__ISSUER=Alarm112.Api
SECURITY__JWT__AUDIENCE=Alarm112.Client
SECURITY__JWT__SIGNINGKEY=<min-32-znaki-losowy-sekret>
```

---

## Sekrety (produkcja)

Nie umieszczaj sekretów w repozytorium. Użyj:

- **Docker Secrets** (Swarm mode) dla haseł bazy danych
- **Zmienne środowiskowe** przez system init serwera
- Plik `.env` nigdy nie powinien być w git (jest w `.gitignore`)

```bash
# Skopiuj .env poza repo
cp infra/.env /etc/alarm112.env
chmod 600 /etc/alarm112.env
```

---

## Kontrola dostępu (RBAC) — v26

System obsługuje 4 role z JWT-based autoryzacją:

| Rola | Uprawnienia | Typ użytkownika |
| --- | --- | --- |
| **CallOperator** | Tworzenie sesji, zarządzanie lobby, dostęp do adminów | Operator alarmowy |
| **Dispatcher** | Wysyłanie jednostek (`POST /api/sessions/{id}/dispatch`) | Dyspozytornia |
| **OperationsCoordinator** | Dostęp do incydentów i podglądu tras | Koordynator |
| **CrisisOfficer** | Dostęp do incydentów i podglądu tras | Oficer kryzysu |

### Konfiguracja autoryzacji

**Włączenie (produkcja):**

```dotenv
SECURITY__REQUIREAUTH=true
SECURITY__ENABLEDEVTOKENENDPOINT=false
SECURITY__JWT__SIGNINGKEY=<min-32-znaki-losowy-sekret>
```

**Wyłączenie (dev/testing):**

```dotenv
SECURITY__REQUIREAUTH=false
SECURITY__ENABLEDEVTOKENENDPOINT=true
```

### Uzyskanie tokena dostępu (dev)

```bash
# Aktywuj dev token endpoint (Security:EnableDevTokenEndpoint=true)

curl -X POST http://localhost:5080/auth/dev-token \
  -H "Content-Type: application/json" \
  -d '{"subject":"alice","role":"Dispatcher"}'

# Odpowiedź:
# { "accessToken": "eyJ0eXAi...", "tokenType": "Bearer", "expiresIn": 3600, "role": "Dispatcher" }
```

Wklejaj `accessToken` w header: `Authorization: Bearer <accessToken>`

### Endpointy chronione (Security:RequireAuth=true)

| PUT/POST/GET | Endpoint | Rola wymagana | Opis |
| --- | --- | --- | --- |
| POST | /api/sessions/demo | CallOperator | Tworzenie sesji demo |
| POST | /api/lobbies/demo | CallOperator | Tworzenie lobby demo |
| POST | /api/sessions/{id}/dispatch | Dispatcher | Wysłanie jednostki |
| POST | /api/quickplay/start | any authenticated | Start quickplay |
| GET | /api/reference-data | any authenticated | Dane referencyjne gry |
| GET | /api/sessions/{id} | any authenticated | Stan sesji |
| POST | /api/sessions/{id}/actions | any authenticated | Akcje gracza |
| POST | /api/sessions/{id}/shared-actions | any authenticated | Wspólne akcje |

### Endpointy publiczne (zawsze dostępne)

- GET `/health` — healthcheck
- GET `/swagger/*` — dokumentacja API
- POST `/auth/dev-token` — generacja tokena deweloperskiego

---

## Uruchomienie


```bash
cd /opt/alarm112

# Build i start
docker compose -f infra/docker-compose.yml up -d --build

# Sprawdź stan
docker compose -f infra/docker-compose.yml ps

# Sprawdź healthcheck
curl http://localhost:5080/health
```

---

## TLS (HTTPS) — Caddy jako reverse proxy

Opcjonalny Caddy serwis jest dostępny w `infra/docker-compose.yml`. Dodaj do compose:

```yaml
caddy:
  image: caddy:2
  ports:
    - "80:80"
    - "443:443"
  volumes:
    - ./Caddyfile:/etc/caddy/Caddyfile
    - caddy-data:/data
    - caddy-config:/config
  depends_on:
    - api
    - admin
```

Plik `infra/Caddyfile`:

```caddyfile
api.twoja-domena.pl {
    reverse_proxy api:8080
}

admin.twoja-domena.pl {
    reverse_proxy admin:8080
}
```

Caddy automatycznie uzyska certyfikaty TLS z Let's Encrypt dla produkcji.

Dla środowiska dev (local HTTPS):

```caddyfile
localhost:5090 {
    reverse_proxy api:8080
}
```

---

## Migracje bazy danych

Skrypty SQL w `db/schema/` numerowane `001_init.sql` ... `021_v26_...sql`.

Aplikuj w kolejności:

```bash
docker compose -f infra/docker-compose.yml exec db psql -U postgres -d alarm112 -f /db/schema/001_init.sql
# ... powtórz dla każdego pliku w kolejności numerycznej
```

Możesz też dodać service `migrate` do compose:

```yaml
migrate:
  image: postgres:16
  volumes:
    - ../db/schema:/migrations
  command: >
    bash -c "for f in /migrations/*.sql; do psql -U postgres -h db -d alarm112 -f $$f; done"
  depends_on:
    db:
      condition: service_healthy
```

---

## Backup bazy danych

### Ręczny backup

```bash
docker compose -f infra/docker-compose.yml exec db pg_dump -U postgres alarm112 > backup_$(date +%Y%m%d_%H%M%S).sql
```

### Automatyczny backup (cron)

```cron
0 2 * * * cd /opt/alarm112 && docker compose -f infra/docker-compose.yml exec -T db pg_dump -U postgres alarm112 > /backups/alarm112_$(date +\%Y\%m\%d).sql
```

Przechowuj backupy poza serwerem (S3, B2, rsync do innego hosta).

---

## Update (nowa wersja)

```bash
cd /opt/alarm112
git pull
docker compose -f infra/docker-compose.yml up -d --build
```

Sprawdź czy nowe migration scripts są do aplikacji po update.

---

## Monitoring minimum

- `/health` endpoint — skonfiguruj monitoring (UptimeRobot, BetterStack) na `http://serwer:5080/health`
- Logi Docker: `docker compose -f infra/docker-compose.yml logs -f api`
- Structured logs z API dostępne przez `docker logs`

---

## Rozwiązywanie problemów

| Problem | Przyczyna | Rozwiązanie |
| --- | --- | --- |
| API nie startuje | `data/` brak | Sprawdź `ContentBundles:DataRoot=/app/data` w Dockerfile |
| 500 na endpointach | Brak content bundli | `docker exec api ls /app/data/` — czy data jest skopiowana |
| DB connection failed | PostgreSQL nie gotowy | Sprawdź healthcheck — poczekaj na `service_healthy` |
| CORS error w przeglądarce | Zły origin | Dodaj origin do `Cors:AllowedOrigins` w appsettings/env |
| 429 Too Many Requests | Rate limiting | Zmniejsz częstość requestów lub zwiększ limit w `Program.cs` |
