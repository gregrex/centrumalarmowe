# Następne 350 zadań po v8

## Faza A — overlay i runtime jednostek
1. Dodać route overlay renderer oparty na segmentach.
2. Dodać style: normal, warning, critical.
3. Dodać heat intensity dla segmentów.
4. Dodać ikony blokad na trasie.
5. Dodać wygaszanie overlay po dispatch.
6. Dodać API units runtime.
7. Dodać cooldown badge na karcie jednostki.
8. Dodać ETA badge na karcie jednostki.
9. Dodać listę runtime sorting by ETA.
10. Dodać listę runtime sorting by readiness.
11. Dodać listę runtime sorting by distance.
12. Dodać filtr per unit type.
13. Dodać filtr per district.
14. Dodać filtr per availability.
15. Dodać payload diagnostyczny dla smoke.
16. Dodać preview zajętości po dispatch.
17. Dodać delty zmian dla jednostek.
18. Dodać panel route warnings.
19. Dodać panel traffic reasons.
20. Dodać test danych dla unit cooldown rules.

## Faza B — live deltas i flow rundy
21. Dodać delta feed per incident.
22. Dodać delta severity escalation.
23. Dodać delta resolved.
24. Dodać delta shared-action-needed.
25. Dodać delta resource-shortage.
26. Dodać delta timeout-risk.
27. Dodać delta audio cue mapping.
28. Dodać delta retention 30 sekund.
29. Dodać kompaktowy log delt.
30. Dodać grupowanie podobnych delt.
31. Dodać odczyt round phase.
32. Dodać tick counter.
33. Dodać elapsed timer.
34. Dodać round objective state.
35. Dodać partial score preview.
36. Dodać fail state preview.
37. Dodać bot reaction cooldown.
38. Dodać human rejoin handoff.
39. Dodać replay seed id.
40. Dodać playtest telemetry list.

## Faza C — split panels i coop
41. Dodać split panel operator+dispatcher.
42. Dodać split panel coordinator+crisis.
43. Dodać tryb debug one-device-two-roles.
44. Dodać panel header per role.
45. Dodać unread counter per role.
46. Dodać role focus indicator.
47. Dodać role lock badge.
48. Dodać ack badge.
49. Dodać mini timeline per role.
50. Dodać touch priority map dla split mode.
51. Dodać safe zones dla kciuka.
52. Dodać collapsed panel state.
53. Dodać swap focus action.
54. Dodać split panel audio priority.
55. Dodać split panel alert collapse.
56. Dodać quick ping do drugiej roli.
57. Dodać one-tap accept shared action.
58. Dodać one-tap delegate to bot.
59. Dodać test lokalnego coop.
60. Dodać acceptance checklist split mode.

## Faza D — boty v4
61. Dodać bot reaction profile operator.
62. Dodać bot reaction profile dispatcher.
63. Dodać bot reaction profile coordinator.
64. Dodać bot reaction profile crisis officer.
65. Dodać delta-driven decision making.
66. Dodać priorytety odpowiedzi na escalation.
67. Dodać priorytety odpowiedzi na timeout risk.
68. Dodać bot wait window before takeover.
69. Dodać human override.
70. Dodać bot confidence flag.
71. Dodać log decyzji bota.
72. Dodać cooldown na kolejne takeovery.
73. Dodać shared action helper.
74. Dodać fallback przy disconnect.
75. Dodać fallback przy AFK.
76. Dodać fallback przy reconnect.
77. Dodać test działań konfliktowych bot vs człowiek.
78. Dodać tuning JSON.
79. Dodać prostą telemetrię skuteczności botów.
80. Dodać acceptance checklist bot v4.

## Faza E — content i słowniki
81. Rozszerzyć słowniki o round phases.
82. Rozszerzyć słowniki o delta types.
83. Rozszerzyć słowniki o unit cooldown reasons.
84. Rozszerzyć słowniki o eta bands.
85. Rozszerzyć słowniki o overlay styles.
86. Rozszerzyć słowniki o panel layout keys.
87. Rozszerzyć słowniki o audio tension states.
88. Rozszerzyć słowniki o heatmap presets.
89. Dodać scenariusze rund v8.
90. Dodać balans ETA vs severity.
91. Dodać balans bot takeover timings.
92. Dodać balans shared action windows.
93. Dodać balans route warnings.
94. Dodać lokalizacje PL.
95. Dodać lokalizacje EN placeholder.
96. Dodać walidację JSON schema.
97. Dodać import pack dla admina.
98. Dodać export pack dla playtest.
99. Dodać diff report konfiguracji.
100. Dodać changelog v8 content.

## Dalsze 250 kroków
Pozostałe 250 kroków agent ma wygenerować i rozpisać w repo jako:
- konkretne taski per moduł,
- kryteria wejścia/wyjścia,
- pliki do utworzenia,
- smoke i acceptance gates.
