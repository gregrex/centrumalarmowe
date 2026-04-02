# V11 Campaign Entry + Bot Fill Scope

## Cel wersji
V11 scala dotychczasowe elementy produktu w jeden bardziej realny przepływ:
**Home -> wybór rozdziału kampanii / Quick Play / Coop -> rezerwacja ról -> bot fill brakujących ról -> wejście do rundy online lub offline**.

## Dlaczego ten etap jest ważny
Do tej pory projekt miał:
- Home hub,
- mapę miasta,
- lobby,
- bot takeover,
- kampanię i daily challenges,
- menu art/audio.

Brakowało jednak **jednego, spójnego entry flow**, które:
1. działa dla solo,
2. działa dla coop,
3. działa gdy graczy jest 1-4,
4. przejmuje puste role botami,
5. prowadzi użytkownika do rundy bez chaosu UX.

## Zakres v11
- animowany renderer menu z presetami dzień/noc/deszcz/burza,
- chapter map kampanii z węzłami, blokadami i nagrodami,
- mission entry sheet z podsumowaniem celu, czasu i ryzyk,
- online/offline route:
  - offline solo + 3 boty,
  - local debug split,
  - online coop + bot fill,
- rola reservation state,
- role handoff rules,
- profile cosmetics:
  - portrety,
  - ramki,
  - nameplate,
  - badge,
  - voice style tag,
- audio transitions:
  - home,
  - chapter map,
  - mission entry,
  - lobby,
  - round start,
- nowe dane słownikowe i content-driven identyfikatory.

## Kryteria ukończenia
- istnieją pliki dokumentacji, danych i kodu dla entry flow,
- smoke test v11 przechodzi,
- content verify obejmuje nowe JSON-y,
- API ma endpointy dla campaign chapters, mission entry, cosmetics i home-to-round audio,
- klient Unity ma szkielety controllerów dla chapter map, mission entry, identity i weather animation.

## Poza zakresem
- finalny renderer runtime z prawdziwymi sprite atlasami,
- gotowy system monetyzacji kosmetyk,
- pełna synchronizacja sieciowa serwer->klient dla live lobby state.
