
# BOT sync, handoff and rejoin

## Cel
Nie dopuścić, aby rozłączenie jednego gracza zniszczyło całą sesję.

## Tryby przejęcia
1. Assist mode — BOT tylko podpowiada.
2. Soft takeover — BOT wykonuje bezpieczne akcje rutynowe.
3. Full takeover — BOT przejmuje slot i działa sam.

## Reguły
- po 2 sekundach braku heartbeat: soft takeover,
- po 6 sekundach: full takeover,
- po reconnect gracz może odzyskać rolę bez resetu sesji,
- wszystkie akcje BOT-a muszą mieć `reason_code`.

## Zdarzenia realtime
- `player.connection.lost`,
- `bot.takeover.started`,
- `bot.takeover.full`,
- `player.rejoined`,
- `player.control.restored`.
