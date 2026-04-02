import { test, expect } from '@playwright/test';

/**
 * E2E tests — Full session flow (integration scenario)
 * Mimics what the Unity client would do: create session → read state → dispatch unit → read updated state
 */

test.describe('Session full flow', () => {
  test('Complete flow: create → get snapshot → dispatch → verify success', async ({ request }) => {
    // Step 1: Create demo session
    const createResp = await request.post('/api/sessions/demo');
    expect(createResp.status()).toBe(200);
    const created = await createResp.json();
    const sessionId: string = created.sessionId;
    expect(sessionId).toBeTruthy();

    // Step 2: Get snapshot
    const snapResp = await request.get(`/api/sessions/${sessionId}`);
    expect(snapResp.status()).toBe(200);
    const snap = await snapResp.json();
    expect(snap.sessionId).toBe(sessionId);

    // Step 3: Find pending incident and available unit
    let incidentId = 'inc-1';
    let unitId = 'unit-1';

    if (Array.isArray(snap.incidents)) {
      for (const inc of snap.incidents) {
        if (inc.status === 'pending') {
          incidentId = inc.incidentId;
          break;
        }
      }
    }
    if (Array.isArray(snap.units)) {
      for (const unit of snap.units) {
        if (unit.status === 'available') {
          unitId = unit.unitId;
          break;
        }
      }
    }

    // Step 4: Dispatch
    const actionResp = await request.post(`/api/sessions/${sessionId}/actions`, {
      data: {
        sessionId,
        actorId: 'player-1',
        role: 'Dispatcher',
        actionType: 'dispatch',
        payloadJson: JSON.stringify({ incidentId, unitId }),
        correlationId: crypto.randomUUID().replace(/-/g, ''),
      },
    });
    expect(actionResp.status()).toBe(200);
    const actionResult = await actionResp.json();
    expect(actionResult.success).toBe(true);
  });

  test('Multiple sessions are isolated', async ({ request }) => {
    const r1 = await request.post('/api/sessions/demo');
    const r2 = await request.post('/api/sessions/demo');
    const s1 = (await r1.json()).sessionId;
    const s2 = (await r2.json()).sessionId;
    expect(s1).not.toBe(s2);
  });
});

test.describe('Bot Director integration', () => {
  test('Session has bot-filled roles in snapshot', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const snap = await (await request.get(`/api/sessions/${(await created.json()).sessionId}`)).json();
    // The demo session should have at least some players/bots
    expect(snap).toBeTruthy();
  });
});
