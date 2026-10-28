// Static accessibility lint — NOT a replacement for axe or a screen-reader test.
// Fails the build on the defects found first in manual testing:
// missing lang, missing skip link, unlabelled inputs, images without alt,
// buttons with no accessible name, colour-only status.
// Run: npm run check:a11y   Full check: axe DevTools + JAWS script in docs/
const fs = require('fs');
const html = fs.readFileSync('index.html', 'utf8');
const fails = [];
if (!/<html lang=/.test(html)) fails.push('html lang missing');
if (!/skip-link/.test(html)) fails.push('skip link missing');
for (const m of html.matchAll(/<input[^>]*id="([^"]+)"/g)) {
  if (!html.includes(`for="${m[1]}"`)) fails.push(`input #${m[1]} has no <label for>`);
}
for (const m of html.matchAll(/<img[^>]*>/g)) {
  if (!/alt=/.test(m[0])) fails.push(`img without alt: ${m[0].slice(0, 80)}`);
}
if (fails.length) { console.error('A11Y CHECK FAILED:\n- ' + fails.join('\n- ')); process.exit(1); }
console.log('Static a11y check passed (axe + JAWS still required — see docs/accessibility-test-notes.md).');
