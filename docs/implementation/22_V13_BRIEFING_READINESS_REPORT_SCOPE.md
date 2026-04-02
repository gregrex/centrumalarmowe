# V13 — Mission Briefing, Team Readiness, Post-Round Report

## Cel
Doprowadzić produkt do pierwszego naprawdę spójnego vertical slice flow:
Home -> Chapter Map -> Mission Entry -> Mission Briefing -> Team Readiness -> Round Bootstrap -> Round -> Post-Round Report -> Rewards.

## Zakres
1. Ekran briefingowy misji z celami, zagrożeniami, mapą sytuacji i sugerowanymi rolami.
2. Team Readiness z role cards, ready check, bot fill, offline/online state i jakościowym UX mobilnym.
3. Pierwszy raport po rundzie: grading, timeline, rewards, mistakes, star system.
4. Rozszerzony pack obiektów, scen i elementów tła pod briefing, report i mission completion.
5. Audio dla briefingów, ready check, nagród, grade reveal i mission complete.
6. Słowniki runtime, rewards, report widgets, performance tags i dialogue cues.

## Definition of done
- agent potrafi przejść z Home do Mission Briefing bez chaosu nawigacyjnego,
- agent potrafi uruchomić Team Readiness i zasymulować ready check 2–4 slotów,
- agent potrafi wygenerować raport końca misji z oceną, gwiazdkami, nagrodami i błędami,
- istnieje spójny pack danych i promptów dla grafiki 2D, obiektów, scen i audio,
- smoke v13 weryfikuje nowe pliki i endpointy.

## Priorytet produkcyjny
Najpierw czytelność flow i szybkość budowy. Wszystko ma być data-driven, z minimalną liczbą twardo zakodowanych reguł.
