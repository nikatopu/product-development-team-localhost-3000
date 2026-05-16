import type { DocumentationResult, RoutesJsonResult, TypeScriptJsonResult } from '../types/api';

const BASE_URL = 'http://localhost:5141';

async function post<T>(path: string, body: object): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  });
  if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
  return res.json();
}

export interface AnalyzeRequest {
  repoUrl: string;
  branch?: string;
}

export async function fetchRoutesJson(req: AnalyzeRequest): Promise<RoutesJsonResult> {
  const result = await post<DocumentationResult>('/api/documentation/json/routes', req);
  return JSON.parse(result.content) as RoutesJsonResult;
}

export async function fetchTypeScriptJson(req: AnalyzeRequest): Promise<TypeScriptJsonResult> {
  const result = await post<DocumentationResult>('/api/documentation/json/typescript', req);
  return JSON.parse(result.content) as TypeScriptJsonResult;
}