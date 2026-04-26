# DEPLOY.md — wdrożenie produkcyjne

## Założenia

Aktualny rekomendowany model wdrożenia:

- API i AdminWeb w Docker Compose,
- PostgreSQL i Redis w tym samym stacku albo jako usługi zarządzane,
- reverse proxy / HTTPS realizowane przez infrastrukturę zewnętrzną (np. Nginx, Caddy, Traefik, ingress).

Repo zawiera teraz **opcjonalny** sample reverse proxy dla Caddy w:

- `infra/Caddyfile`
- `infra/docker-compose.proxy.yml`

Możesz go użyć na local/stage albo zastąpić własnym proxy/infrastructure ingress.

---

## 1. Wymagane zmienne środowiskowe

Skopiuj:

```bash
cp infra/.env.example infra/.env
```

W produkcji ustaw co najmniej:

```dotenv
ASPNETCORE_ENVIRONMENT=Production

POSTGRES_DB=alarm112
POSTGRES_USER=alarm112app
POSTGRES_PASSWORD=<silne_haslo>

REDIS_PASSWORD=<silne_haslo>

JWT_SIGNING_KEY=<minimum_32_znaki>
REQUIRE_AUTH=true

ADMIN_USERNAME=<admin_login>
ADMIN_PASSWORD=<silne_haslo_admina>
```

---

## 2. Security posture dla prod

- `appsettings.Production.json` już wymusza:
  - `RequireAuth=true`
  - `EnableDevTokenEndpoint=false`
  - `Swagger:Enabled=false`
- sekret JWT musi być dostarczony z env i mieć minimum 32 znaki
- AdminWeb nie powinien być wystawiany publicznie bez dodatkowych ograniczeń sieciowych / reverse proxy / allowlisty

---

## 3. Start

```bash
docker compose -f infra/docker-compose.yml up -d --build
docker compose -f infra/docker-compose.yml ps
```

Kontrole po starcie:

```bash
curl http://localhost:5080/health/live
curl http://localhost:5080/health/ready
```

`/health/ready` powinno zwrócić `ok=true`. Jeśli zwraca `503`, najpierw napraw brakujące bundla, konfigurację lub połączenie do store.

---

## 4. Migracje i persistence

- schematy SQL są w `db/schema/`
- `docker-compose.yml` uruchamia teraz serwis `migrate`, który:
  - czeka na zdrowego Postgresa,
  - tworzy `schema_migrations`,
  - aplikuje tylko nieuruchomione pliki SQL,
- `PostgresSessionStore` nadal sam dba o tabelę `sessions`, jeśli jej nie ma.

Ręczne uruchomienie migracji nadal jest dostępne:

```powershell
.\tools\run-migrations.ps1
```

---

## 5. Reverse proxy i HTTPS

Wystawiaj publicznie tylko proxy:

- `/` -> AdminWeb
- `/api/*` i `/hubs/session` -> API

Proxy powinno:
- terminować TLS,
- przekazywać nagłówki reverse-proxy,
- ograniczać dostęp do `/admin` i `/api/admin/*`,
- logować requesty i wspierać health probes.

Przykład lokalnego uruchomienia z Caddy:

```bash
docker compose -f infra/docker-compose.yml -f infra/docker-compose.proxy.yml up -d --build
```

Domyślne wejście:

- `http://localhost:5090/`
- `http://localhost:5090/app`
- `http://localhost:5090/admin`
- `http://localhost:5090/api/...`

---

## 6. Runbook po wdrożeniu

1. `curl /health/live`
2. `curl /health/ready`
3. otwórz `/` i `/app`, aby potwierdzić publiczny surface
4. otwórz `/admin` i zaloguj się kontem admina
5. sprawdź `GET /api/admin/dashboard`
6. uruchom próbne `POST /api/sessions/demo`

---

## 7. Główne braki przed pełnym launch

- pełniejsze observability / monitoring zewnętrzny,
- pelniejsze user-facing flows poza demo dashboardem,
- szersze scenariusze produkcyjne zamiast demo/showcase flows.
