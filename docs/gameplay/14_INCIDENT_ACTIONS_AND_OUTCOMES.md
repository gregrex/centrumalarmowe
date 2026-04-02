# Incident Actions and Outcomes

## First action set for V6
- dispatch ambulance,
- dispatch police,
- dispatch fire truck,
- dispatch support unit,
- request backup,
- mark suspected false alarm,
- hold and monitor,
- escalate to coordinator,
- escalate to crisis officer.

## Outcome categories
- stabilized,
- improving,
- unresolved,
- deteriorating,
- critical,
- closed,
- false alarm confirmed,
- failed due to delay.

## Pressure impact examples
- correct first response: -2 pressure,
- delayed response: +4 pressure,
- wrong service first: +5 pressure,
- backup requested early: -1 pressure,
- incident escalates: +6 pressure,
- false alarm closed: -1 pressure.

## Bot role support
When a human role is absent:
- operator bot classifies simple calls,
- dispatcher bot can auto-send a recommended unit,
- coordinator bot can mark escalation priority,
- crisis bot can trigger reserve support.

## Important design rule
Bot action must be helpful but not perfect.
A human player should still outperform the fallback AI.
