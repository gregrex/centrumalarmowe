# Session State Machine

## Stany
- Draft
- Lobby
- Countdown
- Active
- Recovery
- Summary
- Archived

## Przejścia
- Draft -> Lobby po utworzeniu sesji.
- Lobby -> Countdown gdy spełnione minimalne warunki startu.
- Countdown -> Active po potwierdzeniu gotowości.
- Active -> Recovery gdy utracono część połączeń lub serwer przełącza boty.
- Recovery -> Active po odzyskaniu spójności.
- Active -> Summary po upływie czasu lub spełnieniu warunków końca.
- Summary -> Archived po zapisaniu raportu.

## Reguły AI takeover
- jeżeli rola jest pusta w lobby: start z botem,
- jeżeli gracz rozłączy się w Active: bot wchodzi po grace period,
- jeżeli gracz wraca: bot oddaje sterowanie po snapshot sync.
