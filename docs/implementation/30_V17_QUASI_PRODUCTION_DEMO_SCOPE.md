# V17 Quasi-Production Demo Scope

## Cel
Dowieźć jeden spójny demo-flow misji:
Home -> Chapter Map -> Mission Entry -> Briefing -> Ready Check -> Runtime -> Recovery Decisions -> Mission Complete or Fail -> Report Room -> Retry or Next.

## Zakres
- visual runtime route layer z czytelnymi segmentami, kierunkiem ruchu i warningami,
- recovery decision cards aktywowane przez chain escalation albo objective at risk,
- fail branches z osobnymi ekranami porażki, powodami i rekomendacjami,
- report room polish z reward reveal, retry CTA i next mission CTA,
- spójność audio i grafiki między menu, runtime i raportem.

## Krytyczne założenia
- Całość ma być content-driven.
- Warstwa 2D ma być łatwa do budowy przez agenta i prostą bibliotekę obiektów.
- Bot fill musi przejmować brakujących graczy bez psucia flow.
- Jeden pionowy slice ma wyglądać jak gotowy produkt, nawet jeśli nadal jest szkieletem.

## Done
- istnieją endpointy demo dla v17,
- istnieją kontrolery Unity do route layer, recovery cards, fail branches i report room polish,
- są JSON-y contentowe, prompt packi i słowniki,
- smoke-v17 przechodzi.
