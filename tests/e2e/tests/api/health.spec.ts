import { test, expect, APIRequestContext } from '@playwright/test';

/**
 * E2E API tests — Health endpoint
 * Covers: /health — status, shape, version
 */

test.describe('Health endpoint', () => {
  test('GET /health returns 200', async ({ request }) => {
    const response = await request.get('/health');
    expect(response.status()).toBe(200);
  });

  test('GET /health returns correct JSON shape', async ({ request }) => {
    const response = await request.get('/health');
    const body = await response.json();
    expect(body).toHaveProperty('ok', true);
    expect(body).toHaveProperty('service');
    expect(body).toHaveProperty('version');
  });

  test('GET /health returns service=Alarm112.Api', async ({ request }) => {
    const response = await request.get('/health');
    const body = await response.json();
    expect(body.service).toBe('Alarm112.Api');
  });

  test('GET /health returns version v26', async ({ request }) => {
    const response = await request.get('/health');
    const body = await response.json();
    expect(body.version).toBe('v26');
  });
});
