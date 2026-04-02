# V4 expansion scope

## Cel
Dowiezienie pierwszego naprawdę grywalnego wycinka: lobby + start sesji + słowniki + placeholder premium HUD.

## Zakres v4
- lobby,
- ready check,
- reconnect,
- bot fill,
- reference data extended,
- jednostki i incydenty oparte na słownikach,
- walidacja schematów JSON,
- placeholder art pipeline,
- pierwsze assety SVG,
- prosty content validation endpoint.

## Must
- człowiek może utworzyć demo lobby,
- klient może pobrać reference data i atlas manifest,
- content verify sprawdza schematy,
- build nie zależy od finalnych assetów premium,
- role mają własne HUD IDs i icon family.

## Should
- prosty retry i reconnect,
- preview icon catalog w adminie,
- log działań bota przy takeover,
- podstawowy test smoke lobby -> session.

## Could
- public matchmaking queue,
- observer mode,
- hot-swap atlasów w runtime,
- mock analytics dla ready check.

## Najbliższe 150 zadań
1. Dokończyć DTO lobby.
2. Dodać service i endpointy lobby.
3. Rozszerzyć reference data o grupy i meta.
4. Dodać schematy JSON i walidator.
5. Dodać atlas manifest i placeholder SVG.
6. Podpiąć loader atlasu po stronie klienta.
7. Spiąć vertical slice: quick play -> lobby -> session bootstrap.
