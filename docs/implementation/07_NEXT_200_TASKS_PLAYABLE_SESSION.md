
# Next 200 tasks — playable session build order

## Blok A — Home / Quick Play / Lobby
1. Dodać Home scene.
2. Dodać boot config dla Home.
3. Dodać Quick Play CTA.
4. Dodać Join Lobby CTA.
5. Dodać Settings CTA.
6. Dodać Credits CTA.
7. Dodać panel rotujących komunikatów dnia.
8. Dodać panel statusu połączenia.
9. Dodać loading overlay.
10. Dodać błąd połączenia i retry.
11. Dodać `QuickPlayBootstrapDto`.
12. Dodać endpoint bootstrap Quick Play.
13. Dodać start sesji Quick Play.
14. Dodać model lobby slotów.
15. Dodać auto-bot fill.
16. Dodać ready-check timer.
17. Dodać reconnect okno.
18. Dodać join code display.
19. Dodać scenę lobby 2D mobile.
20. Dodać test manualny: solo quick play.

## Blok B — Session state i incident loop
21. Dodać `SessionMetricDto`.
22. Dodać `SessionReportDto`.
23. Dodać `QuickPlayStartRequestDto`.
24. Dodać `VerticalSliceFactory`.
25. Dodać snapshot z 3 incydentami.
26. Dodać status `Queued` dla incydentu.
27. Dodać status `Validated`.
28. Dodać status `Assigned`.
29. Dodać status `Resolved`.
30. Dodać status `Failed`.
31. Dodać status `Escalated`.
32. Dodać prostą logikę eskalacji.
33. Dodać prostą logikę czasu.
34. Dodać prosty pressure score.
35. Dodać alerty HUD zależne od pressure.
36. Dodać alarmy dla krytycznych zgłoszeń.
37. Dodać liczenie czasu reakcji.
38. Dodać liczenie false dispatch.
39. Dodać liczenie missed incident.
40. Dodać test smoke dla raportu.

## Blok C — Role HUD
41. Doprecyzować HUD Operatora.
42. Doprecyzować HUD Dispatcher’a.
43. Doprecyzować HUD Coordinatora.
44. Doprecyzować HUD Crisis Officer’a.
45. Dodać wspólny panel alertów.
46. Dodać wspólny panel czasu rundy.
47. Dodać wspólny panel pressure bar.
48. Dodać listę aktywnych incydentów.
49. Dodać listę jednostek terenowych.
50. Dodać kartę szczegółów incydentu.
51. Dodać quick actions na 1 tap.
52. Dodać hold-to-confirm dla akcji krytycznych.
53. Dodać touch targets min 48dp.
54. Dodać tryb colorblind.
55. Dodać duży tekst dla alertów krytycznych.
56. Dodać portret roli i status człowieka/BOT-a.
57. Dodać badge reconnecting.
58. Dodać badge AI active.
59. Dodać badge overloaded.
60. Dodać manualny checklist testów HUD.

## Blok D — Dane słownikowe i content
61. Rozszerzyć słownik incydentów.
62. Rozszerzyć słownik statusów jednostek.
63. Rozszerzyć słownik typów jednostek.
64. Rozszerzyć słownik kategorii alertów.
65. Rozszerzyć słownik severity.
66. Rozszerzyć słownik pressure effects.
67. Rozszerzyć UI texts PL.
68. Dodać placeholder EN keys.
69. Dodać JSON schema dla mission pack.
70. Dodać CSV export mapping.
71. Dodać seed SQL dla rosteru jednostek.
72. Dodać seed SQL dla katalogu incydentów.
73. Dodać seed SQL dla efektów presji.
74. Dodać walidator missing ids.
75. Dodać walidator duplicate keys.
76. Dodać walidator invalid statuses.
77. Dodać walidator broken icon refs.
78. Dodać walidator broken prompt refs.
79. Dodać smoke script content + vertical slice.
80. Dodać raport walidacji do terminala.

## Blok E — BOT-y i takeover
81. Dodać role priorities per bot.
82. Dodać bot profile easy.
83. Dodać bot profile normal.
84. Dodać bot profile support.
85. Dodać bot handoff timer.
86. Dodać bot warm takeover.
87. Dodać bot full takeover.
88. Dodać restore control after reconnect.
89. Dodać action cooldown dla botów.
90. Dodać task queue dla botów.
91. Dodać action reason text.
92. Dodać blame-safe logs dla debug.
93. Dodać test disconnect operator.
94. Dodać test disconnect dispatcher.
95. Dodać test 3 bot slots.
96. Dodać test join in progress after start.
97. Dodać test rejoin to same role.
98. Dodać test AI fallback under pressure.
99. Dodać test AI no-free-unit handling.
100. Dodać manual QA matrix.

