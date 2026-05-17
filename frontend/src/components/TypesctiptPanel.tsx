import { useState } from 'react';
import type { TypeScriptJsonResult, RouteType } from '../types/api';
import { MethodBadge } from './MethodBadge';
import styles from './TypescriptPanel.module.css';

interface Props {
  data: TypeScriptJsonResult;
}

function CopyButton({ text }: { text: string }) {
  const [copied, setCopied] = useState(false);

  const copy = async () => {
    await navigator.clipboard.writeText(text);
    setCopied(true);
    setTimeout(() => setCopied(false), 1800);
  };

  return (
    <button className={styles.copyBtn} onClick={copy}>
      {copied ? (
        <>
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
            <polyline points="20 6 9 17 4 12" />
          </svg>
          Copied
        </>
      ) : (
        <>
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <rect x="9" y="9" width="13" height="13" rx="2" /><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1" />
          </svg>
          Copy
        </>
      )}
    </button>
  );
}

function buildInterface(name: string, props: TypeScriptJsonResult['interfaces'][string]): string {
  const lines = [`export interface ${name} {`];
  for (const p of props) {
    if (p.summary) lines.push(`  /** ${p.summary} */`);
    lines.push(`  ${p.name}${p.isRequired ? '' : '?'}: ${p.type};`);
  }
  lines.push('}');
  return lines.join('\n');
}

function buildFetchFunction(rt: RouteType): string {
  const lines: string[] = [];
  if (rt.summary) lines.push(`/** ${rt.summary} */`);

  const paramParts = rt.fetchParams.map(p => `${p.name}: ${p.type}`);
  const returnType = rt.responseTypeName ?? 'void';

  lines.push(`export async function ${rt.functionName}(${paramParts.join(', ')}): Promise<${returnType}> {`);

  if (rt.hasQueryParams) {
    lines.push(`  const query = params`);
    lines.push(`    ? '?' + new URLSearchParams(params as Record<string, string>).toString()`);
    lines.push(`    : '';`);
  }

  let urlPath = rt.path;
  for (const p of rt.routeParams) {
    urlPath = urlPath.replace(`{${p.name}}`, `\${${p.name}}`);
  }
  const urlExpr = rt.hasQueryParams
    ? `\`\${BASE_URL}${urlPath}\${query}\``
    : `\`\${BASE_URL}${urlPath}\``;

  lines.push(`  const res = await fetch(${urlExpr}, {`);
  lines.push(`    method: '${rt.method}',`);
  if (rt.hasBody) {
    lines.push(`    headers: { 'Content-Type': 'application/json' },`);
    lines.push(`    body: JSON.stringify(body),`);
  }
  lines.push(`  });`);
  lines.push(`  if (!res.ok) throw new Error(\`HTTP \${res.status}: \${res.statusText}\`);`);
  if (returnType !== 'void') lines.push(`  return res.json() as Promise<${returnType}>;`);
  lines.push('}');

  return lines.join('\n');
}

export function TypeScriptPanel({ data }: Props) {
  const [tab, setTab] = useState<'interfaces' | 'functions'>('interfaces');

  const allInterfacesCode = Object.entries(data.interfaces)
    .map(([name, props]) => buildInterface(name, props))
    .join('\n\n');

  const allFunctionsCode = [
    `const BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000';`,
    '',
    ...data.routeTypes.map(rt => buildFetchFunction(rt)),
  ].join('\n\n');

  return (
    <div className={styles.panel}>
      <div className={styles.tabs}>
        <button
          className={`${styles.tab} ${tab === 'interfaces' ? styles.active : ''}`}
          onClick={() => setTab('interfaces')}
        >
          Interfaces
          <span className={styles.count}>{Object.keys(data.interfaces).length}</span>
        </button>
        <button
          className={`${styles.tab} ${tab === 'functions' ? styles.active : ''}`}
          onClick={() => setTab('functions')}
        >
          Fetch Functions
          <span className={styles.count}>{data.routeTypes.length}</span>
        </button>
      </div>

      {tab === 'interfaces' && (
        <div className={styles.content}>
          <div className={styles.blockHeader}>
            <span className={styles.blockLabel}>All interfaces</span>
            <CopyButton text={allInterfacesCode} />
          </div>
          {Object.entries(data.interfaces).map(([name, props]) => (
            <div key={name} className={styles.block}>
              <div className={styles.blockInnerHeader}>
                <span className={styles.interfaceName}>{name}</span>
                <CopyButton text={buildInterface(name, props)} />
              </div>
              <pre className={styles.code}>{buildInterface(name, props)}</pre>
            </div>
          ))}
          {Object.keys(data.interfaces).length === 0 && (
            <p className={styles.empty}>No interfaces extracted — types may be primitives only.</p>
          )}
        </div>
      )}

      {tab === 'functions' && (
        <div className={styles.content}>
          <div className={styles.blockHeader}>
            <span className={styles.blockLabel}>All fetch functions</span>
            <CopyButton text={allFunctionsCode} />
          </div>
          {data.routeTypes.map(rt => (
            <div key={rt.functionName} className={styles.block}>
              <div className={styles.blockInnerHeader}>
                <div className={styles.fnMeta}>
                  <MethodBadge method={rt.method} />
                  <span className={styles.fnName}>{rt.functionName}</span>
                  <span className={styles.fnPath}>{rt.path}</span>
                </div>
                <CopyButton text={buildFetchFunction(rt)} />
              </div>
              <pre className={styles.code}>{buildFetchFunction(rt)}</pre>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}