# Split-screen role panels

## Założenie
W trybie debug, local coop lub test build chcemy pokazać dwa role panels na jednym ekranie.

## Układy v8
1. Operator + Dispatcher
2. Coordinator + Crisis Officer

## Reguły
- każdy panel ma własny kolor akcentu,
- header roli musi być stale widoczny,
- unread i alert counters są lokalne dla panelu,
- przełączanie focusu nie może resetować stanu panelu.

## Minimalne elementy panelu
- rola,
- stan gracza/bota,
- focus incident,
- unread count,
- primary KPI,
- primary CTA,
- alert stripe.

## Co jest poza MVP
- pełny 4-role split-screen,
- dynamiczne resize,
- cross-device drag and drop.
