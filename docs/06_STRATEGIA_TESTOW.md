# 06 — Strategia Testów: 112 Centrum Alarmowe

> Kompletna strategia testowania backendu i infrastruktury.

---

## Piramida testów

```
        /\
       /E2E\         Playwright (tests/e2e/)
      /──────\        ~20 scenariuszy
     /  Integ \      WebApplicationFactory (Alarm112.Api.Tests/)
    /──────────\      ~50 testów
   /  Unit     \     xUnit pure (nowe pliki)
  /─────────────\    ~100 testów
```

---

## A. Testy jednostkowe (nowe)

### Lokalizacja
`tests/Alarm112.Api.Tests/Unit/`

### Zakres

#### SessionServiceTests
```csharp
// ✅ Dispatch: pending incident + available unit → dispatched
// ✅ Dispatch: already dispatched incident → no change
// ✅ Dispatch: invalid payload JSON → returns snapshot unchanged
// ✅ Dispatch: null payload → returns snapshot unchanged
// ✅ Escalate: pending → escalated (if applicable)
// ✅ Resolve: dispatched → resolved
// ✅ Unknown action type → snapshot unchanged
// ✅ Create demo session → has incidents and units
```

#### InMemorySessionStoreTests
```csharp
// ✅ GetOrAdd: new key → calls factory
// ✅ GetOrAdd: existing key → returns cached
// ✅ Save: overwrites existing
// ✅ GetActiveSessionIds: returns all keys
// ✅ Concurrent access: thread-safe (multiple writers)
```

#### ValidationTests
```csharp
// ✅ SessionActionDto: null ActionType → invalid
// ✅ SessionActionDto: unknown ActionType → invalid
// ✅ SessionActionDto: unknown Role → invalid
// ✅ SessionActionDto: oversized PayloadJson → invalid
// ✅ SessionActionDto: invalid SessionId chars → invalid
// ✅ DispatchCommandDto: missing IncidentId → invalid
// ✅ DispatchCommandDto: invalid chars in UnitId → invalid
```

---

## B. Testy integracyjne (istniejące + rozszerzenia)

### Lokalizacja
`tests/Alarm112.Api.Tests/` (istniejące) + dodatkowe klasy

### Istniejące testy (baseline)
- `HealthEndpointTests` — 4 testy ✅
- `SessionEndpointTests` — 5 testów ✅
- `LobbyEndpointTests` — 3 testy ✅
- `ContentEndpointTests` — 12 testów ✅

### Brakujące testy integracyjne

#### SecurityEndpointTests
```csharp
// ❌ Brak: POST /api/sessions/{id}/actions bez tokena → 401 (gdy RequireAuth=true)
// ❌ Brak: POST /api/sessions/{id}/actions z wygasłym tokenem → 401
// ❌ Brak: POST /api/sessions/{id}/actions z niepoprawnym tokenem → 401
// ❌ Brak: GET /auth/dev-token gdy endpoint wyłączony → 404
```

#### ValidationEndpointTests
```csharp
// ❌ Brak: POST /actions z null body → 400
// ❌ Brak: POST /actions z brakującym ActionType → 400
// ❌ Brak: POST /actions z nieznanym ActionType → 400
// ❌ Brak: POST /actions z nieznaną Role → 400
// ❌ Brak: POST /actions z oversized PayloadJson → 400
// ❌ Brak: GET /api/sessions/{invalid_sessionId} → 400 lub 404
```

#### SecurityHeaderTests
```csharp
// ❌ Brak: response zawiera X-Frame-Options: DENY
// ❌ Brak: response zawiera X-Content-Type-Options: nosniff
// ❌ Brak: response zawiera Referrer-Policy
```

#### RateLimitTests
```csharp
// ❌ Brak: 201+ requests w 10s → 429
```

---

## C. Testy E2E (Playwright) — do implementacji

### Lokalizacja
`tests/e2e/`

### Setup
```json
// package.json
{
  "scripts": {
    "test:e2e": "playwright test"
  },
  "devDependencies": {
    "@playwright/test": "^1.44.0"
  }
}
```

### Scenariusze

#### smoke.spec.ts
```typescript
// ✅ /health → 200 + ok:true
// ✅ /swagger → dostępny w Development
// ✅ Admin panel → ładuje się (status 200)
```

#### session-flow.spec.ts
```typescript
// ✅ POST /api/sessions/demo → sessionId
// ✅ GET /api/sessions/{id} → snapshot z incidents i units
// ✅ POST /api/sessions/{id}/actions z dispatch → success:true
// ✅ GET /api/sessions/{id}/round-state → round state
```

#### security.spec.ts
```typescript
// ✅ POST /api/sessions/{id}/actions bez tokena (RequireAuth=true) → 401
// ✅ POST /auth/dev-token (enabled) → Bearer token
// ✅ POST /auth/dev-token (disabled) → 404
// ✅ GET /api/sessions/INVALID_SESSION_ID_!@# → 400 lub 404
```

---

## D. Testy bezpieczeństwa logicznego

### SessionOwnershipTests
```csharp
// ✅ Użytkownik A nie może modyfikować sesji użytkownika B
//    (gdy auth włączone i sesje są per-user)
```

### InjectionTests
```csharp
// ✅ sessionId = "'; DROP TABLE sessions; --" → 400 (walidacja)
// ✅ PayloadJson = 10MB → 400
// ✅ Role = "<script>alert(1)</script>" → 400
```

---

## Konfiguracja testowa

### Alarm112ApiFactory — wymagane ulepszenia
```csharp
// Wersja z auth włączonym (dla security testów)
public sealed class Alarm112ApiFactoryWithAuth : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("Security:RequireAuth", "true");
        builder.UseSetting("Security:EnableDevTokenEndpoint", "true");
        builder.UseSetting("Security:Jwt:SigningKey", "test-key-32-chars-minimum-length!!");
    }
}
```

### Helper: GenerateTestToken
```csharp
public static string GenerateTestToken(string role = "Dispatcher")
{
    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("test-key-32-chars-minimum-length!!"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: "Alarm112.Api",
        audience: "Alarm112.Client",
        claims: new[] { new Claim(ClaimTypes.Role, role) },
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds);
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

---

## Metryki pokrycia — cele

| Typ | Obecne | Cel |
|-----|--------|-----|
| Unit tests | 0 | ~80% kluczowych metod |
| Integration tests | ~24 testy (happy path) | ~60 testów (+neg/security) |
| E2E tests | 0 | ~20 scenariuszy |
| Security tests | 0 | ~15 przypadków |

---

## Uruchamianie testów

```powershell
# Wszystkie testy
dotnet test Alarm112.sln

# Tylko jednostkowe
dotnet test tests/Alarm112.Api.Tests/ --filter "Category=Unit"

# Tylko integracyjne
dotnet test tests/Alarm112.Api.Tests/ --filter "Category=Integration"

# Z coverage
dotnet test Alarm112.sln --collect:"XPlat Code Coverage"

# E2E (Playwright)
cd tests/e2e && npx playwright test
```
