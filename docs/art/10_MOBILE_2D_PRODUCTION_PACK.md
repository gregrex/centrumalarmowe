# Mobile 2D production pack

## Cel
Dopiąć styl premium 2D bez zabijania produkcji. Repo ma wspierać szybkie prototypowanie: placeholder SVG -> atlas -> themed sprite -> premium repaint.

## Pipeline
1. Definiujesz rekord w słowniku assetów.
2. Tworzysz placeholder SVG.
3. Dodajesz wpis do atlas manifest.
4. Klient ładuje ikonę po ID.
5. Potem podmieniasz placeholder na final premium sprite bez zmiany kodu.

## Minimalny zestaw premium do vertical slice
- 12 ikon incydentów,
- 8 ikon jednostek,
- 10 ikon statusów,
- 6 ramek alertów,
- 4 portrety ról,
- 1 mapa miasta dzień,
- 1 mapa miasta noc,
- 1 zestaw pogody deszcz,
- 1 zestaw blackout overlay.

## Quality gates
- placeholder i final używają tego samego ID,
- atlas ma wersję i ownera,
- ikona ma test miniatury,
- ikona ma stan disabled,
- brak finalnego assetu nie blokuje builda.

## Build-friendly rule
Jeżeli czegoś jeszcze nie ma:
- generuj SVG placeholder,
- nie blokuj integracji,
- dopisz wpis do backlogu art polish.
