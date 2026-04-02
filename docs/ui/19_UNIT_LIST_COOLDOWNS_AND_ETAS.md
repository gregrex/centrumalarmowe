# Lista jednostek: cooldowny i ETA

## Dlaczego to ważne
Sama flaga `available = true/false` jest za słaba. Gracz potrzebuje zrozumieć:
- kto dojedzie szybciej,
- kto wróci do gotowości za chwilę,
- kto jest przeciążony.

## Pola runtime karty jednostki
- call sign,
- typ jednostki,
- aktualna strefa,
- status,
- ETA do wskazanego incydentu,
- cooldown do ponownej gotowości,
- gotowość do shared action,
- tag bot-assisted.

## Sortowanie
- po ETA,
- po cooldown,
- po typie,
- po strefie,
- po ryzyku spóźnienia.

## Badge
- zielony: gotowa,
- żółty: zaraz gotowa / średnie ETA,
- czerwony: zbyt długi dojazd lub cooldown,
- fioletowy: wymaga shared action,
- szary: bot placeholder.
