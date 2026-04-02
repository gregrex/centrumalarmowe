# Faza 07 — Serwer sesji, real-time i reconnect

301. Wybierz docelowy mechanizm real-time: SignalR lub WebSocket gateway
302. Zaprojektuj hub/kanały komunikacji per sesja
303. Zaimplementuj dołączanie klienta do kanału sesji
304. Dodaj autoryzację połączeń real-time
305. Zaimplementuj broadcast aktualizacji stanu sesji
306. Dodaj synchronizację aktywnych alertów
307. Zaimplementuj publikowanie zmian zdarzeń
308. Dodaj publikowanie zmian jednostek
309. Zaimplementuj obsługę partial state update
310. Dodaj pełny snapshot po reconnect
311. Zaimplementuj keepalive i heartbeat kanału
312. Dodaj wykrywanie opóźnionego klienta
313. Zaimplementuj flagę stale data po stronie klienta
314. Dodaj obsługę konfliktów akcji wysłanych prawie równocześnie
315. Zaimplementuj kolejkę zdarzeń serwerowych per sesja
316. Dodaj rozdzielenie eventów krytycznych i kosmetycznych
317. Zaimplementuj deterministic ordering dla akcji krytycznych
318. Dodaj recovery stanu po restarcie procesu serwera
319. Zaimplementuj persist snapshot co interwał
320. Dodaj zasady garbage collectingu starych sesji
321. Zaimplementuj obsługę reconnect window
322. Dodaj procedurę bot takeover przy utracie połączenia
323. Zaimplementuj վերադարձ sterowania po reconnect
324. Dodaj mechanizm informacji „co zrobił bot podczas nieobecności”
325. Zaimplementuj stan degraded network i reakcje UI
326. Dodaj pomiar RTT i sieciowych metryk klienta
327. Zaimplementuj logikę ponawiania połączenia po stronie klienta
328. Dodaj lokalne buforowanie niezatwierdzonych akcji
329. Zaimplementuj serwerowe odrzucanie zbyt starych poleceń
330. Dodaj retry polityki dla bezpiecznych akcji
331. Zaimplementuj stan read-only przy skrajnym desync
332. Dodaj kontrolę wersji klienta przy połączeniu
333. Zaimplementuj sygnały obecności człowieka i bota
334. Dodaj licznik occupancy sesji
335. Zaimplementuj prosty matchmaking private code
336. Dodaj zaproszenia lobby i readiness
337. Zaimplementuj start sesji dopiero po poprawnej synchronizacji wszystkich członków
338. Dodaj obsługę anulowania sesji i graceful shutdown
339. Zaimplementuj testy reconnect dla każdej roli
340. Dodaj testy konfliktu komend między graczem a botem
341. Zaimplementuj symulator opóźnień sieci do QA
342. Dodaj testy soak dla dłuższej sesji
343. Zaimplementuj logi techniczne dla kanału real-time
344. Dodaj alarmy serwerowe na częste disconnecty
345. Zaimplementuj dashboard podstawowych metryk sesji
346. Dodaj tryb debug sesji dla developerów
347. Zaimplementuj dokumentację sekwencji połączenia
348. Zweryfikuj stabilność sesji 4-osobowej + 1 bot
349. Utwórz RUN_DOCKER dla warstwy sesyjnej
350. Zamknij etap działającym reconnect i takeover flow
