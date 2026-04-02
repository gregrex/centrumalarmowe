import { defineConfig, devices } from '@playwright/test';
import * as path from 'path';

/**
 * Playwright configuration for Alarm112 E2E tests.
 * Targets the local API (default: http://localhost:5080) and AdminWeb (http://localhost:5081).
 * Artefacts (videos, traces, screenshots) are saved to /artifacts/e2e/.
 */

const API_BASE = process.env.API_BASE ?? 'http://localhost:5080';
const ADMIN_BASE = process.env.ADMIN_BASE ?? 'http://localhost:5081';

export default defineConfig({
  testDir: './tests',
  outputDir: '../../artifacts/e2e/results',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 2 : undefined,
  reporter: [
    ['html', { outputFolder: '../../artifacts/e2e/report', open: 'never' }],
    ['list'],
    ['json', { outputFile: '../../artifacts/e2e/results.json' }],
  ],
  use: {
    baseURL: API_BASE,
    trace: 'retain-on-failure',
    video: 'retain-on-failure',
    screenshot: 'only-on-failure',
  },
  projects: [
    {
      name: 'api-chromium',
      use: { ...devices['Desktop Chrome'], baseURL: API_BASE },
      testMatch: 'tests/api/**/*.spec.ts',
    },
    {
      name: 'admin-chromium',
      use: { ...devices['Desktop Chrome'], baseURL: ADMIN_BASE },
      testMatch: 'tests/admin/**/*.spec.ts',
    },
  ],
});
