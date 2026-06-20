import { useState, useEffect, useCallback } from 'react';
import { useAuth } from '../contexts/AuthContext';
import {
  fetchConnectedRepos,
  fetchAvailableRepos,
  connectRepo,
  disconnectRepo,
  triggerScan,
  type GithubRepoOption,
} from '../lib/ApiClient';
import type { Repository } from '../types/api';
import styles from './RepositoriesPage.module.css';

const STATUS_LABEL: Record<string, string> = {
  Connected: 'Connected',
  Scanning: 'Scanning…',
  Ready: 'Ready',
  Failed: 'Failed',
};

const STATUS_CLASS: Record<string, string> = {
  Connected: 'statusConnected',
  Scanning: 'statusScanning',
  Ready: 'statusReady',
  Failed: 'statusFailed',
};

export function RepositoriesPage() {
  const { accessToken } = useAuth();
  const [connected, setConnected] = useState<Repository[]>([]);
  const [available, setAvailable] = useState<GithubRepoOption[]>([]);
  const [loadingConnected, setLoadingConnected] = useState(true);
  const [loadingAvailable, setLoadingAvailable] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showConnect, setShowConnect] = useState(false);
  const [searchFilter, setSearchFilter] = useState('');
  const [scanning, setScanning] = useState<Set<string>>(new Set());

  const loadConnected = useCallback(async () => {
    if (!accessToken) return;
    try {
      const repos = await fetchConnectedRepos(accessToken);
      setConnected(repos);
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to load repositories');
    } finally {
      setLoadingConnected(false);
    }
  }, [accessToken]);

  useEffect(() => { loadConnected(); }, [loadConnected]);

  const openConnectDialog = async () => {
    setShowConnect(true);
    setLoadingAvailable(true);
    try {
      const repos = await fetchAvailableRepos(accessToken!);
      const connectedIds = new Set(connected.map(r => r.id));
      setAvailable(repos.filter(r => !connectedIds.has(String(r.githubRepoId))));
    } catch {
      setError('Failed to load GitHub repositories');
    } finally {
      setLoadingAvailable(false);
    }
  };

  const handleConnect = async (repo: GithubRepoOption) => {
    try {
      await connectRepo(accessToken!, repo);
      setShowConnect(false);
      await loadConnected();
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to connect repository');
    }
  };

  const handleDisconnect = async (id: string) => {
    if (!confirm('Remove this repository from Driftless?')) return;
    try {
      await disconnectRepo(accessToken!, id);
      setConnected(prev => prev.filter(r => r.id !== id));
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to disconnect');
    }
  };

  const handleScan = async (id: string) => {
    setScanning(prev => new Set(prev).add(id));
    try {
      await triggerScan(accessToken!, id);
      setConnected(prev =>
        prev.map(r => r.id === id ? { ...r, status: 'Scanning' } : r)
      );
      // Poll for completion
      const poll = setInterval(async () => {
        await loadConnected();
        setConnected(prev => {
          const repo = prev.find(r => r.id === id);
          if (repo && repo.status !== 'Scanning') {
            clearInterval(poll);
            setScanning(s => { const ns = new Set(s); ns.delete(id); return ns; });
          }
          return prev;
        });
      }, 4000);
      setTimeout(() => clearInterval(poll), 120_000);
    } catch (e) {
      setError(e instanceof Error ? e.message : 'Failed to trigger scan');
      setScanning(prev => { const ns = new Set(prev); ns.delete(id); return ns; });
    }
  };

  const filteredAvailable = available.filter(r =>
    r.fullName.toLowerCase().includes(searchFilter.toLowerCase())
  );

  return (
    <div className={styles.page}>
      <div className={styles.header}>
        <div>
          <h1 className={styles.title}>Repositories</h1>
          <p className={styles.subtitle}>Connect GitHub repositories to track their API contracts.</p>
        </div>
        <button className={styles.connectBtn} onClick={openConnectDialog}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Connect repository
        </button>
      </div>

      {error && (
        <div className={styles.error}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
          {error}
          <button className={styles.dismiss} onClick={() => setError(null)}>✕</button>
        </div>
      )}

      {loadingConnected ? (
        <div className={styles.loading}>Loading repositories…</div>
      ) : connected.length === 0 ? (
        <div className={styles.empty}>
          <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.2">
            <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22" />
          </svg>
          <h2>No repositories connected</h2>
          <p>Click "Connect repository" to link your first GitHub project.</p>
        </div>
      ) : (
        <div className={styles.repoList}>
          {connected.map(repo => (
            <div key={repo.id} className={styles.repoCard}>
              <div className={styles.repoHeader}>
                <div className={styles.repoInfo}>
                  <span className={styles.repoName}>{repo.fullName}</span>
                  <span className={`${styles.statusBadge} ${styles[STATUS_CLASS[repo.status] ?? 'statusConnected']}`}>
                    {repo.status === 'Scanning' && (
                      <span className={styles.spinner} />
                    )}
                    {STATUS_LABEL[repo.status] ?? repo.status}
                  </span>
                </div>
                <div className={styles.repoActions}>
                  <button
                    className={styles.scanBtn}
                    onClick={() => handleScan(repo.id)}
                    disabled={scanning.has(repo.id) || repo.status === 'Scanning'}
                  >
                    {scanning.has(repo.id) ? 'Scanning…' : 'Scan now'}
                  </button>
                  <button className={styles.disconnectBtn} onClick={() => handleDisconnect(repo.id)}>
                    Remove
                  </button>
                </div>
              </div>

              {repo.lastScan && (
                <div className={styles.scanStats}>
                  <span>{repo.lastScan.totalRoutes} routes</span>
                  <span>{repo.lastScan.totalControllers} controllers</span>
                  <span>{repo.lastScan.apiType}</span>
                  {repo.lastScan.breakingChangeCount > 0 && (
                    <span className={styles.breakingCount}>
                      ⚠ {repo.lastScan.breakingChangeCount} breaking
                    </span>
                  )}
                  {repo.lastScannedAt && (
                    <span className={styles.lastScanned}>
                      Last scanned {new Date(repo.lastScannedAt).toLocaleDateString()}
                    </span>
                  )}
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {/* Connect dialog */}
      {showConnect && (
        <div className={styles.overlay} onClick={() => setShowConnect(false)}>
          <div className={styles.dialog} onClick={e => e.stopPropagation()}>
            <div className={styles.dialogHeader}>
              <h2>Connect a repository</h2>
              <button className={styles.closeBtn} onClick={() => setShowConnect(false)}>✕</button>
            </div>

            <input
              className={styles.dialogSearch}
              placeholder="Search repositories…"
              value={searchFilter}
              onChange={e => setSearchFilter(e.target.value)}
              autoFocus
            />

            {loadingAvailable ? (
              <div className={styles.loading}>Loading from GitHub…</div>
            ) : (
              <div className={styles.availableList}>
                {filteredAvailable.length === 0 ? (
                  <p className={styles.noResults}>No repositories found.</p>
                ) : (
                  filteredAvailable.map(r => (
                    <button
                      key={r.githubRepoId}
                      className={styles.availableItem}
                      onClick={() => handleConnect(r)}
                    >
                      <div className={styles.availableName}>
                        {r.isPrivate && <span className={styles.privateBadge}>Private</span>}
                        {r.fullName}
                      </div>
                      {r.description && <span className={styles.availableDesc}>{r.description}</span>}
                    </button>
                  ))
                )}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
