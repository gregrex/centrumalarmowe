# Pierwsze 100 zadań kodowych

## Etap A — repo i kontrakty
1. Uporządkuj solution i referencje projektów.
2. Dodaj Directory.Build.props.
3. Dodaj analyzery i stylecop, jeśli zespół chce.
4. Dodaj Application project.
5. Dodaj Infrastructure project.
6. Dodaj wspólne enumy statusów.
7. Dodaj `SessionState`.
8. Dodaj `IncidentSeverity`.
9. Dodaj `DispatchUnitType`.
10. Dodaj `PlayerRoleSlot`.
11. Dodaj `BotTakeoverState`.
12. Dodaj `CityZoneType`.
13. Dodaj DTO snapshotu sesji.
14. Dodaj DTO incydentu.
15. Dodaj DTO jednostki.
16. Dodaj DTO alertu HUD.
17. Dodaj DTO roli gracza.
18. Dodaj DTO wyniku sesji.
19. Dodaj walidację requestów.
20. Dodaj testy serializacji kontraktów.

## Etap B — domena sesji
21. Dodaj agregat `GameSession`.
22. Dodaj encję `Incident`.
23. Dodaj encję `DispatchUnit`.
24. Dodaj encję `RoleSlot`.
25. Dodaj encję `SessionAlert`.
26. Dodaj prostą logikę priorytetu incydentu.
27. Dodaj logikę assignmentu jednostek.
28. Dodaj logikę kończenia incydentu.
29. Dodaj logikę eskalacji.
30. Dodaj logikę przeciążenia systemu.
31. Dodaj logikę przejęcia roli przez bota.
32. Dodaj logikę powrotu gracza.
33. Dodaj fabrykę scenariusza.
34. Dodaj eventy domenowe.
35. Dodaj KPI calculator.
36. Dodaj testy domeny.

## Etap C — backend
37. Dodaj API health.
38. Dodaj create session.
39. Dodaj join session.
40. Dodaj get snapshot.
41. Dodaj send action.
42. Dodaj end session.
43. Dodaj list scenarios.
44. Dodaj seed endpoint dev-only.
45. Dodaj SignalR hub.
46. Dodaj broadcast snapshot delta.
47. Dodaj heartbeat klienta.
48. Dodaj reconnect window.
49. Dodaj bot tick loop.
50. Dodaj director tick loop.

## Etap D — klient mobilny
51. Dodaj bootstrap klienta.
52. Dodaj ekran logowania testowego.
53. Dodaj ekran lobby.
54. Dodaj ekran wyboru roli.
55. Dodaj ekran operatora.
56. Dodaj ekran dyspozytora.
57. Dodaj ekran koordynatora.
58. Dodaj ekran oficera kryzysowego.
59. Dodaj panel mapy.
60. Dodaj feed alertów.
61. Dodaj panel incydentów.
62. Dodaj panel jednostek.
63. Dodaj panel AI slotów.
64. Dodaj websocket/signalr client.
65. Dodaj cache snapshotu.
66. Dodaj komendy gracza.
67. Dodaj lokalne animacje alertów.
68. Dodaj haptics pod alarmy.
69. Dodaj podstawowe audio.
70. Dodaj raport końca sesji.

## Etap E — admin i content
71. Dodaj stronę listy scenariuszy.
72. Dodaj stronę edycji scenariusza.
73. Dodaj stronę katalogu zdarzeń.
74. Dodaj katalog jednostek.
75. Dodaj edytor roli AI.
76. Dodaj eksport/import JSON.
77. Dodaj walidację contentu.
78. Dodaj audit log.
79. Dodaj feature flags.
80. Dodaj demo seed manager.

## Etap F — jakość
81. Dodaj integration tests.
82. Dodaj testy huba.
83. Dodaj smoke scripts.
84. Dodaj docker compose build.
85. Dodaj seed migration.
86. Dodaj trace logging.
87. Dodaj telemetry event batch.
88. Dodaj obsługę błędów UI.
89. Dodaj retry polityki.
90. Dodaj test reconnect.
91. Dodaj test AI takeover.
92. Dodaj test race condition join/leave.
93. Dodaj test końca sesji.
94. Dodaj test przeciążenia scenariusza.
95. Dodaj profil Demo.
96. Dodaj nagranie demo.
97. Dodaj screenshoty sklepu.
98. Dodaj backlog polish.
99. Dodaj checklistę release.
100. Zamknij vertical slice i zaktualizuj roadmapę.
