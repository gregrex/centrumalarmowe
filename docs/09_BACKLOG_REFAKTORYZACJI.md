# 09 — Backlog Refaktoryzacji: 112 Centrum Alarmowe

> Zidentyfikowany dług techniczny posortowany wg priorytetu.

---

## Krytyczne (blokują produkcję)

| ID | Problem | Lokalizacja | Akcja |
|----|---------|-------------|-------|
| REFACT-01 | `Program.cs` — 962 linie, wszystko w jednym pliku | `src/Alarm112.Api/Program.cs` | Wydzielić endpoint groups do `Endpoints/` |
| REFACT-02 | `BotTickHostedService` zdefiniowany w `Program.cs` | `Program.cs` lines 937-957 | Przenieść do `Application/Services/` |
| REFACT-03 | `DevTokenRequest` record w `Program.cs` | `Program.cs` line 959 | Przenieść do Contracts |
| REFACT-04 | Brak `appsettings.Production.json` | `src/Alarm112.Api/` | Utworzyć z bezpiecznymi defaultami |
| REFACT-05 | JWT key fallback w kodzie | `Program.cs` lines 42-43, 210 | Usunąć fallback, wymagać env var |
| REFACT-06 | `BotTickHostedService` używa hardcoded `"DEMO112"` | `Program.cs` line 953 | Pobierać z ISessionStore.GetActiveSessionIds() |

---

## Wysokie (fix przed MVP release)

| ID | Problem | Lokalizacja | Akcja |
|----|---------|-------------|-------|
| REFACT-07 | Brak walidacji DTO w endpointach | Cały `Program.cs` | Dodać `Validator.TryValidateObject` |
| REFACT-08 | Brak null check na snapshot (`GetSnapshotAsync`) | `SessionService.cs` | Zwracać 404 gdy sesja nie istnieje |
| REFACT-09 | Brak paginacji na endpointach list | `/api/sessions`, `/api/active-incidents` | Dodać `page`/`pageSize` params |
| REFACT-10 | Brak ILogger w serwisach domenowych | `SessionService`, `BotDirector` etc. | Dodać ILogger<T> injection |
| REFACT-11 | Swagger bez wersjonowania API | `Program.cs` | Dodać `/api/v1/` prefix |
| REFACT-12 | CORS `AllowAnyMethod` + `AllowCredentials` | `Program.cs` lines 88-90 | Ograniczyć do GET/POST |

---

## Średnie (backlog sprintu)

| ID | Problem | Lokalizacja | Akcja |
|----|---------|-------------|-------|
| REFACT-13 | `catch { return snapshot; }` — połknięcie błędów | `SessionService.cs` | Logować + rzucać specificzny wyjątek |
| REFACT-14 | `DemoFactory` w `Alarm112.Domain` | `src/Alarm112.Domain/DemoFactory.cs` | Przenieść do Application/Factories (DI) |
| REFACT-15 | 28 serwisów zarejestrowanych w `Program.cs` | `Program.cs` lines 118-144 | Extension method `AddApplicationServices()` |
| REFACT-16 | Brak `CancellationToken` propagacji w `SessionService` | `SessionService.cs` | Propagować do store |
| REFACT-17 | Brak response caching na reference-data | `/api/reference-data` | Dodać `[ResponseCache]` lub ETag |
| REFACT-18 | `InMemorySessionStore` bez limitów rozmiaru | `InMemorySessionStore.cs` | Dodać max session count |
| REFACT-19 | Brak correlation ID w logach | Cały pipeline | Dodać `X-Correlation-Id` middleware |

---

## Niskie (tech debt)

| ID | Problem | Lokalizacja | Akcja |
|----|---------|-------------|-------|
| REFACT-20 | `using System.Text.Json` zamiast System.Text.Json.Serialization | Kilka plików | Unify imports |
| REFACT-21 | Brak XML doc comments na publicznych metodach | Interfejsy | Dodać `<summary>` |
| REFACT-22 | `var` zamiast explicit types w LINQ | `SessionService.cs` | Style preference |
| REFACT-23 | Brak `sealed` na `SessionHub` | `SessionHub.cs` | Dodać `sealed` |
| REFACT-24 | `.NET 10-preview` w Dockerfile | `src/*/Dockerfile` | Aktualizować gdy stable |
| REFACT-25 | Admin panel HTML w `Program.cs` | `AdminWeb/Program.cs` | Wydzielić do pliku `.html` lub Razor |

---

## Refaktoryzacja Program.cs — propozycja struktury

```
src/Alarm112.Api/
├── Program.cs                    ← tylko builder + pipeline + app.Run()
├── ServiceCollectionExtensions.cs ← AddApplicationServices(), AddSecurityServices()
├── Endpoints/
│   ├── SessionEndpoints.cs        ← MapSessionEndpoints()
│   ├── LobbyEndpoints.cs          ← MapLobbyEndpoints()
│   ├── ContentEndpoints.cs        ← MapContentEndpoints()
│   ├── AuthEndpoints.cs           ← MapAuthEndpoints()
│   └── ...
├── Middleware/
│   ├── SecurityHeadersMiddleware.cs
│   ├── CorrelationIdMiddleware.cs
│   └── RequestValidationMiddleware.cs
└── Hubs/
    └── SessionHub.cs
```

### Program.cs po refaktoryzacji (~50 linii)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSecurityServices(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(...);
builder.Services.AddProblemDetails();

var app = builder.Build();
app.UseSecurityHeaders();
app.UseExceptionHandler();
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
    app.UseSwagger().UseSwaggerUI();

app.MapHealthChecks("/health");
app.MapSessionEndpoints();
app.MapLobbyEndpoints();
app.MapContentEndpoints();
app.MapAuthEndpoints();
app.MapHub<SessionHub>("/hubs/session");

app.Run();
```

---

## Mierniki długu technicznego

| Kategoria | Liczba issues | Czas estymowany |
|-----------|--------------|-----------------|
| Krytyczne | 6 | ~2 dni |
| Wysokie | 6 | ~3 dni |
| Średnie | 7 | ~3 dni |
| Niskie | 6 | ~1 dzień |
| **Razem** | **25** | **~9 dni** |
