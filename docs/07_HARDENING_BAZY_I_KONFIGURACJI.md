# 07 — Hardening Bazy i Konfiguracji: 112 Centrum Alarmowe

---

## 1. Sekrety — stan obecny vs wymagany

### Problem: Hardcoded credentials w repozytorium

| Lokalizacja | Sekret | Ryzyko |
|-------------|--------|--------|
| `appsettings.json:Security:Jwt:SigningKey` | `"dev-only-signing-key..."` | 🔴 Każdy z dostępem do repo może podpisać token |
| `infra/docker-compose.yml:POSTGRES_PASSWORD` | `postgres` | 🔴 Domyślne hasło w kodzie źródłowym |
| `infra/docker-compose.yml:ConnectionStrings__Main` | `Password=postgres` | 🔴 Connection string z hasłem |

### Rozwiązanie — lokalne dev

```bash
# Użyj dotnet user-secrets (nigdy nie trafia do repo)
cd src/Alarm112.Api
dotnet user-secrets set "Security:Jwt:SigningKey" "your-super-secret-key-32-chars-min"
dotnet user-secrets set "ConnectionStrings:Main" "Host=localhost;Port=5432;Database=alarm112;Username=alarm112_user;Password=STRONGPASSWORD"
```

### Rozwiązanie — produkcja

```bash
# Zmienne środowiskowe (Docker / Kubernetes / Cloud)
export Security__Jwt__SigningKey="$(openssl rand -base64 32)"
export ConnectionStrings__Main="Host=db;Port=5432;Database=alarm112;Username=alarm112_user;Password=${DB_PASS}"
```

---

## 2. appsettings — konfiguracja per środowisko

### Wymagane pliki

```
src/Alarm112.Api/
├── appsettings.json              ← defaults (no secrets, RequireAuth=false)
├── appsettings.Development.json  ← dev overrides (create)
├── appsettings.Production.json   ← prod overrides (create)
└── appsettings.Test.json         ← test overrides (create)
```

### appsettings.Development.json (do utworzenia)

```json
{
  "Security": {
    "RequireAuth": false,
    "EnableDevTokenEndpoint": true
  },
  "Swagger": {
    "Enabled": true
  }
}
```

### appsettings.Production.json (do utworzenia)

```json
{
  "Security": {
    "RequireAuth": true,
    "EnableDevTokenEndpoint": false,
    "Jwt": {
      "SigningKey": ""
    }
  },
  "AllowedHosts": "api.alarm112.pl;localhost",
  "Cors": {
    "AllowedOrigins": "https://admin.alarm112.pl"
  },
  "Swagger": {
    "Enabled": false
  }
}
```

---

## 3. PostgreSQL — hardening

### Problem: User postgres (superuser)

Aplikacja używa użytkownika `postgres` który ma pełne uprawnienia do całego klastra.

### Rozwiązanie: Dedykowany user z minimum uprawnień

```sql
-- Uruchomić jako postgres admin
CREATE USER alarm112_app WITH PASSWORD 'STRONG_PASSWORD_HERE';
CREATE DATABASE alarm112 OWNER alarm112_app;

-- Nadaj tylko niezbędne uprawnienia na schematy
GRANT CONNECT ON DATABASE alarm112 TO alarm112_app;
GRANT USAGE ON SCHEMA public TO alarm112_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO alarm112_app;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO alarm112_app;

-- Dla przyszłych tabel
ALTER DEFAULT PRIVILEGES IN SCHEMA public
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO alarm112_app;
ALTER DEFAULT PRIVILEGES IN SCHEMA public
  GRANT USAGE, SELECT ON SEQUENCES TO alarm112_app;

-- NIE dawaj: SUPERUSER, CREATEDB, CREATEROLE
-- NIE dawaj: DROP TABLE, TRUNCATE (opcjonalnie)
```

### docker-compose.override.yml (gitignored)

