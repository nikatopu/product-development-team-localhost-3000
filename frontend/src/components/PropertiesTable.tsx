import type { PropertyInfo } from '../types/api';
import styles from './PropertiesTable.module.css';

interface Props {
  properties: PropertyInfo[];
  depth?: number;
}

export function PropertiesTable({ properties, depth = 0 }: Props) {
  if (!properties.length) return null;

  return (
    <table className={styles.table}>
      <thead>
        <tr>
          <th>Property</th>
          <th>Type</th>
          <th>Required</th>
          {depth === 0 && <th>Description</th>}
        </tr>
      </thead>
      <tbody>
        {properties.map(prop => (
          <>
            <tr key={prop.name}>
              <td>
                <span className={styles.propName}>{prop.name}</span>
              </td>
              <td>
                <span className={styles.type}>{prop.type}</span>
              </td>
              <td>
                <span className={prop.isRequired ? styles.required : styles.optional}>
                  {prop.isRequired ? 'required' : 'optional'}
                </span>
              </td>
              {depth === 0 && (
                <td className={styles.summary}>{prop.summary ?? '—'}</td>
              )}
            </tr>
            {prop.nestedProperties && prop.nestedProperties.length > 0 && (
              <tr key={`${prop.name}-nested`} className={styles.nestedRow}>
                <td colSpan={depth === 0 ? 4 : 3}>
                  <div className={styles.nested}>
                    <span className={styles.nestedLabel}>{prop.type} shape:</span>
                    <PropertiesTable properties={prop.nestedProperties} depth={depth + 1} />
                  </div>
                </td>
              </tr>
            )}
          </>
        ))}
      </tbody>
    </table>
  );
}