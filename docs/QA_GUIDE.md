# QA_GUIDE.md — Przewodnik QA

## Przegląd bramek jakości

Projekt ma trzy poziomy weryfikacji:

```
verify.ps1
  ├── dotnet restore
  ├── dotnet build          ← kompilacja (0 errors, 0 warnings)
  ├── dotnet test           ← testy integracyjne (24/24)
  ├── smoke-v26.ps1         ← walidacja plików i struktury
  └── content-verify.ps1   ← walidacja JSON bundli
```

---

## Uruchomienie pełnej weryfikacji

```powershell
cd c:\projekty\centrumalarmowe
.\tools\verify.ps1
```

- Exit code `0` = wszystko zielone
- Exit code `1` = co najmniej jedna bramka nie przeszła (szczegóły na konsoli)

---

## Testy integracyjne API

### Uruchomienie

```powershell
dotnet test tests/Alarm112.Api.Tests/Alarm112.Api.Tests.csproj --logger "console;verbosity=normal"
```

### Zestawy testów

| Klasa | Testy | Pokrycie |
|---|---|---|
| `HealthEndpointTests` | 4 | `/health` — JSON structure, service name, version v26, status 200 |
| `SessionEndpointTests` | 5 | POST demo, GET snapshot, POST action (dispatch/escalate/resolve) |
| `ContentEndpointTests` | 12 | Wszystkie endpointy content — roles, incidents, units, maps, scenarios, HUD |
| `LobbyEndpointTests` | 3 | GET lobby, POST join, GET slots |

### Infrastruktura testów

Testy używają `WebApplicationFactory<Program>` — uruchamiają pełny serwer in-process z prawdziwym DI, bez mockowania. `ContentBundles:DataRoot` jest ustawiony na katalog `data/` w repo.

### Typowe błędy testów

| Błąd | Przyczyna | Rozwiązanie |
|---|---|---|
| `DirectoryNotFoundException` | Brak katalogu `data/` | Sprawdź `ContentBundles:DataRoot` w `Alarm112ApiFactory` |
| `InvalidOperationException` przy `JsonElement` | Pusta kolekcja, `FirstOrDefault` zwraca default | Użyj foreach z warunkiem |
| Build locked (CS2012) | Poprzedni proces dotnet trzyma DLL | `Get-Process dotnet \| Stop-Process -Force` |

---

## Smoke testy

### smoke-v26.ps1

```powershell
.\tools\smoke-v26.ps1
```

Weryfikuje obecność kluczowych plików projektu, bundli JSON i katalogów. Nie wymaga uruchomionego serwera.

### docker-verify.ps1 (wymaga Docker)

```powershell
.\tools\docker-verify.ps1
```

Pełna weryfikacja Docker:
1. Build obrazów
2. Start kontenerów
3. Polling healthchecków (timeout 30s)
4. HTTP smoke check `/health` na API i Admin
5. Cleanup

---

## Walidacja bundli JSON

```powershell
.\tools\content-verify.ps1
```

Weryfikuje, że pliki JSON w `data/` są poprawne składniowo i zawierają wymagane pola.

---

## Sprawdzenie portów

```powershell
.\tools\find-free-port.ps1
```

Testuje dostępność wszystkich portów zdefiniowanych w `.env`. Wyświetla konflikty i sugeruje alternatywy.

---

## Ręczna weryfikacja endpointów

Po uruchomieniu API (`dotnet run --project src/Alarm112.Api`):

```powershell
# Health
Invoke-RestMethod http://localhost:5080/health
# Oczekiwane: { "ok": true, "service": "Alarm112.Api", "version": "v26" }

# Lista ról
Invoke-RestMethod http://localhost:5080/content/roles
# Oczekiwane: JSON z tablicą ról

# Sesja demo
$s = Invoke-RestMethod -Method POST http://localhost:5080/sessions/demo
$s.sessionId  # ID nowej sesji

# Snapshot
Invoke-RestMethod "http://localhost:5080/sessions/$($s.sessionId)/snapshot"

# Akcja dispatch
$action = @{ actionType="dispatch"; actorId="player1"; payloadJson='{"incidentId":"inc-1","unitId":"unit-1"}' } | ConvertTo-Json
Invoke-RestMethod -Method POST "http://localhost:5080/sessions/$($s.sessionId)/action" -Body $action -ContentType "application/json"
```

---

## Swagger UI

Po uruchomieniu API w trybie Development:
- **http://localhost:5080/swagger**

Zawiera dokumentację wszystkich 40+ endpointów z przykładami.

---

## Dodawanie nowych testów

1. Dodaj metodę do odpowiedniej klasy w `tests/Alarm112.Api.Tests/`
2. Klasy testowe dziedziczą z `IClassFixture<Alarm112ApiFactory>` i mają wstrzykowany `HttpClient`
3. Użyj `_client.GetAsync(...)` / `_client.PostAsJsonAsync(...)`
4. Assercje: `response.EnsureSuccessStatusCode()`, `Assert.Equal(HttpStatusCode.OK, ...)`

Przykład:
```csharp
[Fact]
public async Task MyNewEndpoint_Returns200()
{
    var response = await _client.GetAsync("/my-endpoint");
    response.EnsureSuccessStatusCode();
}
```

---

## Metryki jakości (cel)

| Metryka | Aktualnie | Cel |
|---|---|---|
| Testy integracyjne | 24 (100% pass) | 50+ |
| Build warnings | 0 | 0 |
| Smoke gates | 5 bramek | 5 bramek |
| Pokrycie endpointów testami | ~60% | 80%+ |
