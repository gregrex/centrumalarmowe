# Offline and missing player fill

## Scenariusze
- gracz nie dołączył do lobby,
- gracz wyszedł w trakcie,
- gracz ma wysoki ping,
- gracz AFK,
- pełny mecz botów do testu.

## Reguły przejęcia
1. Rola pusta przy starcie dostaje bota z profilu scenariusza.
2. Rola opuszczona w trakcie dostaje takeover po krótkim grace period.
3. Po powrocie gracza bot oddaje kontrolę w bezpiecznym punkcie.
4. Bot zostawia audit trail swoich działań.
5. Jeśli offline jest więcej niż połowa graczy, director przełącza sesję w stabilizację.

## Minimalny UX
- widoczny badge BOT,
- tooltip dlaczego bot przejął rolę,
- historia działań bota,
- opcja odzyskania roli przez gracza.