## Blok F — Grafika 2D premium i łatwa produkcja
101. Wybrać główny art target dla mobile 2D.
102. Zatwierdzić paletę alarmową.
103. Zatwierdzić styl ikon.
104. Zatwierdzić styl portretów postaci.
105. Zatwierdzić styl pojazdów.
106. Zatwierdzić styl mapy miasta.
107. Zatwierdzić styl minimapy.
108. Zatwierdzić styl kart zdarzeń.
109. Zatwierdzić styl paneli HUD.
110. Zatwierdzić styl VFX alertów.
111. Dodać prompt pack operator portraits.
112. Dodać prompt pack vehicle cards.
113. Dodać prompt pack city scenes.
114. Dodać prompt pack dispatch markers.
115. Dodać prompt pack notification icons.
116. Dodać pack SVG placeholderów.
117. Dodać atlas manifest v2.
118. Dodać naming rules assetów.
119. Dodać size rules sprite sheet.
120. Dodać compression rules mobile.

## Blok G — Audio i feedback
121. Dodać adaptive mix states.
122. Dodać niski alarm.
123. Dodać średni alarm.
124. Dodać wysoki alarm.
125. Dodać krytyczny alarm.
126. Dodać alert success.
127. Dodać dispatch confirm.
128. Dodać dispatch reject.
129. Dodać reconnect sound.
130. Dodać bot takeover cue.
131. Dodać ready-check cue.
132. Dodać session end cue.
133. Dodać report score cue.
134. Dodać music state calm.
135. Dodać music state rising.
136. Dodać music state overload.
137. Dodać music state crisis.
138. Dodać ducking rules.
139. Dodać mix priority table.
140. Dodać audio smoke checklist.

## Blok H — Raport i metagame
141. Dodać raport końca rundy.
142. Dodać 8 KPI do raportu.
143. Dodać grade S/A/B/C/D.
144. Dodać highlight best decision.
145. Dodać highlight biggest failure.
146. Dodać panel bot contribution.
147. Dodać panel human contribution.
148. Dodać panel pressure timeline.
149. Dodać retry same scenario.
150. Dodać next scenario CTA.
151. Dodać save summary snapshot.
152. Dodać export JSON summary.
153. Dodać scoreboard v1.
154. Dodać coop summary v1.
155. Dodać performance bucket names.
156. Dodać mission unlock flag.
157. Dodać end screen polish.
158. Dodać fail-state explanation.
159. Dodać recovery advice.
160. Dodać QA test cases.

## Blok I — Server / realtime / smoke
161. Dodać endpoint report.
162. Dodać endpoint quickplay bootstrap.
163. Dodać endpoint quickplay start.
164. Dodać session envelope for report ready.
165. Dodać session envelope for bot takeover.
166. Dodać session envelope for player rejoin.
167. Dodać health with version v5.
168. Dodać better demo session factory.
169. Dodać server-side metric calculator.
170. Dodać transient in-memory state store.
171. Dodać lobby state snapshot endpoint.
172. Dodać scenario metadata endpoint.
173. Dodać unit roster endpoint.
174. Dodać content version endpoint.
175. Dodać trace correlation id pass-through.
176. Dodać smoke-vslice.sh.
177. Dodać smoke-vslice.ps1.
178. Dodać docs how to run smoke.
179. Dodać docker notes for vertical slice.
180. Dodać minimal release checklist.

## Blok J — UX polish i prostota budowy
181. Ograniczyć liczbę kliknięć do startu.
182. Ujednolicić nazwy statusów.
183. Ujednolicić nazwy role ids.
184. Ujednolicić nazwy icon ids.
185. Ujednolicić nazwy scene ids.
186. Ujednolicić nazwy prompt packs.
187. Dodać plik z zasadami authoringu.
188. Dodać plik z zasadami commitów dla agenta.
189. Dodać plik z zasadami placeholder-first.
190. Dodać plik z zasadami fallback-first.
191. Dodać plik z zasadami mobile-first.
192. Dodać plik z zasadami art-quality gates.
193. Dodać plik z zasadami gameplay-quality gates.
194. Dodać plik z zasadami bot-quality gates.
195. Dodać plik z zasadami content-quality gates.
196. Dodać plik z zasadami smoke i verify.
197. Przejść cały vertical slice krok po kroku.
198. Zanotować luki w `DEMO_REPORT`.
199. Zaktualizować file index.
200. Spakować nową wersję paczki.
