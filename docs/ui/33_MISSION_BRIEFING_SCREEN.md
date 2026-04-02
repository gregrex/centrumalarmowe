# Mission Briefing Screen

## Cel
Dać graczowi czytelne, szybkie i atrakcyjne wizualnie wejście do misji przed Team Readiness.

## Sekcje ekranu
1. Header: mission title, chapter badge, difficulty.
2. Hero panel: główny incydent, weather, time of day.
3. Objectives panel: primary + secondary.
4. Risk panel: tags, bottlenecks, missing units.
5. Map mini preview: hotspoty i strefy ryzyka.
6. Recommended team: role chips + unit chips.
7. Speaker rail: dowódca / operator / radio note.
8. Footer CTAs: back, readiness.

## Stany
- loading,
- story_reveal,
- repeat_attempt,
- blocked_by_unlock,
- offline_supported,
- online_session_required.

## Edge cases
- mission bez secondary objectives,
- mission bez speaker portrait,
- mission z blackout theme,
- mission z wymuszonym bot fill.

## Mobile UX
- pionowy layout,
- duże CTA,
- max 2 tapy do wejścia do readiness,
- ważne ryzyka ponad foldem.
