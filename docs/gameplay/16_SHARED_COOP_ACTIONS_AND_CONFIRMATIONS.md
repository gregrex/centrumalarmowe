# Shared coop actions i potwierdzenia

## Dlaczego
Coop ma dawać realną współpracę, a nie 4 osoby klikające te same przyciski.

## Typy shared actions
- `shared.escalate.major-incident`
- `shared.reallocate.cross-district`
- `shared.lock.high-priority-lane`

## Reguły
- inicjator nie powinien sam zamykać akcji wymagającej drugiej roli,
- bot może potwierdzić tylko jeśli rola jest pusta lub offline,
- timeout shared action nie może być długi,
- wynik shared action musi być czytelny i natychmiast widoczny w timeline.

## Bot assist
Jeżeli gracz zniknie:
- bot przejmuje tylko zakres swojej roli,
- nie generuje decyzji przekraczających poziom ryzyka określony w profilu,
- zapisuje reason code do timeline.
