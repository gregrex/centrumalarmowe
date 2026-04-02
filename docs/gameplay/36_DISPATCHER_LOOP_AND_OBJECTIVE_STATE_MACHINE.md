# Dispatcher Loop And Objective State Machine

## Dispatcher loop
1. incident spawned,
2. player selects incident,
3. route + unit preview,
4. dispatch confirm,
5. outcome resolved,
6. objective delta applied,
7. city pressure updated,
8. new opportunity or escalation.

## Objective states
- locked,
- active,
- progress,
- at_risk,
- completed,
- failed.

## Trigger examples
- brak dispatchu przez 20 sekund -> at_risk,
- poprawny dispatch do critical medical -> progress +25,
- chain event nieobsłużony -> failed.
