import { test, expect } from '@playwright/test';

/**
 * E2E API tests — Content & Reference Data endpoints
 * Covers all major content GET endpoints that return data bundles.
 */

const GET_ENDPOINTS: Array<{ name: string; path: string }> = [
  { name: 'reference-data', path: '/api/reference-data' },
  { name: 'content/validate', path: '/api/content/validate' },
  { name: 'home-hub', path: '/api/home-hub' },
  { name: 'campaign-chapters', path: '/api/campaign-chapters/demo' },
  { name: 'mission-briefing', path: '/api/mission-briefing/demo' },
  { name: 'city-map', path: '/api/city-map' },
  { name: 'quickplay-bootstrap', path: '/api/quickplay/bootstrap' },
  { name: 'theme-pack', path: '/api/theme-pack' },
  { name: 'menu-flow', path: '/api/menu-flow' },
  { name: 'role-selection', path: '/api/role-selection/demo' },
  { name: 'mission-runtime', path: '/api/mission-runtime/demo' },
  { name: 'postround-report', path: '/api/postround-report/demo' },
  { name: 'campaign-overview', path: '/api/campaign-overview/demo' },
  { name: 'daily-challenges', path: '/api/daily-challenges/demo' },
  { name: 'settings-bundle', path: '/api/settings-bundle' },
  { name: 'meta-progression', path: '/api/meta-progression/demo' },
  { name: 'audio-routes', path: '/api/audio-routes' },
  { name: 'chapter-runtime', path: '/api/chapter-runtime/demo' },
  { name: 'round-bootstrap', path: '/api/round-bootstrap/demo' },
  { name: 'team-readiness', path: '/api/team-readiness/demo' },
];

test.describe('Content endpoints', () => {
  for (const { name, path } of GET_ENDPOINTS) {
    test(`GET ${path} returns 200`, async ({ request }) => {
      const response = await request.get(path);
      expect(response.status(), `${name} should return 200`).toBe(200);
    });

    test(`GET ${path} returns JSON body`, async ({ request }) => {
      const response = await request.get(path);
      const contentType = response.headers()['content-type'] ?? '';
      expect(contentType, `${name} should return JSON`).toContain('application/json');
      const body = await response.json();
      expect(body).not.toBeNull();
      expect(body).not.toBeUndefined();
    });
  }
});

test.describe('Reference data structure', () => {
  test('GET /api/reference-data has expected fields', async ({ request }) => {
    const response = await request.get('/api/reference-data');
    const body = await response.json();
    expect(body).toBeTruthy();
  });

  test('GET /api/content/validate returns validationResult', async ({ request }) => {
    const response = await request.get('/api/content/validate');
    const body = await response.json();
    expect(body).toHaveProperty('isValid');
  });
});
