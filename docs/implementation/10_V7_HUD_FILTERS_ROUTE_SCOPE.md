# V7 scope: HUD, filtry, route preview i shared actions

## Cel wersji
Zbudować pierwszy naprawdę wygodny do grania mobilny loop dla dwóch kluczowych ról:
- operator,
- dispatcher.

Wersja v7 ma zmniejszyć chaos interfejsu i pokazać, że gra może być:
- szybka,
- czytelna,
- sieciowa,
- bot-assisted,
- łatwa do dalszej rozbudowy przez dane.

## Co musi powstać
1. Active incident board z priorytetami, timerami i tagami.
2. Pasek filtrów mapy i listy incydentów.
3. Route preview dla dispatchera.
4. Shared action panel dla sytuacji wymagających potwierdzeń coop.
5. Role-focused HUD dla operatora i dispatchera.
6. Role bot profiles v2.
7. API szkicujące powyższe moduły.
8. Dane słownikowe dla filtrów, statusów, tagów i akcji współdzielonych.

## Zakres MVP v7
- tylko 4 filtry mapy,
- tylko 6 aktywnych incydentów jednocześnie,
- tylko 3 rodzaje shared actions,
- tylko 3 typy route warningów,
- tylko 2 rozbudowane HUD-y: operator i dispatcher.

## Definition of Done v7
- `tools/content-verify.sh` przechodzi,
- `tools/smoke-v7.sh` przechodzi,
- nowe JSON-y są poprawne,
- API ma endpointy v7,
- klient Unity ma starter controlery v7,
- dokumentacja art/UI/gameplay/backend/testy jest spójna.
