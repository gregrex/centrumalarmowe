# Music Cue Sheet and Adaptive Rules

## Cue sheet
1. menu_calm_loop
2. menu_tension_loop
3. coop_lobby_loop
4. reward_reveal_sting
5. debrief_loop
6. escalation_preview_sting

## Adaptive rules
- jeśli gracz jest w menu i ma 0 aktywnych nagród -> calm
- jeśli pojawia się quick play streak / daily urgency -> tension
- jeśli lobby ma 2+ real players -> coop_lobby
- jeśli koniec rundy i success high -> debrief positive layer
- jeśli koniec rundy i fail -> debrief heavy layer

## Audio implementation rule
Music state switching ma mieć:
- fade out 200–500 ms,
- fade in 250–600 ms,
- optional pre-roll sting,
- brak nagłego cięcia.
