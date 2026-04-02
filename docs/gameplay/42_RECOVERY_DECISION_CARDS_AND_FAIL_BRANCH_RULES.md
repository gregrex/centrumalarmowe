# Recovery Decision Cards and Fail Branch Rules

## Recovery rules
Karta pojawia się, gdy:
- objective przejdzie do at_risk,
- chain escalation osiągnie severity high lub critical,
- timer spadnie poniżej progu,
- city pressure > threshold.

## Fail branch rules
Misja wpada w fail branch, gdy:
- primary objective failed,
- cumulative city pressure critical przez zbyt długo,
- brak reakcji na recovery card,
- za dużo złych dispatch outcomes.

## Output
- clear fail reason,
- recommended retry angle,
- seed do następnego podejścia.
