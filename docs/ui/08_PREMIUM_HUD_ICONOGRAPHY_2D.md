# Premium HUD iconography 2D

## Cel
Zbudować zestaw ikon i sygnałów, który wygląda premium, ale jest tani w produkcji i łatwy do skalowania.

## Filary
- silhouette first,
- 3 stany czytelności: normal / warning / critical,
- jeden wspólny język kształtów,
- mało detalu wewnętrznego,
- świetna czytelność w 48, 64 i 96 px.

## Rodziny ikon
### Incydenty
- medyczne,
- pożarowe,
- policyjne,
- infrastrukturalne,
- masowe.

### Jednostki
- ambulans podstawowy,
- ambulans specjalistyczny,
- straż gaśnicza,
- policja,
- patrol mieszany,
- support technical,
- command vehicle.

### Stany
- przyjęto zgłoszenie,
- oczekuje na dispatch,
- w drodze,
- na miejscu,
- eskalacja,
- częściowo opanowane,
- zamknięte.

### Alerty
- przeciążenie miasta,
- brak jednostek,
- reconnect,
- bot takeover,
- blackout,
- traffic delay,
- critical stack.

## Zasady rysunku
- grubszy obrys zewnętrzny,
- wewnętrzna bryła prosta,
- jeden główny punkt światła,
- brak tekstu w ikonie,
- brak cienkich losowych kresek,
- shape memory ważniejsze niż detal.

## Kolory
- czerwony = krytyczny,
- bursztyn = warning,
- zielony = stabilny,
- niebieski = medyczny/operacyjny neutral,
- fiolet tylko dla warstwy meta lub specjalnej.

## Produkcyjny skrót
Najpierw:
1. czarno-białe SVG,
2. warianty kolorów z configu,
3. prosty glow jako oddzielna warstwa,
4. dopiero potem wersja premium z drobnym polish.
