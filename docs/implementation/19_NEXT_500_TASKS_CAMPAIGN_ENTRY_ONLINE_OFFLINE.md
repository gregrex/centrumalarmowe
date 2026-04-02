# Next 500 Tasks - Campaign Entry / Online / Offline

## Cel
Ta lista rozbija budowę wersji v11 na praktyczne zadania dla agenta kodującego.

## Blok 1 - UX wejścia
1. Zdefiniuj ekrany Home, Chapter Map, Mission Entry, Lobby Entry.
2. Dodaj wspólny breadcrumbs model.
3. Ustal back navigation dla online/offline.
4. Zdefiniuj enter/exit transition tokens.
5. Dodaj loading skeleton dla chapter map.
6. Dodaj loading skeleton dla mission entry.
7. Dodaj panel "missing roles".
8. Dodaj panel "bot fill summary".
9. Dodaj panel "recommended role".
10. Dodaj panel "risk tags".
11. Dodaj panel "mission rewards".
12. Dodaj panel "network mode".
13. Dodaj CTA "Play Solo".
14. Dodaj CTA "Play With Bots".
15. Dodaj CTA "Create Coop Lobby".
16. Dodaj CTA "Quick Fill Missing Roles".
17. Dodaj CTA "Preferred Role".
18. Dodaj CTA "Auto Assign".
19. Dodaj CTA "Return To Home".
20. Dodaj CTA "Start Round".
21. Dodaj accessibility labels.
22. Dodaj haptics map dla CTA.
23. Dodaj colorblind token mapping.
24. Dodaj left-hand layout notes.
25. Dodaj reduced motion notes.

## Blok 2 - Campaign model
26. Zdefiniuj chapter ids.
27. Zdefiniuj mission ids.
28. Zdefiniuj unlock conditions.
29. Zdefiniuj fail retry rules.
30. Zdefiniuj reward categories.
31. Zdefiniuj chapter themes.
32. Zdefiniuj node visual states.
33. Zdefiniuj node audio stingers.
34. Zdefiniuj chapter completion summary.
35. Zdefiniuj campaign difficulty ramps.
36. Zdefiniuj chapter weather variants.
37. Zdefiniuj chapter day/night presets.
38. Zdefiniuj story snippets.
39. Zdefiniuj dispatcher hint packs.
40. Zdefiniuj operator hint packs.
41. Zdefiniuj coordinator hint packs.
42. Zdefiniuj crisis officer hint packs.
43. Zdefiniuj mission brief tags.
44. Zdefiniuj mission briefing cards.
45. Zdefiniuj mission length presets.
46. Zdefiniuj mission start unit packs.
47. Zdefiniuj mission incident pools.
48. Zdefiniuj mission route difficulty.
49. Zdefiniuj mission score targets.
50. Zdefiniuj mission medals.

## Blok 3 - Bot fill
51. Zdefiniuj role reservation states.
52. Zdefiniuj preferred role priority.
53. Zdefiniuj bot assignment order.
54. Zdefiniuj bot persona per role.
55. Zdefiniuj bot caution/aggression ranges.
56. Zdefiniuj bot ack timeout.
57. Zdefiniuj bot rejoin handoff.
58. Zdefiniuj bot resume after disconnect.
59. Zdefiniuj bot takeover log event.
60. Zdefiniuj bot release log event.
61. Zdefiniuj bot confidence state.
62. Zdefiniuj bot latency tolerance.
63. Zdefiniuj offline solo policy.
64. Zdefiniuj 2-player fill policy.
65. Zdefiniuj 3-player fill policy.
66. Zdefiniuj 4-player no-fill policy.
67. Zdefiniuj bot limitation tags.
68. Zdefiniuj bot explanation strings.
69. Zdefiniuj bot iconography.
70. Zdefiniuj bot HUD badges.
71. Zdefiniuj bot audio cues.
72. Zdefiniuj bot failure safeguards.
73. Zdefiniuj bot lock-out on critical flow.
74. Zdefiniuj bot assist-only mode.
75. Zdefiniuj bot full takeover mode.

## Blok 4 - Audio / art / polish
76. Dodaj menu weather animator.
77. Dodaj chapter map parallax pack.
78. Dodaj mission entry card art pack.
79. Dodaj portrait cosmetics prompt pack.
80. Dodaj nameplate prompt pack.
81. Dodaj badge prompt pack.
82. Dodaj chapter map icon set.
83. Dodaj reward chest icon set.
84. Dodaj route preview entry stinger.
85. Dodaj bot fill confirmation sound.
86. Dodaj role reserved sound.
87. Dodaj role released sound.
88. Dodaj mission locked sound.
89. Dodaj chapter complete flourish.
90. Dodaj reduced motion animation profile.
91. Dodaj day->night tween rules.
92. Dodaj storm overlay rules.
93. Dodaj chapter node pulse rules.
94. Dodaj mission sheet open/close rules.
95. Dodaj UI card shadow levels.
96. Dodaj chapter background blur tokens.
97. Dodaj premium mobile readability review.
98. Dodaj screenshot checklist.
99. Dodaj store capsule chapter art checklist.
100. Dodaj home-to-round polish gate.

## Blok 5 - API / client / build
101. Dodaj CampaignChapterDto.
102. Dodaj CampaignMissionEntryDto.
103. Dodaj ProfileCosmeticDto.
104. Dodaj PlayerIdentityDto.
105. Dodaj HomeToRoundAudioDto.
106. Dodaj ICampaignEntryService.
107. Dodaj CampaignEntryService.
108. Dodaj endpoint /api/campaign-chapters/demo.
109. Dodaj endpoint /api/mission-entry/demo.
110. Dodaj endpoint /api/profile-cosmetics/demo.
111. Dodaj endpoint /api/player-identity/demo.
112. Dodaj endpoint /api/home-to-round-audio.
113. Dodaj CampaignChapterMapController.
114. Dodaj MissionEntryFlowController.
115. Dodaj PlayerIdentityPanelController.
116. Dodaj AnimatedMenuWeatherController.
117. Dodaj HomeToRoundAudioTransitionController.
118. Dodaj smoke-v11.sh.
119. Dodaj smoke-v11.ps1.
120. Rozszerz content-verify.
121. Zaktualizuj README.
122. Zaktualizuj FILE_INDEX.

## Blok 6 - Acceptance
123. Sprawdź czy solo offline ma bot fill.
124. Sprawdź czy coop online ma missing role panel.
125. Sprawdź czy mission entry pokazuje rewards.
126. Sprawdź czy chapter nodes mają states.
127. Sprawdź czy cosmetics mają preview assets.
128. Sprawdź czy audio routes mają mission entry.
129. Sprawdź czy weather config jest poprawny JSON.
130. Sprawdź czy chapter map config jest poprawny JSON.
131. Sprawdź czy mission entry flow config jest poprawny JSON.
132. Sprawdź czy cosmetics config jest poprawny JSON.
133. Sprawdź czy portrait pack jest poprawny JSON.
134. Sprawdź czy smoke-v11 przechodzi.
135. Dodaj końcowy wpis do changelogu.
136. Zweryfikuj łatwość dalszej budowy.
137. Zweryfikuj spójność nazw.
138. Zweryfikuj spójność ids.
139. Zweryfikuj brak duplikatów.
140. Zweryfikuj gotowość pod v12.
