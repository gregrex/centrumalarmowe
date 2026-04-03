# 02 — Ryzyka Bezpieczeństwa: 112 Centrum Alarmowe

> Klasyfikacja wg OWASP Top 10 + OWASP ASVS L1/L2

---

## Macierz ryzyk

| ID | Kategoria | Ryzyko | Prawdopodobieństwo | Wpływ | Priorytet | Status |
|----|-----------|--------|-------------------|-------|-----------|--------|
| SEC-01 | A01 Broken Access Control | Brak auth na API | Wysokie | Krytyczny | 🔴 | ✅ NAPRAWIONE (appsettings.Production.json) |
| SEC-02 | A02 Cryptographic Failures | Hardcoded JWT key | Wysokie | Krytyczny | 🔴 | ✅ NAPRAWIONE (env var, fail-fast startup) |
| SEC-03 | A01 Broken Access Control | Admin panel bez auth | Wysokie | Krytyczny | 🔴 | ✅ NAPRAWIONE (Basic Auth middleware) |
| SEC-04 | A01 Broken Access Control | Dev token endpoint otwarty | Wysokie | Krytyczny | 🔴 | ✅ NAPRAWIONE (disabled w Production) |
| SEC-05 | A02 Cryptographic Failures | Hardcoded DB credentials | Wysokie | Wysoki | 🔴 | ✅ NAPRAWIONE (env var w docker-compose) |
| SEC-06 | A03 Injection | Brak walidacji DTO | Średnie | Wysoki | 🟠 | ✅ NAPRAWIONE (DataAnnotations na 3 DTO) |
| SEC-07 | A05 Security Misconfiguration | Brak security headers | Wysokie | Średni | 🟠 | ✅ NAPRAWIONE (SecurityHeadersMiddleware) |
| SEC-08 | A05 Security Misconfiguration | CORS + AllowCredentials | Średnie | Wysoki | 🟠 | ⚠️ CZĘŚCIOWE (ograniczone do known origins) |
| SEC-09 | A05 Security Misconfiguration | Swagger publiczny | Wysokie | Średni | 🟠 | ✅ NAPRAWIONE (tylko w Development) |
| SEC-10 | A04 Insecure Design | Rate limiter niepodpięty | Średnie | Średni | 🟠 | ✅ NAPRAWIONE (RequireRateLimiting na POST) |
| SEC-11 | A05 Security Misconfiguration | Redis bez auth | Niskie | Średni | 🟡 | 📋 BACKLOG |
| SEC-12 | A05 Security Misconfiguration | AllowedHosts: "*" | Niskie | Niski | 🟡 | 📋 BACKLOG |
| SEC-13 | A04 Insecure Design | InMemory — brak persystencji | Niskie | Wysoki | 🟡 | ⚠️ PostgresSessionStore gotowy, nie podpięty |
| SEC-14 | A09 Security Logging Failures | Brak audit log | Niskie | Średni | 🟡 | 📋 BACKLOG |
| SEC-15 | A01 Broken Access Control | SignalR hub bez auth | Niskie | Średni | 🟡 | 📋 BACKLOG |
| SEC-16 | A03 Injection | PayloadJson bez limitu | Niskie | Niski | 🟢 | ✅ NAPRAWIONE (StringLength validation) |
| SEC-17 | A03 Injection | innerHTML XSS w AdminWeb | Wysokie | Wysoki | 🔴 | ✅ NAPRAWIONE (textContent w admin.js) |
| SEC-18 | A09 Security Logging Failures | catch{} bez logów w SessionService | Średnie | Średni | 🟠 | ✅ NAPRAWIONE (ILogger + catch(JsonException)) |
| SEC-19 | A07 Auth Failures | AdminWeb hardcoded password fallback | Krytyczny | Krytyczny | 🔴 | ✅ NAPRAWIONE (fail-fast, min 12 chars) |

---

## Szczegółowe opisy ryzyk krytycznych

### SEC-01: Brak autoryzacji na wszystkich endpointach API

**Opis:** `appsettings.json` ustawia `Security:RequireAuth = false`. Wszystkie ~80 endpointów API oraz hub SignalR są dostępne bez jakiegokolwiek tokenu.

**Wektor ataku:**
```
Atakujący → GET /api/sessions/{sessionId} → pełny stan gry (bez tokenu)
Atakujący → POST /api/sessions/{sessionId}/actions → modyfikacja sesji (bez tokenu)
Atakujący → POST /api/sessions/{sessionId}/dispatch → dispatch komend (bez tokenu)
```

