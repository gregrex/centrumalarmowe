# 05 — Standardy Walidacji: 112 Centrum Alarmowe

> Kompletny standard walidacji danych wejściowych dla backendu i frontendu.

---

## Zasady ogólne

1. **Walidacja po obu stronach** — zawsze frontend + backend (nigdy tylko jeden)
2. **Fail-fast** — odrzucaj niepoprawne dane jak najwcześniej
3. **Whitelist > Blacklist** — akceptuj znane poprawne wartości, nie filtruj złych
4. **Jednolity format błędów** — zawsze RFC 7807 ProblemDetails
5. **Nie ujawniaj szczegółów implementacji** w komunikatach błędów

---

## Format błędów walidacji (ProblemDetails)

```json
{
  "type": "https://tools.ietf.org/html/rfc7807",
  "title": "Validation Failed",
  "status": 400,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/sessions/abc/actions",
  "errors": {
    "actionType": ["ActionType is required.", "ActionType must be one of: dispatch, escalate, resolve"],
    "payloadJson": ["Payload exceeds maximum allowed size of 1024 bytes."]
  }
}
```

---

## Walidacja DTO — `SessionActionDto`

```csharp
public sealed class SessionActionDto
{
    [Required]
    [StringLength(128, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$", ErrorMessage = "SessionId contains invalid characters.")]
    public required string SessionId { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public required string ActorId { get; set; }

    [Required]
    [AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer")]
    public required string Role { get; set; }

    [Required]
    [AllowedValues("dispatch", "escalate", "resolve")]
    public required string ActionType { get; set; }

    [StringLength(1024)]  // Max 1KB payload
    public string? PayloadJson { get; set; }

    [StringLength(64)]
    public string? CorrelationId { get; set; }
}
```

---

## Walidacja DTO — `DispatchCommandDto`

```csharp
public sealed class DispatchCommandDto
{
    [Required]
    [StringLength(64)]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$")]
    public required string IncidentId { get; set; }

    [Required]
    [StringLength(64)]
    [RegularExpression(@"^[a-zA-Z0-9\-_]+$")]
    public required string UnitId { get; set; }

    [Required]
    [AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer")]
    public required string ActorRole { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 1)]
    public required string ActionId { get; set; }
}
```

---

## Walidacja DTO — `QuickPlayStartRequestDto`

```csharp
public sealed class QuickPlayStartRequestDto
{
    [StringLength(64)]
    [RegularExpression(@"^[a-zA-Z0-9\-_]*$")]
    public string? MissionId { get; set; }

    [AllowedValues("solo", "coop", "bot", null)]
    public string? Mode { get; set; }

    [AllowedValues("CallOperator", "Dispatcher", "OperationsCoordinator", "CrisisOfficer", null)]
    public string? Role { get; set; }
}
```

---

## Reguły walidacji — tabela referencyjna

| Pole | Typ | Min | Max | Pattern | Enum |
|------|-----|-----|-----|---------|------|
| SessionId | string | 1 | 128 | `[a-zA-Z0-9\-_]+` | — |
| ActorId | string | 1 | 64 | — | — |
| Role | string | — | — | — | CallOperator, Dispatcher, OperationsCoordinator, CrisisOfficer |
| ActionType | string | — | — | — | dispatch, escalate, resolve |
| PayloadJson | string? | 0 | 1024 | valid JSON | — |
| CorrelationId | string? | 0 | 64 | — | — |
| IncidentId | string | 1 | 64 | `[a-zA-Z0-9\-_]+` | — |
| UnitId | string | 1 | 64 | `[a-zA-Z0-9\-_]+` | — |
| LobbyId | string | 1 | 128 | `[a-zA-Z0-9\-_]+` | — |

---

## Walidacja w endpointach (Program.cs)

### Automatyczna (ModelState)
Dodać `builder.Services.AddControllers()` lub ręczne sprawdzenie:

```csharp
app.MapPost("/api/sessions/{sessionId}/actions", 
    async (string sessionId, SessionActionDto? action, ...) =>
{
    if (action is null)
        return Results.BadRequest(new { error = "Request body is required." });
    
    var validationResults = new List<ValidationResult>();
    var context = new ValidationContext(action);
    if (!Validator.TryValidateObject(action, context, validationResults, validateAllProperties: true))
    {
        var errors = validationResults
            .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "general")
            .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage).ToArray());
        return Results.ValidationProblem(errors);
    }
    
    // ... proceed
});
```

---

## Walidacja PayloadJson

```csharp
private static bool IsValidPayloadJson(string? payload)
{
    if (payload is null) return true;
    if (payload.Length > 1024) return false;
    try
    {
        JsonDocument.Parse(payload);
        return true;
    }
    catch (JsonException)
    {
        return false;
    }
}
```

---

## Walidacja sessionId w ścieżce URL

Każdy endpoint z `{sessionId}` powinien walidować przed użyciem:

```csharp
if (!Regex.IsMatch(sessionId, @"^[a-zA-Z0-9\-_]{1,128}$"))
    return Results.BadRequest(new { error = "Invalid sessionId format." });

var snapshot = await sessionService.GetSnapshotAsync(sessionId, cancellationToken);
if (snapshot is null)
    return Results.NotFound(new { error = $"Session '{sessionId}' not found." });
```

---

## Walidacja po stronie klienta (Unity)

Przed wysłaniem każdego żądania do API:
1. Sprawdź wymagane pola (nie null/empty)
2. Sprawdź długość stringów
3. Sprawdź wartości enum (ActionType, Role)
4. Wyświetl błąd w UI przed wysłaniem

---

## Komunikaty błędów — zasady UX

| Kategoria | Format | Przykład |
|-----------|--------|---------|
| Required field | "{Pole} jest wymagane." | "Typ akcji jest wymagany." |
| Max length | "{Pole} może mieć maksymalnie {max} znaków." | "ID sesji może mieć maksymalnie 128 znaków." |
| Invalid format | "{Pole} zawiera niedozwolone znaki." | "ID zdarzenia zawiera niedozwolone znaki." |
| Invalid enum | "{Pole} musi być jedną z wartości: {lista}." | "Rola musi być jedną z: CallOperator, Dispatcher..." |
| Not found | "{Zasób} o ID '{id}' nie został znaleziony." | "Sesja 'abc123' nie została znaleziona." |
| Server error | "Wystąpił błąd serwera. Spróbuj ponownie." | (bez szczegółów technicznych) |
