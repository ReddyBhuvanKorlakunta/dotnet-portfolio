export const API_BASE = window.PORTAL_API_BASE || 'http://localhost:5000';
export async function fetchCases(params = {}) {
  const qs = new URLSearchParams(params).toString();
  const res = await fetch(`${API_BASE}/api/v1/cases?${qs}`);
  if (!res.ok) throw new Error(`API ${res.status}`);
  return res.json();
}
export async function fetchCasesWithFallback(params = {}) {
  try { return await fetchCases(params); }
  catch (e) {
    console.warn('API unavailable, using offline dummy data:', e.message);
    const data = await (await fetch('data/cases.json')).json();
    return { items: data, total: data.length, page: 1, pageSize: data.length, totalPages: 1 };
  }
}
