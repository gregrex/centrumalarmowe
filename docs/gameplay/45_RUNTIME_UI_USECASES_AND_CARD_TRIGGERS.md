# Runtime UI Use Cases and Card Triggers

## Use cases
1. Jednostka EMS spóźnia się -> pokaż recovery card.
2. Zablokowana trasa straży -> pokaż reroute card.
3. City pressure rośnie za szybko -> pokaż overload card.
4. Objective at risk -> pokaż reinforcement card.
5. Radio partial loss -> pokaż communication fallback card.

## Trigger formula
severity + time pressure + objective risk + resource scarcity >= threshold

## Output
- HUD banner,
- recovery card,
- optional bot recommendation,
- impact preview.
