import * as XLSX from 'xlsx';

export function stripHtml(input?: string): string {
  if (!input) return '';
  const div = document.createElement('div');
  div.innerHTML = input;
  return (div.textContent || div.innerText || '').trim();
}

export function formatDate(val?: Date | string | null): string {
  if (!val) return '';
  const d = new Date(val);
  if (isNaN(d.getTime())) return '';
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}
          ${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`.replace(/\s+/g, ' ');
}

export function autosize(worksheet: XLSX.WorkSheet, rows: any[]) {
  const keys = Object.keys(rows[0] || {});
  worksheet['!cols'] = keys.map(k => {
    const maxLen = rows.reduce((m, r) => Math.max(m, String(r[k] ?? '').length), k.length);
    return { wch: Math.min(Math.max(10, maxLen + 2), 60) };
  });
}
