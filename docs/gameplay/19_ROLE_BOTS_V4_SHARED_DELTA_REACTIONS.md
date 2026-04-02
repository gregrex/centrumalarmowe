# Boty v4: reakcja na delty i shared actions

## Nowość v8
Bot nie reaguje już tylko na brak gracza. Reaguje też na:
- escalation,
- timeout risk,
- brak ACK,
- za długi cooldown,
- konflikt priorytetów.

## Operator bot
- priorytetyzuje zgłoszenia,
- zamyka oczywiste low priority,
- pinguję dispatchera przy escalation.

## Dispatcher bot
- reaguje na ETA i cooldown,
- unika wysłania jednostki z najgorszym ETA jeśli istnieje sensowna alternatywa,
- potrafi zasugerować ghost dispatch.

## Coordinator bot
- potwierdza shared action przy krytycznych węzłach,
- przebudowuje priorytety po eskalacji.

## Crisis officer bot
- reaguje na global overload,
- podnosi poziom alertu,
- aktywuje wsparcie systemowe.

## Ważna zasada
Bot ma pomagać, nie grać za człowieka bez potrzeby.
