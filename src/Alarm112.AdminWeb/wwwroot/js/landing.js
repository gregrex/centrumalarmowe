(function () {
  'use strict';

  const form = document.getElementById('contact-form');
  const status = document.getElementById('contact-status');
  if (!form || !status) return;

  form.addEventListener('submit', async function (event) {
    event.preventDefault();
    status.textContent = 'Wysylanie...';

    const payload = {
      name: document.getElementById('lead-name')?.value?.trim() ?? '',
      email: document.getElementById('lead-email')?.value?.trim() ?? '',
      company: document.getElementById('lead-company')?.value?.trim() ?? '',
      message: document.getElementById('lead-message')?.value?.trim() ?? ''
    };

    try {
      const response = await fetch('/api/public/contact', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });

      const data = await response.json();
      status.textContent = response.ok
        ? (data.message || 'Prosba o demo zostala przyjeta.')
        : 'Nie udalo sie zapisac prosby o demo.';
    } catch (error) {
      status.textContent = 'Nie udalo sie polaczyc z formularzem demo.';
    }
  });
})();
