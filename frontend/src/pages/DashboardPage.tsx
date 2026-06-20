import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { fetchConnectedRepos } from '../lib/ApiClient';
import type { Repository } from '../types/api';
import type { Page } from '../App';
import styles from './DashboardPage.module.css';

interface Props {
  onNavigate: (page: Page, repoId?: string) => void;
}

export function DashboardPage({ onNavigate }: Props) {
  const { accessToken } = useAuth();
  const [repos, setRepos] = useState<Repository[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');

  useEffect(() => {
    if (!accessToken) return;
    fetchConnectedRepos(accessToken)
      .then(setRepos)
      .catch(() => {})
      .finally(() => setLoading(false));
  }, [accessToken]);

  const totalRoutes = repos.reduce((sum, r) => sum + (r.lastScan?.totalRoutes ?? 0), 0);
  const totalBreaking = repos.reduce((sum, r) => sum + (r.lastScan?.breakingChangeCount ?? 0), 0);
  const readyCount = repos.filter(r => r.status === 'Ready').length;

  const filtered = repos.filter(r =>
    r.fullName.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className={styles.page}>
      <div className={styles.header}>
        <h1 className={styles.title}>Dashboard</h1>
        <p className={styles.subtitle}>Overview of all connected API repositories.</p>
      </div>

      {/* Summary cards */}
      <div className={styles.cards}>
        <div className={styles.card}>
          <span className={styles.cardValue}>{repos.length}</span>
          <span className={styles.cardLabel}>Repositories</span>
        </div>
        <div className={styles.card}>
          <span className={styles.cardValue}>{totalRoutes}</span>
          <span className={styles.cardLabel}>Total Routes</span>
        </div>
        <div className={styles.card}>
          <span className={styles.cardValue}>{readyCount}</span>
          <span className={styles.cardLabel}>Ready</span>
        </div>
        <div className={`${styles.card} ${totalBreaking > 0 ? styles.cardWarning : ''}`}>
          <span className={styles.cardValue}>{totalBreaking}</span>
          <span className={styles.cardLabel}>Breaking Changes</span>
        </div>
      </div>

      {/* Repository list with search */}
      <div className={styles.section}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>Repositories</h2>
          <input
            className={styles.search}
            placeholder="Filter repositories…"
            value={search}
            onChange={e => setSearch(e.target.value)}
          />
        </div>

        {loading ? (
          <div className={styles.loading}>Loading…</div>
        ) : filtered.length === 0 ? (
          <div className={styles.empty}>
            {repos.length === 0
              ? 'No repositories connected yet.'
              : 'No repositories match your search.'}
          </div>
        ) : (
          <div className={styles.repoGrid}>
            {filtered.map(repo => (
              <div key={repo.id} className={styles.repoCard} onClick={() => onNavigate('repos')}>
                <div className={styles.repoTop}>
                  <span className={styles.repoName}>{repo.fullName}</span>
                  <StatusDot status={repo.status} />
                </div>
                {repo.lastScan ? (
                  <div className={styles.repoStats}>
                    <Stat label="Routes" value={repo.lastScan.totalRoutes} />
                    <Stat label="Controllers" value={repo.lastScan.totalControllers} />
                    {repo.lastScan.breakingChangeCount > 0 && (
                      <span className={styles.breakingPill}>
                        ⚠ {repo.lastScan.breakingChangeCount} breaking
                      </span>
                    )}
                  </div>
                ) : (
                  <span className={styles.noScan}>Not yet scanned</span>
                )}
                {repo.lastScannedAt && (
                  <span className={styles.scannedAt}>
                    Scanned {new Date(repo.lastScannedAt).toLocaleDateString()}
                  </span>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

function StatusDot({ status }: { status: string }) {
  const cls = {
    Connected: styles.dotBlue,
    Scanning: styles.dotYellow,
    Ready: styles.dotGreen,
    Failed: styles.dotRed,
  }[status] ?? styles.dotBlue;
  return <span className={`${styles.dot} ${cls}`} title={status} />;
}

function Stat({ label, value }: { label: string; value: number }) {
  return (
    <div className={styles.stat}>
      <span className={styles.statValue}>{value}</span>
      <span className={styles.statLabel}>{label}</span>
    </div>
  );
}
