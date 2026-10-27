const toggle = document.getElementById('nav-toggle');
const nav = document.getElementById('nav');
toggle?.addEventListener('click', () => {
  const open = nav.classList.toggle('open');
  toggle.setAttribute('aria-expanded', String(open));
  toggle.textContent = open ? 'Close' : 'Menu';
});
const refresh = () => {
  // Dummy data only — in production this would call the API in 02-.
  document.getElementById('stat-a').textContent = '14';
  document.getElementById('stat-b').textContent = '9';
  document.getElementById('stat-c').textContent = '3 days';
};
document.getElementById('refresh')?.addEventListener('click', refresh);
refresh();
document.getElementById('apply-form')?.addEventListener('submit', (e) => {
  e.preventDefault();
  const name = document.getElementById('a-name');
  const err = document.getElementById('a-err');
  if (!name.value.trim()) { err.textContent = 'Enter your full name.'; name.focus(); return; }
  err.textContent = '';
  document.getElementById('a-out').textContent = 'Step 1 validated (demo — nothing submitted).';
});
