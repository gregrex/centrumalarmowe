# Next 250 Tasks — City Map, Dispatch, Timeline, Premium 2D

## Phase A — Data foundations
1. Define city map schema.
2. Define zone ids.
3. Define node types.
4. Define marker categories.
5. Define unit availability states.
6. Define dispatch action ids.
7. Define outcome codes.
8. Define timeline event codes.
9. Define end-report timeline sections.
10. Define map scale guidance.
11. Add schema examples.
12. Add validation examples.
13. Add fallback examples.
14. Add localization keys.
15. Add icon ids.
16. Add unit color rules.
17. Add zone severity overlay rules.
18. Add route display rules.
19. Add report grade rules.
20. Add bot takeover state ids.
21. Add reconnect state ids.
22. Add human reclaim state ids.
23. Add default scenario ids.
24. Add sample city presets.
25. Add sample night/day variants.
26. Add map clutter rules.
27. Add mobile touch rules.
28. Add tiny-screen fallback rules.
29. Add portrait-mode layout rules.
30. Add quickplay map preset.
31. Add tutorial map preset.
32. Add overload map preset.
33. Add emergency event tags.
34. Add weighted dispatch rules.
35. Add invalid dispatch examples.
36. Add delayed dispatch examples.
37. Add duplicate dispatch examples.
38. Add escalation trigger examples.
39. Add incident close reasons.
40. Add incident fail reasons.
41. Add route blocked status.
42. Add weather modifiers.
43. Add traffic modifiers.
44. Add infrastructure modifiers.
45. Add zone population pressure factor.
46. Add call density factor.
47. Add critical cluster definition.
48. Add safe zone definition.
49. Add staging area definition.
50. Review all ids for consistency.

## Phase B — API and contracts
51. Add city map DTO.
52. Add city node DTO.
53. Add city connection DTO.
54. Add dispatch command DTO.
55. Add dispatch result DTO.
56. Add session timeline item DTO.
57. Add timeline list DTO if needed.
58. Add dispatch preview DTO if needed.
59. Add city map service interface.
60. Add city map service implementation.
61. Add map endpoint.
62. Add timeline endpoint.
63. Add dispatch endpoint.
64. Add fake data generator.
65. Add version string.
66. Add logging for dispatch.
67. Add invalid unit handling.
68. Add invalid incident handling.
69. Add session not found behavior.
70. Add trace id in result.
71. Add route summary in result.
72. Add pressure delta in result.
73. Add timeline insertion after dispatch.
74. Add bot note when AI assisted.
75. Add role ownership note.
76. Add reconnect-safe payload notes.
77. Add hub event naming note.
78. Add envelope example.
79. Add API examples to docs.
80. Add smoke examples to docs.
81. Add fast path for local in-memory use.
82. Add test notes.
83. Add future persistence notes.
84. Add content-driven mapping notes.
85. Add warning code catalog.
86. Add error code catalog.
87. Add preview UI notes.
88. Add report UI notes.
89. Add dispatch cooldown notes.
90. Add concurrency note.
91. Add optimistic update note.
92. Add fallback refresh note.
93. Add delta synchronization note.
94. Add route duration placeholder.
95. Add ETA placeholder.
96. Add mission pressure placeholder.
97. Add KPI delta placeholder.
98. Add end-of-round summary placeholder.
99. Add timeline severity note.
100. Freeze V6 API names.

