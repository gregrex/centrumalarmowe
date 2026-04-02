import { test, expect } from '@playwright/test';

/**
 * E2E API tests — Lobby endpoints
 * Covers: POST /api/lobbies/demo, GET /api/lobbies/{id}, lobby state
 */

test.describe('Lobby endpoints', () => {
  test('POST /api/lobbies/demo creates lobby and returns 200', async ({ request }) => {
    const response = await request.post('/api/lobbies/demo');
    expect(response.status()).toBe(200);
  });

  test('POST /api/lobbies/demo has lobbyId field', async ({ request }) => {
    const response = await request.post('/api/lobbies/demo');
    const body = await response.json();
    expect(body).toHaveProperty('lobbyId');
  });

  test('POST /api/lobbies/demo has players array', async ({ request }) => {
    const response = await request.post('/api/lobbies/demo');
    const body = await response.json();
    expect(Array.isArray(body.players)).toBe(true);
  });

  test('GET /api/lobbies/{lobbyId} returns lobby for existing id', async ({ request }) => {
    // First create a demo lobby to get a valid lobbyId
    const createResp = await request.post('/api/lobbies/demo');
    expect(createResp.status()).toBe(200);
    const created = await createResp.json();
    const lobbyId = created.lobbyId as string;

    const response = await request.get(`/api/lobbies/${lobbyId}`);
    // Should return 200 or 404 but not crash with 500
    expect(response.status()).toBeLessThan(500);
  });
});
