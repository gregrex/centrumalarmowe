# Director zdarzeń

## Cel
Sterować napięciem, żeby gra była trudna, ale nie losowo niesprawiedliwa.

## Director bierze pod uwagę
- liczbę aktywnych zdarzeń,
- średni czas oczekiwania,
- dostępność jednostek,
- przeciążenie operatora,
- opóźnienia transportu,
- poziom chaosu na mapie,
- stan AI botów,
- postęp scenariusza.

## Typy zdarzeń
- planowe,
- losowe,
- półplanowe,
- łańcuchowe,
- kryzysowe,
- reputacyjne.

## Reguły
- jeśli system jest na granicy, Director ogranicza część drobnych zdarzeń,
- jeśli gracz sobie radzi dobrze, Director zwiększa presję,
- jeśli brak gracza w roli, Director może ograniczyć złożoność wejść dla tej roli na czas przejęcia przez bota.

## Wyjście
Director publikuje:
- nowe zgłoszenia,
- mutatory sesji,
- eskalacje,
- awarie,
- modyfikatory intensywności audio/UI.
