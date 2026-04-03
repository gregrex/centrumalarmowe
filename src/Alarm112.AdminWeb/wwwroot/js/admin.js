/* admin.js — Alarm112 AdminWeb runtime
 * API_BASE is injected by the server as a global variable in the HTML template.
 * All user-visible strings use textContent (never innerHTML) to prevent XSS.
 */

(function () {
  'use strict';

  const dot        = document.getElementById('api-dot');
  const statusText = document.getElementById('api-status-text');
  const metricStatus = document.getElementById('metric-api-status');
  const metricSub    = document.getElementById('metric-api-sub');
  const metricBundle = document.getElementById('metric-bundle');
  const logBody      = document.getElementById('log-body');

  const tsBootEl = document.getElementById('ts-boot');
  if (tsBootEl) tsBootEl.textContent = new Date().toLocaleTimeString('pl-PL');

  /**
   * Appends a log row safely (no innerHTML — XSS-safe).
   * @param {'ok'|'warn'|'err'|'info'} level
   * @param {string} tag
   * @param {string} msg
   */
  function addLog(level, tag, msg) {
    const row = document.createElement('div');
    row.className = 'log-row';

    const timeSpan = document.createElement('span');
    timeSpan.className = 'log-time';
    timeSpan.textContent = new Date().toLocaleTimeString('pl-PL');

    const tagSpan = document.createElement('span');
    tagSpan.className = 'log-tag ' + level;
    tagSpan.textContent = '[' + tag + ']';

    const msgSpan = document.createElement('span');
    msgSpan.className = 'log-msg';
    msgSpan.textContent = String(msg);

    row.appendChild(timeSpan);
    row.appendChild(tagSpan);
    row.appendChild(msgSpan);
    logBody.appendChild(row);
    logBody.scrollTop = logBody.scrollHeight;

    // Keep log to last 200 entries
    while (logBody.childElementCount > 200) {
      logBody.removeChild(logBody.firstChild);
    }
  }

  async function checkApi() {
    try {
      const r = await fetch(window.API_BASE + '/health', { signal: AbortSignal.timeout(4000) });
      const j = await r.json();
      dot.className = 'status-dot';
      statusText.textContent = 'API online';
      metricStatus.textContent = 'OK';
      metricStatus.style.color = 'var(--ok)';
      // Use String() to ensure we're always setting textContent safely
      metricSub.textContent = String(j.version ?? 'v26');
      addLog('ok', 'HEALTH', 'OK · ' + String(j.service ?? 'Alarm112.Api') + ' · ' + String(j.version ?? ''));
    } catch (e) {
      dot.className = 'status-dot err';
      statusText.textContent = 'API offline';
      metricStatus.textContent = 'ERR';
      metricStatus.style.color = 'var(--err)';
      metricSub.textContent = 'brak połączenia';
      addLog('err', 'HEALTH', 'Brak odpowiedzi: ' + String(e.message));
    }
  }

  async function checkBundle() {
    try {
      const r = await fetch(window.API_BASE + '/api/reference-data', { signal: AbortSignal.timeout(5000) });
      if (r.ok) {
        metricBundle.textContent = 'OK';
        metricBundle.style.color = 'var(--ok)';
        addLog('ok', 'BUNDLE', 'reference-data v26 dostępny');
      } else {
        metricBundle.textContent = String(r.status);
        metricBundle.style.color = 'var(--warn)';
        addLog('warn', 'BUNDLE', 'HTTP ' + String(r.status));
      }
    } catch (e) {
      metricBundle.textContent = '—';
      addLog('warn', 'BUNDLE', 'Niedostępny: ' + String(e.message));
    }
  }

  checkApi();
  checkBundle();
  setInterval(checkApi, 30000);

})();
