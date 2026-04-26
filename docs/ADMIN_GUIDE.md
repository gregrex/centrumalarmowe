# ADMIN_GUIDE.md — przewodnik administratora

## Dostęp

Aktualny układ webowy:

- `http://localhost:5081/` — landing page
- `http://localhost:5081/app` — dashboard demo gracza
- `http://localhost:5081/admin` — panel administratora

Panel admina jest chroniony **Basic Auth**.

Przykład lokalny:

- login: `admin`
- hasło: `AdminDemoPass_12345`

---

## Co robi panel admina

Panel `/admin` agreguje dane z API i pokazuje:

- status backendu,
- aktywne sesje,
- wynik walidacji contentu,
- liczbę aktywnych incydentów,
- dostępność jednostek i bot backfill.

Źródło danych:

- publiczne `GET /health`
- chronione `GET /api/sessions`
- chronione `GET /api/content/validate`
- chronione runtime endpoints sesji

Jeśli API działa z `RequireAuth=true`, AdminWeb musi mieć poprawny `ApiAuth__Jwt__SigningKey`.

---

## Operacje codzienne

### 1. Kontrola zdrowia

```powershell
Invoke-RestMethod http://localhost:5080/health/live
Invoke-RestMethod http://localhost:5080/health/ready
```

### 2. Kontrola contentu

```powershell
Invoke-RestMethod http://localhost:5080/api/content/validate
.\tools\content-verify.ps1
```

### 3. Kontrola demo sesji

```powershell
Invoke-RestMethod -Method POST http://localhost:5080/api/sessions/demo
Invoke-RestMethod http://localhost:5080/api/sessions?page=1&pageSize=20
```

### 4. Pełna lokalna bramka

```powershell
.\tools\verify.ps1
```

---

## Zmienne środowiskowe admina

| Zmienna | Opis |
|---|---|
| `AdminAuth__Username` | login do `/admin` |
| `AdminAuth__Password` | hasło do `/admin` |
| `ApiBaseUrl` | URL backendu API |
| `ApiAuth__Jwt__SigningKey` | klucz JWT do odpytywania chronionych endpointów API |
| `ApiAuth__Jwt__Issuer` | issuer tokenu dla AdminWeb |
| `ApiAuth__Jwt__Audience` | audience tokenu dla AdminWeb |

---

## Najczęstsze problemy

| Problem | Przyczyna | Co zrobić |
|---|---|---|
| `/admin` zwraca `401` | brak / zły Basic Auth | sprawdź `AdminAuth__Username` i `AdminAuth__Password` |
| dashboard pokazuje `auth-not-configured` | brak `ApiAuth__Jwt__SigningKey` | ustaw ten sam klucz co w API |
| readiness zwraca `503` | brak danych lub problem ze store | sprawdź `data/`, content validation i połączenie do Postgresa |
