import type { RoutesJsonResult, TypeScriptJsonResult, Repository, AppNotification } from '../types/api';

export interface AnalyzeRequest {
  repoUrl: string;
  branch?: string;
}

interface DocumentationResult { format: string; content: string; generatedAt: string; }

const BASE = (import.meta.env.VITE_API_URL ?? 'http://localhost:5141').replace(/\/$/, '');

function authHeaders(token?: string | null): Record<string, string> {
  return token ? { Authorization: `Bearer ${token}` } : {};
}

async function post<T>(path: string, body: object, token?: string | null): Promise<T> {
  const res = await fetch(`${BASE}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', ...authHeaders(token) },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
  return res.json();
}

async function get<T>(path: string, token?: string | null): Promise<T> {
  const res = await fetch(`${BASE}${path}`, { headers: authHeaders(token) });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
  return res.json();
}

async function del(path: string, token?: string | null): Promise<void> {
  const res = await fetch(`${BASE}${path}`, { method: 'DELETE', headers: authHeaders(token) });
  if (!res.ok && res.status !== 204) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
}

async function patch(path: string, body: object, token?: string | null): Promise<void> {
  const res = await fetch(`${BASE}${path}`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json', ...authHeaders(token) },
    body: JSON.stringify(body),
  });
  if (!res.ok && res.status !== 204) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
}

// ── Public analysis ───────────────────────────────────────────────────────

export async function fetchRoutesJson(req: AnalyzeRequest): Promise<RoutesJsonResult> {
  const result = await post<DocumentationResult>('/api/documentation/json/routes', req);
  return JSON.parse(result.content) as RoutesJsonResult;
}

export async function fetchTypeScriptJson(req: AnalyzeRequest): Promise<TypeScriptJsonResult> {
  const result = await post<DocumentationResult>('/api/documentation/json/typescript', req);
  return JSON.parse(result.content) as TypeScriptJsonResult;
}

// ── Repositories (authenticated) ──────────────────────────────────────────

export interface GithubRepoOption {
  githubRepoId: number;
  owner: string;
  name: string;
  fullName: string;
  defaultBranch: string;
  isPrivate: boolean;
  description: string | null;
  htmlUrl: string;
  language: string | null;
  updatedAt: string;
}

export async function fetchAvailableRepos(token: string): Promise<GithubRepoOption[]> {
  return get('/api/repos/available', token);
}

export async function fetchConnectedRepos(token: string): Promise<Repository[]> {
  return get('/api/repos', token);
}

export async function connectRepo(token: string, body: GithubRepoOption): Promise<{ id: string }> {
  return post('/api/repos/connect', {
    githubRepoId: body.githubRepoId,
    owner: body.owner,
    name: body.name,
    fullName: body.fullName,
    defaultBranch: body.defaultBranch,
    isPrivate: body.isPrivate,
    description: body.description,
    htmlUrl: body.htmlUrl,
  }, token);
}

export async function disconnectRepo(token: string, id: string): Promise<void> {
  return del(`/api/repos/${id}`, token);
}

export async function triggerScan(token: string, id: string): Promise<{ scanId: string }> {
  return post(`/api/repos/${id}/scan`, {}, token);
}

export async function fetchScanHistory(token: string, repoId: string, page = 1): Promise<object[]> {
  return get(`/api/repos/${repoId}/scans?page=${page}&pageSize=20`, token);
}

export async function updateNotificationSettings(
  token: string, repoId: string,
  settings: { slackWebhookUrl?: string | null; discordWebhookUrl?: string | null }
): Promise<void> {
  return patch(`/api/repos/${repoId}/notifications`, settings, token);
}

// ── Notifications (authenticated) ─────────────────────────────────────────

export async function fetchNotifications(
  token: string,
  opts?: { unreadOnly?: boolean; page?: number }
): Promise<{ notifications: AppNotification[]; unreadCount: number }> {
  const params = new URLSearchParams();
  if (opts?.unreadOnly) params.set('unreadOnly', 'true');
  if (opts?.page) params.set('page', String(opts.page));
  const qs = params.size ? `?${params}` : '';
  return get(`/api/notifications${qs}`, token);
}

export async function markNotificationRead(token: string, id: string): Promise<void> {
  return patch(`/api/notifications/${id}/read`, {}, token);
}

export async function markAllNotificationsRead(token: string): Promise<void> {
  return post('/api/notifications/read-all', {}, token);
}
