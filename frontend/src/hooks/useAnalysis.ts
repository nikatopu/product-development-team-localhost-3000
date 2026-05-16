import { useState } from 'react';
import { fetchRoutesJson, fetchTypeScriptJson } from '../lib/ApiClient';
import type { RoutesJsonResult, TypeScriptJsonResult } from '../types/api';

export type ViewMode = 'routes' | 'typescript';

interface AnalysisState {
  routes: RoutesJsonResult | null;
  typescript: TypeScriptJsonResult | null;
  loading: boolean;
  error: string | null;
  activeMode: ViewMode;
}

export function useAnalysis() {
  const [state, setState] = useState<AnalysisState>({
    routes: null,
    typescript: null,
    loading: false,
    error: null,
    activeMode: 'routes',
  });

  const analyze = async (repoUrl: string, branch?: string) => {
    setState(s => ({ ...s, loading: true, error: null, routes: null, typescript: null }));
    try {
      // Fetch both in parallel
      const [routes, typescript] = await Promise.all([
        fetchRoutesJson({ repoUrl, branch }),
        fetchTypeScriptJson({ repoUrl, branch }),
      ]);
      setState(s => ({ ...s, routes, typescript, loading: false }));
    } catch (err) {
      setState(s => ({
        ...s,
        loading: false,
        error: err instanceof Error ? err.message : 'Unknown error',
      }));
    }
  };

  const setMode = (mode: ViewMode) => setState(s => ({ ...s, activeMode: mode }));

  return { ...state, analyze, setMode };
}