# One Mission Slice Music Routing V4

## Stany muzyczne
- home_calm
- chapter_tension
- briefing_focus
- runtime_low
- runtime_mid
- runtime_peak
- recovery_focus
- fail_state
- retry_prep
- report_success
- report_partial

## Routing
Home -> Briefing -> Runtime -> Recovery/Fail/Resolve -> Report -> Retry/Next

## Reguły miksu
- timer threshold +1 intensywność,
- objective completed -1 intensywność,
- city pressure > 80 przełącza peak stem,
- failstate wycina część rytmu.
