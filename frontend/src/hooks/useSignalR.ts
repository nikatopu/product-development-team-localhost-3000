import { useEffect, useRef, useCallback } from 'react';
import * as signalR from '@microsoft/signalr';

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5141';

export interface AnalysisProgressEvent {
  repoUrl: string;
  stage: string;
  percent: number;
}

export interface AnalysisCompleteEvent {
  repoUrl: string;
  totalRoutes: number;
  totalControllers: number;
  breakingChangeCount: number;
}

export interface AnalysisFailedEvent {
  repoUrl: string;
  error: string;
}

interface SignalRHandlers {
  onStarted?: (e: { repoUrl: string }) => void;
  onProgress?: (e: AnalysisProgressEvent) => void;
  onComplete?: (e: AnalysisCompleteEvent) => void;
  onFailed?: (e: AnalysisFailedEvent) => void;
}

export function useSignalR(repoUrl: string | null, handlers: SignalRHandlers) {
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const handlersRef = useRef(handlers);
  handlersRef.current = handlers;

  const connect = useCallback(async (url: string) => {
    if (connectionRef.current) {
      await connectionRef.current.stop();
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${API_URL}/hubs/analysis`)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build();

    connection.on('AnalysisStarted', (e) => handlersRef.current.onStarted?.(e));
    connection.on('AnalysisProgress', (e) => handlersRef.current.onProgress?.(e));
    connection.on('AnalysisComplete', (e) => handlersRef.current.onComplete?.(e));
    connection.on('AnalysisFailed', (e) => handlersRef.current.onFailed?.(e));

    try {
      await connection.start();
      await connection.invoke('JoinAnalysis', url);
      connectionRef.current = connection;
    } catch {
      // SignalR is optional — analysis still works via HTTP
    }
  }, []);

  useEffect(() => {
    if (!repoUrl) return;
    connect(repoUrl);

    return () => {
      connectionRef.current?.stop();
      connectionRef.current = null;
    };
  }, [repoUrl, connect]);
}
