import { test, expect } from '@playwright/test';

/**
 * E2E API tests — Validation errors and security edge cases
 * Covers: 400 on bad input, 404 on unknown IDs, never 500 on bad input
 */

test.describe('Validation error responses', () => {
  test('POST /api/sessions/{id}/actions with empty body returns 400', async ({ request }) => {
    const response = await request.post('/api/sessions/test-session/actions', {
      data: {},
    });
    expect(response.status()).toBe(400);
  });

  test('POST /api/sessions/{id}/actions with invalid role returns 400', async ({ request }) => {
    const response = await request.post('/api/sessions/test-session/actions', {
      data: {
        sessionId: 'test-session',
        actorId: 'player-1',
        role: 'INVALID_ROLE_XYZ',
        actionType: 'dispatch',
        correlationId: 'corr-001',
      },
    });
    expect(response.status()).toBe(400);
  });

  test('POST /api/sessions/{id}/actions with invalid actionType returns 400', async ({ request }) => {
    const response = await request.post('/api/sessions/test-session/actions', {
      data: {
        sessionId: 'test-session',
        actorId: 'player-1',
        role: 'Dispatcher',
        actionType: 'DELETE_ALL',
        correlationId: 'corr-002',
      },
    });
    expect(response.status()).toBe(400);
  });

  test('POST /api/sessions/{id}/actions with oversized payloadJson returns 400', async ({ request }) => {
    const bigPayload = 'x'.repeat(2000);
    const response = await request.post('/api/sessions/test-session/actions', {
      data: {
        sessionId: 'test-session',
        actorId: 'player-1',
        role: 'Dispatcher',
        actionType: 'dispatch',
        payloadJson: bigPayload,
        correlationId: 'corr-003',
      },
    });
    expect(response.status()).toBe(400);
  });

  test('GET /api/sessions/nonexistent returns 404', async ({ request }) => {
    const response = await request.get('/api/sessions/this-session-does-not-exist-xyz-999');
    expect(response.status()).toBe(404);
  });

  test('400 responses include JSON body (not empty)', async ({ request }) => {
    const response = await request.post('/api/sessions/test/actions', {
      data: {},
    });
    expect(response.status()).toBe(400);
    const body = await response.json();
    expect(body).not.toBeNull();
  });

  test('Injected SQL in sessionId path does not return 500', async ({ request }) => {
    const response = await request.get("/api/sessions/'; DROP TABLE sessions; --");
    expect(response.status()).toBeLessThan(500);
  });

  test('XSS in path does not return 500', async ({ request }) => {
    const response = await request.get('/api/sessions/%3Cscript%3Ealert(1)%3C%2Fscript%3E');
    expect(response.status()).toBeLessThan(500);
  });
});

test.describe('Security headers on all responses', () => {
  test('GET /health has X-Frame-Options: DENY', async ({ request }) => {
    const response = await request.get('/health');
    const header = response.headers()['x-frame-options'];
    expect(header).toBe('DENY');
  });

  test('GET /health has X-Content-Type-Options: nosniff', async ({ request }) => {
    const response = await request.get('/health');
    const header = response.headers()['x-content-type-options'];
    expect(header).toBe('nosniff');
  });

  test('GET /health has Referrer-Policy', async ({ request }) => {
    const response = await request.get('/health');
    expect(response.headers()['referrer-policy']).toBeTruthy();
  });

  test('GET /api/reference-data has security headers', async ({ request }) => {
    const response = await request.get('/api/reference-data');
    expect(response.headers()['x-frame-options']).toBeTruthy();
    expect(response.headers()['x-content-type-options']).toBe('nosniff');
  });
});

test.describe('Audit logging does not break responses', () => {
  test('POST /api/sessions/demo still returns 200 (audit does not break mutations)', async ({ request }) => {
    const response = await request.post('/api/sessions/demo');
    expect(response.status()).toBe(200);
  });

  test('GET /health still returns 200 (audit transparent for GET)', async ({ request }) => {
    const response = await request.get('/health');
    expect(response.status()).toBe(200);
  });
});
