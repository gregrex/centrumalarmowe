# Shared action audio i alert stacki

## Cele
- shared action musi mieć osobny dźwięk wezwania,
- ACK nie może zagłuszać krytycznego alarmu,
- dispatcher i operator mają różne priorytety audio.

## Minimalny stack
1. critical incident
2. shared action timeout
3. route blocked
4. dispatch accepted
5. generic info

## Dobre praktyki
- jeden krótki cue dla request,
- osobny cue dla resolve success,
- osobny cue dla resolve fail,
- brak długich melodyjek.
