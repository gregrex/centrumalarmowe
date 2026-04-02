# AGENTS.md

## Misja agenta
Masz zbudować grę mobilną 2D „112: Centrum Alarmowe” na podstawie dokumentacji znajdującej się w repo.
Twoim celem jest:
- przełożyć dokumentację na kod i działające moduły,
- utrzymywać spójność domeny, UI, backendu i sieci,
- budować iteracyjnie od vertical slice do produkcji,
- uruchamiać buildy, testy i smoke testy po małych krokach.

## Najważniejsze zasady
1. Najpierw czytaj dokumentację, potem implementuj.
2. Nie omijaj AI botów, sieci i systemu zdarzeń — to filary produktu.
3. Nie mieszaj warstw:
   - domena,
   - gameplay,
   - UI,
   - backend,
   - sieć,
   - content,
   - liveops.
4. Po każdej zmianie aktualizuj odpowiednią dokumentację.
5. Wprowadzaj tylko takie uproszczenia, które są zapisane w dokumentach jako MVP.
6. Nie usuwaj funkcji, jeśli da się je zrealizować w wersji prostszej.
7. Gdy brakuje grafiki lub audio, twórz stuby i oznaczaj punkty integracji.
8. Jeśli brak gracza w sesji coop, aktywuj AI bota zgodnie z dokumentacją.

## Źródła prawdy
- `README.md`
- `docs/scenario/*`
- `docs/product/*`
- `docs/gameplay/*`
- `docs/ui/*`
- `docs/network/*`
- `docs/backend/*`
- `docs/ai/*`
- `docs/art/*`
- `docs/audio/*`
- `docs/implementation/*`
- `data/config/*`
- `db/*`
- `infra/*`

## Priorytet wdrożeniowy
1. Vertical slice:
   - jedno miasto,
   - 4 role,
   - 20–30 typów zdarzeń,
   - 1 pełny dyżur,
   - 1 tryb coop,
   - 1 AI bot fallback,
   - podstawowy backend sesji.
2. MVP:
   - kampania podstawowa,
   - 4 role,
   - AI fallback dla każdej roli,
   - panel admina,
   - telemetry,
   - podstawowy liveops.
3. Production:
   - pełna kampania,
   - specjalne scenariusze,
   - rozbudowany balans,
   - rozbudowane audio, grafika i VFX,
   - ranking, sezony, challenge.

## Kryteria jakości
- UI ma być czytelne na telefonie w pionie.
- Decyzje gracza mają być szybkie, ale znaczące.
- Coop ma wymuszać współpracę, nie duplikację działań.
- AI bot ma działać wiarygodnie i przewidywalnie.
- Serwer sesji ma być odporny na reconnect i braki graczy.
