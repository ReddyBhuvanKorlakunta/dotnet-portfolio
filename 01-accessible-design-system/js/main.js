// Vanilla JS only — no framework needed for this demo.
document.getElementById('demo-btn')?.addEventListener('click', () => {
  document.getElementById('demo-out').textContent = 'Case CA-1042 opened (demo — no data loaded).';
});
const form = document.getElementById('demo-form');
form?.addEventListener('submit', (e) => {
  e.preventDefault();
  const name = document.getElementById('f-name');
  const email = document.getElementById('f-email');
  let firstInvalid = null;
  const setErr = (input, errEl, msg) => {
    document.getElementById(errEl).textContent = msg;
    input.setAttribute('aria-invalid', msg ? 'true' : 'false');
    if (msg && !firstInvalid) firstInvalid = input;
  };
  setErr(name, 'f-name-err', name.value.trim() ? '' : 'Enter your full name.');
  setErr(email, 'f-email-err', /.+@.+\..+/.test(email.value) ? '' : 'Enter a valid work email.');
  if (firstInvalid) { firstInvalid.focus(); return; }
  document.getElementById('form-out').textContent = 'Thanks — demo form validated. Nothing was submitted.';
  form.reset();
});
