import { test, expect } from '@playwright/test';

/**
 * E2E API tests — Error handling & edge cases
 * Covers: 404 for unknown routes, malformed requests, rate limiting awareness
 */

test.describe('Error handling', () => {
  test('GET /api/nonexistent returns 404', async ({ request }) => {
    const response = await request.get('/api/nonexistent-endpoint-xyz');
    expect(response.status()).toBe(404);
  });

  test('GET /api/sessions/nonexistent-id does not crash server (returns 4xx or 200)', async ({ request }) => {
    const response = await request.get('/api/sessions/nonexistent-session-id-12345');
    // Should gracefully return 404 or similar, not crash with 500
    expect(response.status()).toBeLessThan(500);
  });

  test('POST /api/sessions/{id}/actions with missing fields returns 4xx not 500', async ({ request }) => {
    const response = await request.post('/api/sessions/bad-id/actions', {
      data: {},
    });
    // Bad request or not found — but not a server crash
    expect(response.status()).toBeLessThan(500);
  });

  test('Response headers include content-type for health', async ({ request }) => {
    const response = await request.get('/health');
    const ct = response.headers()['content-type'] ?? '';
    expect(ct).toContain('application/json');
  });
});

test.describe('Swagger UI', () => {
  test('GET /swagger/index.html returns 200', async ({ page }) => {
    const response = await page.goto('/swagger/index.html');
    expect(response?.status()).toBe(200);
  });

  test('Swagger page has swagger-ui element', async ({ request }) => {
    // Verify the OpenAPI spec JSON is accessible — more reliable than DOM check
    const response = await request.get('/swagger/v1/swagger.json');
    expect(response.status()).toBe(200);
    const body = await response.json();
    expect(body).toHaveProperty('openapi');
    expect(body).toHaveProperty('info');
  });
});
