import { useState } from 'react';
import type { Route } from '../types/api';
import { MethodBadge } from './MethodBadge';
import { PropertiesTable } from './PropertiesTable';
import styles from './RouteCard.module.css';

interface Props {
  route: Route;
}

export function RouteCard({ route }: Props) {
  const [expanded, setExpanded] = useState(false);

  const hasBody = !!route.requestBody;
  const hasParams = route.parameters.length > 0;
  const hasDetails = hasBody || hasParams || route.responses.some(r => r.properties.length > 0);

  return (
    <div className={`${styles.card} ${expanded ? styles.expanded : ''}`}>
      <button className={styles.header} onClick={() => setExpanded(v => !v)}>
        <div className={styles.left}>
          <MethodBadge method={route.method} />
          <span className={styles.path}>{route.path}</span>
          {route.summary && <span className={styles.summary}>{route.summary}</span>}
        </div>
        <div className={styles.right}>
          {hasBody && <span className={styles.tag}>body</span>}
          {hasParams && <span className={styles.tag}>{route.parameters.length} param{route.parameters.length > 1 ? 's' : ''}</span>}
          <span className={styles.chevron} data-open={expanded}>
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
              <polyline points="6 9 12 15 18 9" />
            </svg>
          </span>
        </div>
      </button>

      {expanded && hasDetails && (
        <div className={styles.body}>
          {/* Route / Query Parameters */}
          {hasParams && (
            <section className={styles.section}>
              <h4 className={styles.sectionTitle}>Parameters</h4>
              <table className={styles.paramTable}>
                <thead>
                  <tr>
                    <th>Name</th><th>Type</th><th>Source</th><th>Required</th>
                  </tr>
                </thead>
                <tbody>
                  {route.parameters.map(p => (
                    <tr key={p.name}>
                      <td><span className={styles.mono}>{p.name}</span></td>
                      <td><span className={styles.typeChip}>{p.type}</span></td>
                      <td><span className={styles.sourceChip}>{p.source}</span></td>
                      <td>
                        <span className={p.isRequired ? styles.req : styles.opt}>
                          {p.isRequired ? 'yes' : 'no'}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </section>
          )}

          {/* Request Body */}
          {hasBody && route.requestBody && (
            <section className={styles.section}>
              <h4 className={styles.sectionTitle}>
                Request Body
                <span className={styles.typePill}>{route.requestBody.typeName}</span>
              </h4>
              {route.requestBody.properties.length > 0 ? (
                <PropertiesTable properties={route.requestBody.properties} />
              ) : (
                <p className={styles.empty}>No property details available</p>
              )}
            </section>
          )}

          {/* Responses */}
          {route.responses.map(r => (
            <section key={r.statusCode} className={styles.section}>
              <h4 className={styles.sectionTitle}>
                <span className={`${styles.statusCode} ${r.statusCode < 300 ? styles.ok : styles.err}`}>
                  {r.statusCode}
                </span>
                {r.description}
                {r.typeName && r.typeName !== 'void' && (
                  <span className={styles.typePill}>{r.typeName}</span>
                )}
              </h4>
              {r.properties.length > 0 && (
                <PropertiesTable properties={r.properties} />
              )}
            </section>
          ))}
        </div>
      )}

      {expanded && !hasDetails && (
        <div className={styles.body}>
          <p className={styles.empty}>No additional details available for this route.</p>
        </div>
      )}
    </div>
  );
}