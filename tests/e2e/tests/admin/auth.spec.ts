import { test, expect } from '@playwright/test';

/**
 * E2E security tests — AdminWeb Basic Authentication
 *
 * Verifies that the Basic Auth middleware correctly:
 *  - allows public /health endpoint without credentials
 *  - rejects unauthenticated requests with 401 + WWW-Authenticate
 *  - rejects wrong credentials with 401
 *  - rejects malformed Base64 with 400
 *  - accepts valid credentials
 *
 * Credentials come from env vars (matching docker-compose / CI):
 *   ADMIN_USERNAME  (default: admin)
 *   ADMIN_PASSWORD  (default: AdminPass_dev_only_1)
 */

const ADMIN_USER = process.env.ADMIN_USERNAME ?? 'admin';
const ADMIN_PASS = process.env.ADMIN_PASSWORD ?? 'AdminPass_dev_only_1';

function basicAuth(user: string, pass: string): string {
  return 'Basic ' + Buffer.from(`${user}:${pass}`).toString('base64');
}

test.describe('AdminWeb Basic Auth — public endpoints', () => {
  test('GET /health is accessible without credentials', async ({ request }) => {
    const res = await request.get('/health');
    expect(res.status()).toBe(200);
  });

  test('GET /health returns correct shape without credentials', async ({ request }) => {
    const res = await request.get('/health');
    const body = await res.json();
    expect(body.ok).toBe(true);
    expect(body.service).toBe('Alarm112.AdminWeb');
  });
});

test.describe('AdminWeb Basic Auth — unauthenticated rejection', () => {
  test('GET / without Authorization returns 401', async ({ request }) => {
    const res = await request.get('/');
    expect(res.status()).toBe(401);
  });

  test('GET / without auth returns WWW-Authenticate header', async ({ request }) => {
    const res = await request.get('/');
    const wwwAuth = res.headers()['www-authenticate'];
    expect(wwwAuth).toBeTruthy();
    expect(wwwAuth).toContain('Basic');
    expect(wwwAuth).toContain('112 Admin Panel');
  });

  test('GET / with wrong username returns 401', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: basicAuth('wronguser', ADMIN_PASS) },
    });
    expect(res.status()).toBe(401);
  });

  test('GET / with wrong password returns 401', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: basicAuth(ADMIN_USER, 'wrongpassword') },
    });
    expect(res.status()).toBe(401);
  });

  test('GET / with empty password returns 401', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: basicAuth(ADMIN_USER, '') },
    });
    expect(res.status()).toBe(401);
  });

  test('GET / with Bearer token (wrong scheme) returns 401', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: 'Bearer some-jwt-token' },
    });
    expect(res.status()).toBe(401);
  });
});

test.describe('AdminWeb Basic Auth — malformed input', () => {
  test('GET / with malformed Base64 returns 400', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: 'Basic !!!not-valid-base64!!!' },
    });
    expect(res.status()).toBe(400);
  });
});

test.describe('AdminWeb Basic Auth — authenticated access', () => {
  test('GET / with valid credentials returns 200', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: basicAuth(ADMIN_USER, ADMIN_PASS) },
    });
    expect(res.status()).toBe(200);
  });

  test('GET / with valid credentials returns HTML dashboard', async ({ request }) => {
    const res = await request.get('/', {
      headers: { Authorization: basicAuth(ADMIN_USER, ADMIN_PASS) },
    });
    const body = await res.text();
    expect(body).toContain('Alarm112');
    expect(body).toContain('Panel Admina');
  });

  test('Dashboard page renders with valid auth', async ({ page }) => {
    // Set Basic Auth header via extraHTTPHeaders so every request is authenticated
    await page.setExtraHTTPHeaders({ Authorization: basicAuth(ADMIN_USER, ADMIN_PASS) });
    const response = await page.goto('/');
    expect(response?.status()).toBe(200);
    await expect(page).toHaveTitle(/Alarm112/i);
  });

  test('Dashboard shows Status API card when authenticated', async ({ page }) => {
    await page.setExtraHTTPHeaders({ Authorization: basicAuth(ADMIN_USER, ADMIN_PASS) });
    await page.goto('/');
    await expect(page.getByText('Status API')).toBeVisible();
  });
});
