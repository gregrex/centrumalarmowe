(function () {
  'use strict';

  const byId = (id) => document.getElementById(id);
  const setText = (id, value) => {
    const el = byId(id);
    if (el) el.textContent = value;
  };

  const renderList = (id, items, selector) => {
    const el = byId(id);
    if (!el) return;
    el.innerHTML = '';
    items.forEach((item) => {
      const li = document.createElement('li');
      li.textContent = selector(item);
      el.appendChild(li);
    });
  };

  fetch('/api/user/dashboard')
    .then((response) => response.json())
    .then((data) => {
      setText('user-api-status', data.api?.status === 'online' ? 'online' : 'offline');
      setText('user-api-sub', [data.api?.service, data.api?.version, data.api?.store].filter(Boolean).join(' · ') || 'Brak danych API');

      setText('user-card-count', String(data.home?.cards?.length ?? 0));
      setText('user-home-sub', data.home?.continueSummary || data.home?.defaultScreen || 'Home hub gotowy');
      setText('user-chapter-count', String(data.chapters?.chapterCount ?? 0));
      setText('user-chapter-sub', ((data.chapters?.missionNodeCount ?? 0) + ' wezlow misji'));
      setText('user-quickplay-role', data.quickPlay?.preferredRole || 'n/a');
      setText('user-quickplay-sub', data.quickPlay?.scenarioId || data.quickPlay?.error || 'Quickplay niegotowy');

      setText('home-summary', data.home?.continueSummary || 'Brak aktywnej sesji do wznowienia.');
      renderList('home-cards', data.home?.cards || [], (card) => `${card.type}: ${card.labelKey} -> ${card.route}`);

      setText('mission-entry-title', data.missionEntry?.title || 'Brak danych mission entry');
      setText('mission-entry-meta', [data.missionEntry?.difficulty, data.missionEntry?.recommendedRole].filter(Boolean).join(' · '));
      renderList('mission-risk-tags', data.missionEntry?.riskTags || [], (tag) => tag);

      setText('briefing-title', data.briefing?.title || 'Brak briefingu');
      setText('briefing-meta', [data.briefing?.difficulty, data.briefing?.estimatedMinutes ? `${data.briefing.estimatedMinutes} min` : null].filter(Boolean).join(' · '));
      renderList('briefing-objectives', data.briefing?.primaryObjectives || [], (objective) => objective);

      setText('showcase-title', data.showcase?.title || 'Brak showcase mission');
      renderList('showcase-steps', data.showcase?.steps || [], (step) => `${step.order}. ${step.title}`);

      setText('launchpad-copy', data.callToAction?.description || 'Dashboard gotowy do prezentacji.');
    })
    .catch((error) => {
      setText('user-api-status', 'offline');
      setText('user-api-sub', 'Nie udalo sie pobrac danych dashboardu');
      setText('home-summary', String(error.message || error));
      setText('launchpad-copy', 'Sprawdz API i sproboj ponownie.');
    });
})();
