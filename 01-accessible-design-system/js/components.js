// Tabs: arrow keys move, roving tabindex, aria-selected kept in sync.
const tabs = [...document.querySelectorAll('[role="tab"]')];
tabs.forEach((tab, i) => {
  tab.addEventListener('click', () => selectTab(i));
  tab.addEventListener('keydown', (e) => {
    if (e.key === 'ArrowRight') { tabs[(i + 1) % tabs.length].focus(); }
    if (e.key === 'ArrowLeft') { tabs[(i - 1 + tabs.length) % tabs.length].focus(); }
  });
});
function selectTab(i) {
  tabs.forEach((t, j) => {
    const selected = i === j;
    t.setAttribute('aria-selected', String(selected));
    t.tabIndex = selected ? 0 : -1;
    document.getElementById(t.getAttribute('aria-controls')).hidden = !selected;
  });
}
// Dialog: native <dialog> gives focus trapping and Escape for free —
// do not hand-roll a modal with divs.
const dialog = document.getElementById('confirm-dialog');
document.getElementById('open-dialog')?.addEventListener('click', () => dialog.showModal());
dialog?.addEventListener('close', () => {
  document.getElementById('dialog-out').textContent = dialog.returnValue === 'close' ? 'Case closed (demo).' : 'Cancelled.';
});
// Sortable table: sort by column, update aria-sort.
document.querySelectorAll('#sortable th button').forEach((btn) => {
  btn.addEventListener('click', () => {
    const table = document.getElementById('sortable');
    const tbody = table.tBodies[0];
    const col = btn.dataset.sort === 'ref' ? 0 : 1;
    const rows = [...tbody.rows];
    const th = btn.closest('th');
    const asc = th.getAttribute('aria-sort') !== 'ascending';
    rows.sort((a, b) => asc ? a.cells[col].textContent.localeCompare(b.cells[col].textContent) : b.cells[col].textContent.localeCompare(a.cells[col].textContent));
    rows.forEach(r => tbody.appendChild(r));
    table.querySelectorAll('th').forEach(h => h.setAttribute('aria-sort', 'none'));
    th.setAttribute('aria-sort', asc ? 'ascending' : 'descending');
  });
});
