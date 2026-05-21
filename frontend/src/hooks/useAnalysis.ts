import { useState } from 'react';
import posthog from 'posthog-js';
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
    const startTime = performance.now();

    try {
      const [routes, typescript] = await Promise.all([
        fetchRoutesJson({ repoUrl, branch }),
        fetchTypeScriptJson({ repoUrl, branch }),
      ]);

      const durationMs = Math.round(performance.now() - startTime);

      setState(s => ({ ...s, routes, typescript, loading: false }));

      // Fire activation event to PostHog
      posthog.capture('analysis_completed', {
        repo_url: repoUrl,
        branch_name: branch ?? 'default',
        route_count: routes.metadata.totalRoutes,
        controller_count: routes.metadata.totalControllers,
        duration_ms: durationMs,
        api_type: routes.metadata.apiType
      });

    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Unknown error';
      setState(s => ({
        ...s,
        loading: false,
        error: errorMessage,
      }));
      posthog.capture('analysis_failed', {
        repo_url: repoUrl,
        error_message: errorMessage
      });
    }
  };

  const setMode = (mode: ViewMode) => setState(s => ({ ...s, activeMode: mode }));

  return { ...state, analyze, setMode };
}