```yaml
# infra/docker-compose.override.yml — nie commitować!
version: '3.9'
services:
  db:
    environment:
      POSTGRES_USER: ${POSTGRES_ADMIN_USER:-postgres}
      POSTGRES_PASSWORD: ${POSTGRES_ADMIN_PASSWORD:?DB admin password required}
      POSTGRES_DB: alarm112
  api:
    environment:
      ConnectionStrings__Main: >
        Host=db;Port=5432;Database=alarm112;
        Username=${DB_APP_USER:-alarm112_app};
        Password=${DB_APP_PASSWORD:?DB app password required}
```

---

## 4. Redis — hardening

### Problem: Redis bez uwierzytelniania

```yaml
# Obecny stan — redis bez hasła
redis:
  image: redis:7
  ports:
    - "${REDIS_PORT:-6379}:6379"
```

### Rozwiązanie

```yaml
redis:
  image: redis:7
  command: redis-server --requirepass ${REDIS_PASSWORD:?Redis password required}
  ports:
    - "127.0.0.1:${REDIS_PORT:-6379}:6379"  # Bind tylko do localhost
  environment:
    REDIS_PASSWORD: ${REDIS_PASSWORD}
```

Connection string w appsettings:
```json
{
  "Redis": {
    "Connection": "redis:6379,password=${REDIS_PASSWORD}"
  }
}
```

---

## 5. Migracje bazy danych

### Stan obecny
- 21 migracji SQL w `db/schema/` (001–021)
- PostgreSQL uruchomiony w docker-compose ale **nieużywany przez aplikację**
- Aplikacja używa `InMemorySessionStore`

### Plan podłączenia PostgreSQL

```
Krok 1: Dodać Npgsql / Dapper do Alarm112.Infrastructure
Krok 2: Dodać IDbConnectionFactory
Krok 3: Zaimplementować PostgresSessionStore : ISessionStore
Krok 4: Uruchamiać migracje w CI/CD (lub startup)
Krok 5: Feature flag: InMemory (dev) / Postgres (prod)
```

### Skrypt migracji

```powershell
# tools/run-migrations.ps1

# pokaż kolejność bez wykonywania
.\tools\run-migrations.ps1 -ListOnly

# zastosuj wszystkie migracje do kontenera db z infra/docker-compose.yml
.\tools\run-migrations.ps1
```

Skrypt:
- czyta `POSTGRES_DB` i `POSTGRES_USER` z `infra/.env` lub `infra/.env.example`,
- wykonuje wszystkie pliki `db/schema/*.sql` w kolejności alfabetycznej,
- używa `docker compose exec -T db psql -v ON_ERROR_STOP=1`, więc zatrzymuje się na pierwszym błędzie.

---

## 6. Bezpieczeństwo połączeń

### TLS dla PostgreSQL (produkcja)

```
ConnectionStrings__Main=Host=db;Port=5432;Database=alarm112;
  Username=alarm112_app;Password=XXX;SSL Mode=Require;
  Trust Server Certificate=false
```

### Szyfrowanie danych wrażliwych

Dane gracza (profil, statystyki) powinny być szyfrowane at-rest:
- PostgreSQL transparent data encryption (TDE) lub
- Column-level encryption przez aplikację (dla PII)

---

## 7. .gitignore — wymagane wpisy

```gitignore
# Secrets
.env
.env.production
.env.local
infra/docker-compose.override.yml
src/*/appsettings.Production.json

# User secrets
**/UserSecrets/

# Certificates
*.pfx
*.pem
*.key
```

---

## 8. Security Checklist — baza danych

- [ ] PostgreSQL: dedykowany user (nie postgres)
- [ ] PostgreSQL: minimum uprawnień
- [ ] PostgreSQL: hasło ≥ 16 znaków, losowe
- [ ] PostgreSQL: port nie wystawiony publicznie
- [ ] PostgreSQL: TLS w produkcji
- [ ] Redis: hasło ustawione
- [ ] Redis: port nie wystawiony publicznie
- [ ] Connection strings: poza repo (env vars / secrets)
- [ ] JWT signing key: poza repo, ≥ 32 znaki, losowy
- [ ] Migracje: uruchamiane automatycznie w CI/CD
- [ ] Backup: zautomatyzowany (przynajmniej daily)
- [ ] Soft delete: zaimplementowany dla danych gracza
