# Incident chains and director rules

## Cel
Director ma sterować rytmem bez niszczenia sprawczości gracza.

## Wejścia directora
- liczba aktywnych incydentów,
- liczba nieobsłużonych zgłoszeń,
- dostępność jednostek,
- średni czas reakcji,
- poziom błędów klasyfikacji,
- obciążenie każdego gracza/roli,
- liczba ról przejętych przez boty,
- wynik aktualnej misji.

## Reguły
1. Jeśli gracz przegrywa i system jest czerwony, ograniczaj drobne eventy i dawaj okna oddechu.
2. Jeśli zespół kontroluje sytuację zbyt łatwo, podbijaj średnią trudność, nie od razu katastrofą.
3. Jeśli bot zastępuje człowieka, director zmniejsza tempo wymiany informacji i komplikuje decyzje mniej agresywnie.
4. Jeśli dwóch graczy jest offline, sesja dostaje tryb stabilizacji, nie tryb kary.
5. Event chain ma być czytelny: gracz widzi źródło, rozwój i skutek.

## Szablon łańcucha
- trigger początkowy,
- opóźnienie do eskalacji,
- warunki przerwania,
- koszt zignorowania,
- skutki globalne,
- warianty pogodowe,
- warianty coop/bot.
