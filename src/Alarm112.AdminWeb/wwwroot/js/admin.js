/* admin.js — Alarm112 AdminWeb runtime
 * All user-visible strings use textContent (never innerHTML) to prevent XSS.
 */

(function () {
  'use strict';

  const dot        = document.getElementById('api-dot');
  const statusText = document.getElementById('api-status-text');
  const metricStatus = document.getElementById('metric-api-status');
  const metricSub    = document.getElementById('metric-api-sub');
  const metricSessions = document.getElementById('metric-sessions');
  const metricSessionsSub = document.getElementById('metric-sessions-sub');
  const metricBundle = document.getElementById('metric-bundle');
  const metricBundleSub = document.getElementById('metric-bundle-sub');
  const pageSubtitle = document.getElementById('page-subtitle');
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

  function setApiOffline(message) {
    dot.className = 'status-dot err';
    statusText.textContent = 'API offline';
    metricStatus.textContent = 'ERR';
    metricStatus.style.color = 'var(--err)';
    metricSub.textContent = String(message);
  }

  function setSessions(status, totalCount, summary, error) {
    if (status === 'ok') {
      metricSessions.textContent = String(totalCount ?? 0);
      metricSessions.style.color = 'var(--role-op)';
      metricSessionsSub.textContent = String(summary ?? 'Aktywne sesje z API.');
      return;
    }

    metricSessions.textContent = '—';
    metricSessions.style.color = 'var(--warn)';
    metricSessionsSub.textContent = String(error ?? summary ?? 'Brak danych o sesjach.');
  }

  function setContent(status, issueCount, summary, error) {
    if (status === 'valid') {
      metricBundle.textContent = 'OK';
      metricBundle.style.color = 'var(--ok)';
      metricBundleSub.textContent = String(summary ?? 'Brak problemow walidacji.');
      return;
    }

    if (status === 'invalid') {
      metricBundle.textContent = String(issueCount ?? 0);
      metricBundle.style.color = 'var(--warn)';
      metricBundleSub.textContent = String(summary ?? 'Wykryto problemy walidacji.');
      return;
    }

    metricBundle.textContent = '—';
    metricBundle.style.color = 'var(--warn)';
    metricBundleSub.textContent = String(error ?? summary ?? 'Brak danych walidacji.');
  }

  async function refreshDashboard() {
    try {
      const r = await fetch('/api/admin/dashboard', { signal: AbortSignal.timeout(5000) });
      const j = await r.json();
      if (r.ok) {
        dot.className = j.api?.status === 'online' ? 'status-dot' : 'status-dot err';
        statusText.textContent = j.api?.status === 'online' ? 'API online' : 'API offline';
        metricStatus.textContent = j.api?.status === 'online' ? 'OK' : 'ERR';
        metricStatus.style.color = j.api?.status === 'online' ? 'var(--ok)' : 'var(--err)';
        metricSub.textContent = String(j.api?.service ?? j.api?.error ?? 'Alarm112.Api');

        const subtitleParts = ['Centrum Alarmowe v26'];
        if (j.api?.store) subtitleParts.push(String(j.api.store));
        if (j.api?.version) subtitleParts.push('API ' + String(j.api.version));
        pageSubtitle.textContent = subtitleParts.join(' · ');

        setSessions(j.sessions?.status, j.sessions?.totalCount, j.sessions?.summary, j.sessions?.error);
        setContent(j.content?.status, j.content?.issueCount, j.content?.summary, j.content?.error);

        addLog('ok', 'HEALTH', String(j.api?.service ?? 'Alarm112.Api') + ' · ' + String(j.api?.version ?? ''));
        addLog(
          j.sessions?.status === 'ok' ? 'info' : 'warn',
          'SESSIONS',
          String(j.sessions?.summary ?? j.sessions?.error ?? 'Brak danych o sesjach.')
        );
        addLog(
          j.content?.status === 'valid' ? 'ok' : (j.content?.status === 'invalid' ? 'warn' : 'err'),
          'CONTENT',
          String(j.content?.summary ?? j.content?.error ?? 'Brak danych walidacji.')
        );
        return;
      }

      setApiOffline('blad odpowiedzi');
      setSessions('unavailable', null, null, 'Dashboard zwrocil HTTP ' + String(r.status));
      setContent('unavailable', null, null, 'Dashboard zwrocil HTTP ' + String(r.status));
      addLog('err', 'DASH', 'HTTP ' + String(r.status));
    } catch (e) {
      setApiOffline('brak połączenia');
      setSessions('unavailable', null, null, e.message);
      setContent('unavailable', null, null, e.message);
      addLog('err', 'DASH', 'Brak odpowiedzi: ' + String(e.message));
    }
  }

  refreshDashboard();
  setInterval(refreshDashboard, 30000);

})();
