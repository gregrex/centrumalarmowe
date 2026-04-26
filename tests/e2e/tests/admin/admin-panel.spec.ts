import { test, expect } from '@playwright/test';

/**
 * E2E tests — AdminWeb panel
 * Covers: health page, dashboard (cards visible), links to API
 *
 * Auth credentials come from env vars to support CI and local dev.
 */

const ADMIN_USER = process.env.ADMIN_USERNAME ?? 'admin';
const ADMIN_PASS = process.env.ADMIN_PASSWORD ?? 'AdminPass_dev_only_1';

function basicAuth(user: string, pass: string): string {
  return 'Basic ' + Buffer.from(`${user}:${pass}`).toString('base64');
}

test.describe('AdminWeb health', () => {
  test('GET /health returns 200', async ({ request }) => {
    const response = await request.get('/health');
    expect(response.status()).toBe(200);
  });

  test('GET /health returns correct service name', async ({ request }) => {
    const response = await request.get('/health');
    const body = await response.json();
    expect(body).toHaveProperty('service', 'Alarm112.AdminWeb');
    expect(body).toHaveProperty('ok', true);
    expect(body).toHaveProperty('version', 'v26');
  });
});

test.describe('AdminWeb dashboard', () => {
  test('Landing page returns 200', async ({ page }) => {
    const response = await page.goto('/');
    expect(response?.status()).toBe(200);
  });

  test('Landing page exposes CTA to user dashboard', async ({ page }) => {
    await page.goto('/');
    await expect(page.getByText('Zobacz dashboard uzytkownika')).toBeVisible();
  });

  test('User dashboard returns 200', async ({ page }) => {
    const response = await page.goto('/app');
    expect(response?.status()).toBe(200);
  });

  test('User dashboard has player-facing hero copy', async ({ page }) => {
    await page.goto('/app');
    await expect(page.getByText('Od home huba do quickplay w jednym widoku.')).toBeVisible();
  });

  test.describe('Protected admin dashboard', () => {
    test.beforeEach(async ({ page }) => {
      await page.setExtraHTTPHeaders({ Authorization: basicAuth(ADMIN_USER, ADMIN_PASS) });
    });

    test('GET /admin returns 200', async ({ page }) => {
      const response = await page.goto('/admin');
      expect(response?.status()).toBe(200);
    });

    test('Dashboard has page title', async ({ page }) => {
      await page.goto('/admin');
      await expect(page).toHaveTitle(/Alarm112/i);
    });

    test('Dashboard has API Status card', async ({ page }) => {
      await page.goto('/admin');
      await expect(page.getByText('Status API')).toBeVisible();
    });

    test('Dashboard has Content Validation card', async ({ page }) => {
      await page.goto('/admin');
      await expect(page.getByText('Walidacja contentu')).toBeVisible();
    });

    test('Dashboard has Session Demo card', async ({ page }) => {
      await page.goto('/admin');
      await expect(page.getByText('Sesje demo')).toBeVisible();
    });

    test('Dashboard has Reference Data card', async ({ page }) => {
      await page.goto('/admin');
      await expect(page.getByText('Reference data')).toBeVisible();
    });

    test('Dashboard has City Map card', async ({ page }) => {
      await page.goto('/admin');
      await expect(page.getByText('City Map')).toBeVisible();
    });

    test('Dashboard links are present', async ({ page }) => {
      await page.goto('/admin');
      const links = page.locator('a.btn');
      const count = await links.count();
      expect(count).toBeGreaterThan(3);
    });
  });
});
