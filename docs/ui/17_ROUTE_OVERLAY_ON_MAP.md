# Route overlay na mapie

## Cel
Dispatcher ma widzieć trasę bez przełączania kontekstu.

## Co pokazujemy
- linię trasy,
- segmenty ruchu,
- blokady i warningi,
- węzeł startowy i docelowy,
- ETA,
- dystans,
- stan krytyczności overlay.

## Stany
- hidden,
- preview,
- locked-after-dispatch,
- stale,
- invalid.

## Reguły UX
- overlay nie może zasłaniać kart incydentu,
- warning segmenty muszą być grubsze lub migające, ale subtelnie,
- start i cel muszą być odróżnialne ikoną,
- przy kilku aktywnych trasach pokazujemy tylko jedną główną i resztę jako ghost.

## Edge cases
- brak dostępnej trasy,
- blokada drogi,
- dwie jednostki jadą do tego samego incydentu,
- przestarzały preview po zmianie statusu incydentu,
- overlay znikający po zakończeniu interwencji.