## Phase C — Unity starter
101. Add city map controller.
102. Add city marker view model.
103. Add live unit list controller.
104. Add incident card controller.
105. Add dispatch request builder.
106. Add session timeline controller.
107. Add report timeline controller.
108. Add quickplay start flow controller.
109. Add API client methods.
110. Add mock JSON loading path.
111. Add live data loading path.
112. Add controller logging.
113. Add portrait layout anchors.
114. Add compact panel rules.
115. Add bottom-sheet incident card rule.
116. Add role accent usage.
117. Add marker color usage.
118. Add touch target validation notes.
119. Add list virtualization note.
120. Add simple sort by ETA.
121. Add simple sort by severity.
122. Add unit state badges.
123. Add incident state badges.
124. Add map pan note.
125. Add pinch zoom note.
126. Add single-tap marker note.
127. Add long-press alternative note.
128. Add safe offline fallback.
129. Add API unavailable state.
130. Add report fallback state.
131. Add skeleton loader state.
132. Add empty state.
133. Add warning state.
134. Add no-units state.
135. Add route blocked state.
136. Add bot dispatch state.
137. Add handoff note.
138. Add reclaim note.
139. Add local debug menu note.
140. Add scene prefab plan.
141. Add city map scene plan.
142. Add quickplay scene plan.
143. Add report scene plan.
144. Add controller dependency notes.
145. Add content path notes.
146. Add readme notes.
147. Add mobile perf notes.
148. Add atlas notes.
149. Add icon fallback notes.
150. Freeze V6 Unity starter list.

## Phase D — Art, UX, audio
151. Define premium 2D city map style.
152. Define road and district layers.
153. Define marker silhouettes.
154. Define unit cards style.
155. Define incident card style.
156. Define bot badge style.
157. Define route line style.
158. Define pressure pulse effect.
159. Define alert shimmer effect.
160. Define zone heat overlay style.
161. Define portrait panel style.
162. Define map tile palette.
163. Define night palette.
164. Define rain palette.
165. Define high-pressure palette.
166. Define audio priority per role.
167. Define incident ping tiers.
168. Define dispatch confirm sound.
169. Define route blocked sound.
170. Define escalation sound.
171. Define timeline insert sound.
172. Define report reveal sound.
173. Define low-clutter typography.
174. Define icon readability rule.
175. Define contrast rule.
176. Define accessibility color fallback.
177. Define tiny device safe layout.
178. Define low-spec fx profile.
179. Define placeholder asset naming.
180. Define atlas grouping.
181. Define VFX naming.
182. Define prompt batch naming.
183. Define export folder rules.
184. Define no-text-in-art rule.
185. Define no-brand rule.
186. Define no-photo-real gore rule.
187. Define no-over-detail rule.
188. Define polish priorities.
189. Define hero screens.
190. Define store screenshot candidates.
191. Define UI promo shots.
192. Define QA checklist for art.
193. Define QA checklist for touch.
194. Define QA checklist for map readability.
195. Define QA checklist for incident scan speed.
196. Define QA checklist for bot state visibility.
197. Define QA checklist for report clarity.
198. Define final prompt packs.
199. Define scene assembly order.
200. Freeze V6 art and UX rules.

## Phase E — Validation and build
201. Extend content verify for new JSON packs.
202. Add city map file checks.
203. Add incident action file checks.
204. Add timeline file checks.
205. Add map schema file checks.
206. Add smoke-v6 script.
207. Add smoke-v6 PowerShell script.
208. Add README V6 notes.
209. Add file index updates.
210. Add easy local boot notes.
211. Add no-Docker fast path notes.
212. Add art placeholder fast path notes.
213. Add acceptance checklist.
214. Add playtest checklist.
215. Add bot fallback checklist.
216. Add reconnect checklist.
217. Add invalid dispatch checklist.
218. Add report accuracy checklist.
219. Add log capture note.
220. Add known limitations section.
221. Add handoff instructions for Copilot.
222. Add handoff instructions for Claude.
223. Add agent priority order.
224. Add small-commit rule reminder.
225. Add build-first rule reminder.
226. Add content validation gate reminder.
227. Add art gate reminder.
228. Add UI readability gate reminder.
229. Add mobile perf gate reminder.
230. Add API contract freeze reminder.
231. Add route simulation TODO list.
232. Add persistence TODO list.
233. Add analytics TODO list.
234. Add localization TODO list.
235. Add web admin TODO list.
236. Add matchmaking TODO list.
237. Add bots learning TODO list.
238. Add campaign TODO list.
239. Add monetization TODO list.
240. Add liveops TODO list.
241. Add final review task.
242. Add archive task.
243. Add zip package task.
244. Add smoke rerun task.
245. Add docs rerun task.
246. Add file index rerun task.
247. Add sample gif capture TODO.
248. Add sample video capture TODO.
249. Add final vertical slice signoff.
250. Hand off to next iteration.
