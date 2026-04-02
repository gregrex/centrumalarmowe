# API v7: active incidents, filtry, route preview, shared actions

## Endpointy
- `GET /api/sessions/{sessionId}/active-incidents`
- `GET /api/map-filters`
- `POST /api/sessions/{sessionId}/route-preview`
- `POST /api/sessions/{sessionId}/shared-actions`

## Cel
Dać klientowi minimum danych do zbudowania pierwszego czytelnego HUD-u mobilnego dla operatora i dispatchera.

## Contract notes
- DTO mają być małe,
- pola tekstowe przez localization keys tam, gdzie to możliwe,
- route preview ma działać bez pełnego pathfindingu,
- shared action ma zwracać wynik natychmiast, nawet jeśli to tylko scaffold.
