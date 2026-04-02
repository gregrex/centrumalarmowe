# Lobby and matchmaking screens

## Lista ekranów
### Screen: Home Hub
Cel:
- wejście do gry,
- wybór: Quick Play / Private Lobby / Matchmaking / Training / Settings.

CTA:
- Zagraj teraz,
- Utwórz lobby,
- Dołącz kodem,
- Trening solo.

### Screen: Private Lobby
Cel:
- pokazać 4 sloty ról,
- pozwolić wybrać scenariusz,
- ustawić poziom trudności,
- włączyć bot fill.

Elementy:
- kod lobby,
- lista graczy,
- sloty roli,
- toggle botów,
- scenariusz,
- ready button,
- start button dla hosta.

### Screen: Matchmaking Queue
Cel:
- pokazać kolejkę i przewidywany czas,
- pozwolić zmienić preferowaną rolę,
- wyjść z kolejki bez chaosu.

### Screen: Ready Check
Cel:
- ostatnie potwierdzenie składu i ról,
- odliczanie do startu,
- szybka informacja o botach.

### Screen: Reconnect Offer
Cel:
- przywrócić gracza do aktywnej sesji,
- pokazać, czy bot aktualnie trzyma rolę,
- dać decyzję: przejmij teraz / obserwuj chwilę.

### Screen: Session Loading
Cel:
- pokazać scenariusz, pogodę, mutatory, role i status połączenia.

## Stany
- waiting,
- partial ready,
- all ready,
- role conflict,
- missing player with bot fallback,
- reconnect pending,
- loading failed,
- start blocked by invalid config.

## Edge cases
- gracz opuszcza ready check,
- host zmienia scenariusz tuż przed startem,
- bot fill włącza się automatycznie,
- dwóch graczy kliknęło tę samą rolę,
- reconnect przy pełnym kryzysie.