**Naprawa:**
```json
// appsettings.Production.json
{
  "Security": {
    "RequireAuth": true,
    "EnableDevTokenEndpoint": false
  }
}
```

---

### SEC-02: Hardcoded JWT Signing Key

**Opis:** Klucz podpisywania JWT jest w repozytorium:
```
"SigningKey": "dev-only-signing-key-change-me-to-32-plus-chars"
```
Każdy z dostępem do repo może podpisać dowolny token z dowolną rolą.

**Naprawa:**
- Usunąć z `appsettings.json`
- Użyć `dotnet user-secrets` lokalnie
- W produkcji: zmienna środowiskowa `Security__Jwt__SigningKey` lub secret manager

---

### SEC-03: Admin Panel bez uwierzytelniania

**Opis:** `Alarm112.AdminWeb` serwuje panel admina pod `/` bez żadnej weryfikacji tożsamości.

**Wektor ataku:**
```
Atakujący → GET http://host:5081/ → pełny panel administracyjny
```

**Naprawa:**
- Dodać Basic Auth middleware (dev) lub JWT cookie auth (prod)
- Dodać ekran logowania
- Ograniczyć dostęp przez reverse proxy (IP whitelist)

---

### SEC-04: Dev Token Endpoint — otwarty generator tokenów

**Opis:** `POST /auth/dev-token` z `EnableDevTokenEndpoint=true` pozwala każdemu wygenerować token jako dowolna rola.

**Wektor ataku:**
```bash
curl -X POST /auth/dev-token -d '{"Role":"CrisisOfficer"}' → Bearer token z pełnymi uprawnieniami
```

**Naprawa:**
```json
// appsettings.Production.json
{
  "Security": {
    "EnableDevTokenEndpoint": false
  }
}
```

---

### SEC-05: Hardcoded PostgreSQL credentials

**Plik:** `infra/docker-compose.yml`
```yaml
POSTGRES_PASSWORD: postgres
ConnectionStrings__Main=Host=db;...Password=postgres
```

**Naprawa:**
```yaml
# docker-compose.override.yml (gitignored)
environment:
  POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
  ConnectionStrings__Main: Host=db;Port=5432;Database=alarm112;Username=${DB_USER};Password=${DB_PASSWORD}
```
Lub użyć Docker Secrets.

---

## OWASP Top 10 Coverage

| OWASP | Nazwa | Status w projekcie |
|-------|-------|--------------------|
| A01 | Broken Access Control | 🔴 RequireAuth=false, Admin bez auth |
| A02 | Cryptographic Failures | 🔴 Hardcoded JWT key |
| A03 | Injection | 🟠 Brak walidacji DTO, PayloadJson raw parse |
| A04 | Insecure Design | 🟠 Rate limiter nie działa, InMemory |
| A05 | Security Misconfiguration | 🟠 Swagger publiczny, AllowedHosts=* |
| A06 | Vulnerable Components | 🟡 .NET 10-preview |
| A07 | Authentication Failures | 🔴 Brak wymuszenia auth |
| A08 | Software Integrity Failures | 🟡 Brak CI/CD, brak podpisywania |
| A09 | Security Logging | 🟡 Brak audit log |
| A10 | SSRF | 🟢 Brak zewnętrznych requestów z user input |

---

## Security Headers — brakujące

| Nagłówek | Wartość rekomendowana | Status |
|----------|----------------------|--------|
| `X-Frame-Options` | `DENY` | ❌ brak |
| `X-Content-Type-Options` | `nosniff` | ❌ brak |
| `X-XSS-Protection` | `1; mode=block` | ❌ brak |
| `Referrer-Policy` | `strict-origin-when-cross-origin` | ❌ brak |
| `Content-Security-Policy` | `default-src 'self'` | ❌ brak |
| `Strict-Transport-Security` | `max-age=31536000` | ❌ brak |
| `Permissions-Policy` | `camera=(), microphone=()` | ❌ brak |

---

## Plan naprawy — kolejność

```
Tydzień 1 (Krytyczne):
  1. appsettings.Production.json z RequireAuth=true, DevToken=false
  2. Usunięcie JWT key z appsettings.json → user-secrets + env var
  3. Basic Auth na AdminWeb
  4. Security headers middleware

Tydzień 2 (Wysokie):
  5. DTO validation (DataAnnotations)
  6. Rate limiter podpięcie do endpointów
  7. Swagger tylko w Development
  8. CORS hardening (bez AllowAnyMethod w prod)

Tydzień 3 (Średnie):
  9. Redis auth w docker-compose
  10. AllowedHosts konfiguracja
  11. Audit logging middleware
  12. SignalR auth requirement
```
