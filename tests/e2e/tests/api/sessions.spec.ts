import { test, expect } from '@playwright/test';

/**
 * E2E API tests — Session lifecycle
 * Covers: POST /api/sessions/demo, GET /api/sessions/{id}, POST /api/sessions/{id}/actions
 */

test.describe('Session endpoints', () => {
  test('POST /api/sessions/demo creates a session and returns 200', async ({ request }) => {
    const response = await request.post('/api/sessions/demo');
    expect(response.status()).toBe(200);
  });

  test('POST /api/sessions/demo returns sessionId', async ({ request }) => {
    const response = await request.post('/api/sessions/demo');
    const body = await response.json();
    expect(body).toHaveProperty('sessionId');
    expect(typeof body.sessionId).toBe('string');
    expect(body.sessionId.length).toBeGreaterThan(0);
  });

  test('GET /api/sessions/{id} returns snapshot', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const createdBody = await created.json();
    const sessionId = createdBody.sessionId;

    const response = await request.get(`/api/sessions/${sessionId}`);
    expect(response.status()).toBe(200);
    const body = await response.json();
    expect(body).toHaveProperty('sessionId', sessionId);
  });

  test('GET /api/sessions/{id} snapshot has incidents and units arrays', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const createdBody = await created.json();
    const sessionId = createdBody.sessionId;

    const response = await request.get(`/api/sessions/${sessionId}`);
    const body = await response.json();
    expect(Array.isArray(body.incidents)).toBe(true);
    expect(Array.isArray(body.units)).toBe(true);
  });

  test('POST /api/sessions/{id}/actions returns 200 for dispatch action', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const createdBody = await created.json();
    const sessionId = createdBody.sessionId;

    const action = {
      sessionId,
      actorId: 'player-1',
      role: 'Dispatcher',
      actionType: 'dispatch',
      payloadJson: JSON.stringify({ incidentId: 'inc-1', unitId: 'unit-1' }),
      correlationId: crypto.randomUUID().replace(/-/g, ''),
    };

    const response = await request.post(`/api/sessions/${sessionId}/actions`, { data: action });
    expect(response.status()).toBe(200);
  });

  test('POST /api/sessions/{id}/actions returns success flag', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const createdBody = await created.json();
    const sessionId = createdBody.sessionId;

    const action = {
      sessionId,
      actorId: 'player-1',
      role: 'Dispatcher',
      actionType: 'dispatch',
      payloadJson: JSON.stringify({ incidentId: 'inc-1', unitId: 'unit-1' }),
      correlationId: crypto.randomUUID().replace(/-/g, ''),
    };

    const response = await request.post(`/api/sessions/${sessionId}/actions`, { data: action });
    const body = await response.json();
    expect(body).toHaveProperty('success', true);
  });

  test('Session snapshot has state field', async ({ request }) => {
    const created = await request.post('/api/sessions/demo');
    const createdBody = await created.json();
    const sessionId = createdBody.sessionId;

    const getResp = await request.get(`/api/sessions/${sessionId}`);
    const snap = await getResp.json();
    expect(snap).toHaveProperty('state');
  });
});
