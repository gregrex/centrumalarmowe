# Flow przejęcia roli przez AI

1. Wykryj brak aktywności gracza albo disconnect.
2. Oznacz rolę jako zagrożoną.
3. Uruchom tryb bot standby.
4. Bot przejmuje:
   - odbiór kolejki,
   - monitoring,
   - proste decyzje,
   - krytyczne alarmy.
5. Gracze otrzymują powiadomienie o takeover.
6. Po reconnect:
   - pokaż listę działań bota,
   - pozwól odzyskać kontrolę,
   - zsynchronizuj HUD i kontekst.

## Edge cases
- bot przejmuje rolę w chwili masowego zdarzenia,
- bot i gracz równocześnie wydają polecenie,
- gracz wraca podczas krytycznego alertu,
- bot ma stan niepewności > próg.
