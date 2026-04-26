# USER_GUIDE.md — przewodnik użytkownika / demo

## Gdzie wejść

Aktualny publiczny surface produktu:

- `http://localhost:5081/` — landing page
- `http://localhost:5081/app` — dashboard demo gracza

Dashboard `/app` prezentuje player-facing dane z API:

- home hub,
- kampanię i mission entry,
- mission briefing,
- quickplay bootstrap,
- showcase mission.

---

## Konto demo / dostęp demo

Repo nie ma jeszcze trwałego systemu kont końcowego użytkownika.

W trybie `Development` demo access do API uzyskasz przez dev token:

```powershell
$body = @{ subject = "demo-player"; role = "Dispatcher" } | ConvertTo-Json
Invoke-RestMethod -Method POST http://localhost:5080/auth/dev-token -ContentType "application/json" -Body $body
```

Przykładowa rola demo:

- `Dispatcher`

Dostęp do dashboardu `/app` nie wymaga logowania; to publiczny widok demonstracyjny.

---

## Co zobaczysz na `/app`

1. Stan API i store.
2. Liczbę kart home huba i skrót bieżącej zmiany.
3. Rozdziały kampanii i liczbę węzłów misji.
4. Mission entry z rolą rekomendowaną.
5. Mission briefing z celami.
6. Showcase mission z krokami demo.

---

## Jak pokazać demo klientowi

1. Uruchom API.
2. Uruchom AdminWeb.
3. Otwórz `/`.
4. Przejdź do `/app`.
5. Pokaż `/admin` jako panel operacyjny zaplecza.

---

## Ograniczenia

- To nadal jest dashboard demo, nie pełny portal użytkownika.
- Dane pochodzą z flow showcase/content bundles.
- Pełny login/rejestracja użytkownika końcowego nie są jeszcze zaimplementowane.
