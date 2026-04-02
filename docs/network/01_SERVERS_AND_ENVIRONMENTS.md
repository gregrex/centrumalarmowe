# Serwery i środowiska

## Lokalne
- API,
- DB,
- Redis,
- SignalR gateway,
- admin,
- test telemetry collector.

## QA/Staging
- pełny stack kontenerowy,
- separacja content bundle,
- testowe buildy klienta.

## Production
- API w kontenerach,
- session gateway,
- Redis,
- DB,
- storage dla bundles i telemetry export,
- panel admina,
- monitoring.

## Porty konfigurowalne
- API_PORT
- ADMIN_PORT
- REDIS_PORT
- DB_PORT
- GATEWAY_PORT
- OTEL_PORT

## Kluczowe usługi
- auth/profile,
- gameplay session,
- content/liveops,
- analytics,
- notifications,
- bot inference / decision service.
