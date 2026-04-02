# V12 — Chapter Runtime, Role Selection, Bot Fill Preview, Round Bootstrap

## Cel
Doprowadzić produkt do poziomu, w którym wejście do rundy jest czytelne, atrakcyjne wizualnie i gotowe do dalszej automatycznej implementacji.

## Zakres
1. Chapter Map runtime z aktywnymi stanami node'ów.
2. Mission Entry z rolami, lockami, rekomendacjami i podglądem bot fill.
3. Round Bootstrap z trybem offline, online, debug coop i bot team.
4. Final vertical slice pack dla grafiki 2D, obiektów, scen i audio.
5. Runtime dictionaries dla slotów ról, locków, objectives, gradingu i scen.

## Definition of done
- agent potrafi z Home wejść do Chapter Map,
- agent potrafi otworzyć Mission Entry i wybrać rolę,
- agent potrafi uruchomić Round Bootstrap online/offline,
- dostępne są dane do bot fill preview i objective grading,
- istnieją spójne prompty do grafiki, obiektów, scen, muzyki i barków.

## Uwaga produkcyjna
Rola wyboru postaci i bot fill ma być maksymalnie data-driven. Nie kodować list i stanów na sztywno.
