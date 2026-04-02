# FULL MISSION RUNTIME API

## Endpointy
- `GET /api/mission-runtime/demo`
- `GET /api/mission-complete-gate/demo`
- `GET /api/runtime-dispatch-outcomes/demo`
- `GET /api/objective-tracker/demo`
- `GET /api/mission-script/demo`

## Zastosowanie
`mission-runtime/demo`:
- podstawowy snapshot runtime

`mission-complete-gate/demo`:
- warunki końca i powód

`runtime-dispatch-outcomes/demo`:
- przykładowe outcomes

`objective-tracker/demo`:
- stan objective tracker

`mission-script/demo`:
- dane sekwencji misji

## Zasady
- payloady lekkie,
- nazwy kluczy stabilne,
- brak zależności od finalnej bazy,
- gotowe do późniejszego podmiany na prawdziwy runtime backend.
