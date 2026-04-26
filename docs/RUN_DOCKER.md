# RUN_DOCKER.md — uruchomienie w Dockerze

## Wymagania

- Docker Desktop / Docker Engine z Compose v2
- skopiowany plik `infra\.env`

---

## 1. Przygotowanie `.env`

```powershell
Copy-Item infra\.env.example infra\.env
```

Minimalne rzeczy do sprawdzenia w `infra\.env`:

- `POSTGRES_PASSWORD`
- `REDIS_PASSWORD`
- `JWT_SIGNING_KEY`
- `ADMIN_USERNAME`
- `ADMIN_PASSWORD`
- `REQUIRE_AUTH`

---

## 2. Start całego stacka

```powershell
docker compose -f infra\docker-compose.yml up -d --build
```

Po starcie:

- API: `http://localhost:5080`
- API Swagger: `http://localhost:5080/swagger`
- API live: `http://localhost:5080/health/live`
- API ready: `http://localhost:5080/health/ready`
- landing: `http://localhost:5081/`
- dashboard gracza: `http://localhost:5081/app`
- panel admina: `http://localhost:5081/admin`

Opcjonalny pojedynczy gateway z Caddy:

```powershell
docker compose -f infra\docker-compose.yml -f infra\docker-compose.proxy.yml up -d --build
```

Wtedy wejście publiczne jest pod:

- `http://localhost:5090/`
- `http://localhost:5090/app`
- `http://localhost:5090/admin`
- `http://localhost:5090/api/...`

---

## 3. Logowanie do AdminWeb

Panel admina używa Basic Auth z wartościami z `.env`:

- login: `ADMIN_USERNAME`
- hasło: `ADMIN_PASSWORD`

Jeśli `REQUIRE_AUTH=true`, AdminWeb powinien mieć poprawny `JWT_SIGNING_KEY`, bo sam pobiera dane chronione z API.

---

## 4. Weryfikacja po starcie

```powershell
.\tools\docker-verify.ps1
```

Skrypt:
1. buduje obrazy,
2. uruchamia kontenery,
3. uruchamia też automatyczny serwis `migrate` dla `db/schema`,
4. czeka na zdrowe API,
5. odpytuje kluczowe endpointy API,
6. wykonuje prawdziwy flow sesji (`demo session -> dispatch action -> odczyt zmienionego snapshotu`),
7. odpytuje landing, `/app`, `/health` i chronione `/admin`,
8. sprząta środowisko.

Możesz też sprawdzić ręcznie:

```powershell
Invoke-RestMethod http://localhost:5080/health/live
Invoke-RestMethod http://localhost:5080/health/ready
docker compose -f infra\docker-compose.yml ps
```

Jeśli chcesz odpalić migracje ręcznie:

```powershell
.\tools\run-migrations.ps1
```

---

## 5. Zatrzymanie

```powershell
docker compose -f infra\docker-compose.yml down
```

Pełne czyszczenie z wolumenami:

```powershell
docker compose -f infra\docker-compose.yml down -v
```
