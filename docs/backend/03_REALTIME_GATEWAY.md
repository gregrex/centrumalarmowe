# Realtime Gateway

## Cel
Zapewnić spójny stan sesji dla 1–4 klientów mobilnych, admina obserwacyjnego i AI botów.

## Warstwy
- API REST do bootstrapu i lobby,
- SignalR hub do aktualizacji w trakcie sesji,
- tick engine do director/bot logic,
- snapshot store w Redis,
- persistence w SQL dla raportów i contentu.

## Minimalne kontrakty
- CreateSession
- JoinSession
- GetSessionSnapshot
- SendPlayerAction
- EndSession
- GetScenarioCatalog
- GetRoleHudConfig
