import { fetchCasesWithFallback } from './api.js';
const toggle = document.getElementById('nav-toggle');
const nav = document.getElementById('nav');
toggle?.addEventListener('click', () => {
  const open = nav.classList.toggle('open');
  toggle.setAttribute('aria-expanded', String(open));
  toggle.textContent = open ? 'Close' : 'Menu';
});
function renderCases(data) {
  const list = document.getElementById('cases-list');
  if (!data.items?.length) { list.innerHTML = '<p>No cases match that filter.</p>'; return; }
  list.innerHTML = '<ul>' + data.items.map(c =>
    `<li><strong>${c.reference}</strong> — ${c.title} <span>(${c.status})</span></li>`).join('') + '</ul>';
  document.getElementById('stat-total').textContent = data.total;
  document.getElementById('stat-approved').textContent = data.items.filter(c => c.status === 'Approved').length;
  document.getElementById('stat-review').textContent = data.items.filter(c => c.status === 'InReview').length;
}
async function load(params = {}) {
  document.getElementById('api-status').textContent = 'Loading cases…';
  try {
    const data = await fetchCasesWithFallback(params);
    renderCases(data);
    document.getElementById('api-status').textContent = `Showing ${data.items.length} of ${data.total} cases.`;
  } catch (e) {
    document.getElementById('cases-list').innerHTML = '<p>Could not load cases. Start the API in 02- or check data/cases.json.</p>';
  }
}
document.getElementById('filter-form')?.addEventListener('submit', (e) => {
  e.preventDefault();
  const params = {};
  const search = document.getElementById('f-search').value.trim();
  const status = document.getElementById('f-status').value;
  if (search) params.search = search;
  if (status) params.status = status;
  load(params);
});
load();
// Two-step apply form with draft in sessionStorage (dummy data only).
const nameEl = document.getElementById('a-name'), titleEl = document.getElementById('a-title');
const draft = JSON.parse(sessionStorage.getItem('apply-draft') || '{}');
if (draft.name) nameEl.value = draft.name;
if (draft.title) titleEl.value = draft.title;
function showStep(n) {
  document.getElementById('apply-step-1').hidden = n !== 1;
  document.getElementById('apply-step-2').hidden = n !== 2;
  document.getElementById('step-1').setAttribute('aria-current', n === 1 ? 'step' : 'false');
  document.getElementById('step-2').setAttribute('aria-current', n === 2 ? 'step' : 'false');
}
document.getElementById('to-step-2')?.addEventListener('click', () => {
  if (!nameEl.value.trim() || !titleEl.value.trim()) {
    document.getElementById('a-err').textContent = 'Enter your full name and a case title.'; nameEl.focus(); return;
  }
  document.getElementById('a-err').textContent = '';
  sessionStorage.setItem('apply-draft', JSON.stringify({ name: nameEl.value, title: titleEl.value }));
  document.getElementById('review-text').textContent = `${nameEl.value} — ${titleEl.value}`;
  showStep(2);
});
document.getElementById('back-step-1')?.addEventListener('click', () => showStep(1));
document.getElementById('apply-form')?.addEventListener('submit', (e) => {
  e.preventDefault();
  document.getElementById('a-out').textContent = 'Demo only — nothing was submitted. In production this POSTs to /api/v1/cases with a JWT.';
  sessionStorage.removeItem('apply-draft');
});
