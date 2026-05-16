import { useState } from 'react';
import { useAnalysis } from '../hooks/useAnalysis';
import { UrlForm } from '../components/UrlForm';
import { RouteCard } from '../components/RouteCard';
import { TypeScriptPanel } from '../components/TypesctiptPanel';
import styles from './HomePage.module.css';

type Tab = 'routes' | 'typescript';

export function HomePage() {
  const { routes, typescript, loading, error, analyze } = useAnalysis();
  const [tab, setTab] = useState<Tab>('routes');
  const [filter, setFilter] = useState('');
  const [methodFilter, setMethodFilter] = useState<string>('ALL');

  const hasResult = !!routes;

  const filteredRoutes = routes?.routes.filter(r => {
    const matchesText =
      r.path.toLowerCase().includes(filter.toLowerCase()) ||
      r.controller.toLowerCase().includes(filter.toLowerCase()) ||
      r.summary?.toLowerCase().includes(filter.toLowerCase());
    const matchesMethod = methodFilter === 'ALL' || r.method === methodFilter;
    return matchesText && matchesMethod;
  }) ?? [];

  const methods = ['ALL', ...Array.from(new Set(routes?.routes.map(r => r.method) ?? []))];

  return (
    <div className={styles.page}>
      {/* Header */}
      <header className={styles.header}>
        <div className={styles.logo}>
          <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8">
            <polyline points="16 18 22 12 16 6" />
            <polyline points="8 6 2 12 8 18" />
          </svg>
          <span>ApiDocGen</span>
        </div>
        <p className={styles.tagline}>Paste a repo URL. Get instant API docs.</p>
      </header>

      {/* Search bar */}
      <div className={styles.searchBar}>
        <UrlForm onSubmit={analyze} loading={loading} />
      </div>

      {/* Error */}
      {error && (
        <div className={styles.error}>
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
          </svg>
          {error}
        </div>
      )}

      {/* Empty state */}
      {!hasResult && !loading && !error && (
        <div className={styles.emptyState}>
          <div className={styles.emptyIcon}>
            <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.2">
              <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z" />
              <polyline points="14 2 14 8 20 8" />
              <line x1="16" y1="13" x2="8" y2="13" /><line x1="16" y1="17" x2="8" y2="17" /><polyline points="10 9 9 9 8 9" />
            </svg>
          </div>
          <h2>No repository analyzed yet</h2>
          <p>Enter a public GitHub repository URL above to extract all API routes and generate TypeScript types.</p>
          <div className={styles.examples}>
            <span>Try:</span>
            <code>https://github.com/tomadanelia/community-dashboard.git</code>
          </div>
        </div>
      )}

      {/* Results */}
      {hasResult && (
        <div className={styles.results}>
          {/* Metadata bar */}
          <div className={styles.metaBar}>
            <div className={styles.metaLeft}>
              <span className={styles.projectName}>{routes.projectName}</span>
              <span className={styles.metaPill}>{routes.metadata.totalRoutes} routes</span>
              <span className={styles.metaPill}>{routes.metadata.totalControllers} controllers</span>
              <span className={styles.metaPill}>{routes.metadata.apiType}</span>
              {routes.metadata.detectedFrameworks.map(f => (
                <span key={f} className={styles.metaPill}>{f}</span>
              ))}
            </div>
          </div>

          {/* Tab switcher */}
          <div className={styles.tabBar}>
            <button
              className={`${styles.tabBtn} ${tab === 'routes' ? styles.tabActive : ''}`}
              onClick={() => setTab('routes')}
            >
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <line x1="3" y1="12" x2="21" y2="12" /><line x1="3" y1="6" x2="21" y2="6" /><line x1="3" y1="18" x2="21" y2="18" />
              </svg>
              Routes
            </button>
            <button
              className={`${styles.tabBtn} ${tab === 'typescript' ? styles.tabActive : ''}`}
              onClick={() => setTab('typescript')}
            >
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <polyline points="16 18 22 12 16 6" /><polyline points="8 6 2 12 8 18" />
              </svg>
              TypeScript
            </button>
          </div>

          {/* Routes view */}
          {tab === 'routes' && (
            <div className={styles.routesView}>
              {/* Filters */}
              <div className={styles.filters}>
                <div className={styles.searchBox}>
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                    <circle cx="11" cy="11" r="8" /><line x1="21" y1="21" x2="16.65" y2="16.65" />
                  </svg>
                  <input
                    className={styles.searchInput}
                    placeholder="Filter routes…"
                    value={filter}
                    onChange={e => setFilter(e.target.value)}
                  />
                </div>
                <div className={styles.methodFilters}>
                  {methods.map(m => (
                    <button
                      key={m}
                      className={`${styles.methodBtn} ${methodFilter === m ? styles.methodActive : ''}`}
                      onClick={() => setMethodFilter(m)}
                    >
                      {m}
                    </button>
                  ))}
                </div>
              </div>

              {/* Route list */}
              <div className={styles.routeList}>
                {filteredRoutes.length > 0 ? (
                  filteredRoutes.map((route, i) => (
                    <RouteCard key={`${route.method}-${route.path}-${i}`} route={route} />
                  ))
                ) : (
                  <p className={styles.noResults}>No routes match your filter.</p>
                )}
              </div>
            </div>
          )}

          {/* TypeScript view */}
          {tab === 'typescript' && typescript && (
            <TypeScriptPanel data={typescript} />
          )}
        </div>
      )}
    </div>
  );
}