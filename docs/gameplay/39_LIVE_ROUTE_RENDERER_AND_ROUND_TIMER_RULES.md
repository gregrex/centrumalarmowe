# Gameplay 39 — Live Route Renderer and Round Timer Rules

## Route rules
- dispatch tworzy primary route,
- opóźnienie ruchu może zmienić route state na delayed,
- zdarzenie blokujące tworzy blocked i wymusza reroute,
- reroute zwiększa ETA i może pchnąć objective do at_risk.

## Timer rules
- każda misja ma prosty round timer,
- progi: 180s, 90s, 30s, 10s,
- przy 30s uruchamia się critical mix.
