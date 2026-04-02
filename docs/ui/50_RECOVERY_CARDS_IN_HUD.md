# Recovery Cards in HUD

## Założenie
Recovery cards przestają być osobnym, rzadkim ekranem. W v18 są integralną częścią runtime HUD.

## Trigger categories
- route.blocked
- unit.delayed
- city.overload
- radio.partial_loss
- objective.at_risk
- chain.escalating

## Anatomy karty
- tytuł,
- severity chip,
- krótki opis,
- 2–3 opcje,
- koszt,
- przewidywany wpływ,
- rekomendacja bota,
- ikona roli dominującej.

## Zasady
- tylko 1 aktywna karta krytyczna na raz,
- karta może zostać odłożona na kilka sekund,
- część kart wygasa,
- decyzja może zmienić objective states i city pressure.

## Potrzebne warianty
- compact,
- expanded,
- critical overlay,
- role-focused.
