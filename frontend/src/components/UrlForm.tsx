import { useState } from 'react';
import styles from './UrlForm.module.css';

interface Props {
  onSubmit: (repoUrl: string, branch?: string) => void;
  loading: boolean;
}

export function UrlForm({ onSubmit, loading }: Props) {
  const [url, setUrl] = useState('');
  const [branch, setBranch] = useState('main');
  const [showBranch, setShowBranch] = useState(false);

  const handleSubmit = (e:any) => {
  e.preventDefault();
  if (!url.trim()) return;
  onSubmit(url.trim(), branch.trim() || undefined);
};

  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <div className={styles.inputRow}>
        <div className={styles.inputWrapper}>
          <span className={styles.icon}>
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
              <path d="M9 19c-5 1.5-5-2.5-7-3m14 6v-3.87a3.37 3.37 0 0 0-.94-2.61c3.14-.35 6.44-1.54 6.44-7A5.44 5.44 0 0 0 20 4.77 5.07 5.07 0 0 0 19.91 1S18.73.65 16 2.48a13.38 13.38 0 0 0-7 0C6.27.65 5.09 1 5.09 1A5.07 5.07 0 0 0 5 4.77a5.44 5.44 0 0 0-1.5 3.78c0 5.42 3.3 6.61 6.44 7A3.37 3.37 0 0 0 9 18.13V22" />
            </svg>
          </span>
          <input
            className={styles.input}
            type="url"
            placeholder="https://github.com/username/repository"
            value={url}
            onChange={e => setUrl(e.target.value)}
            disabled={loading}
            required
          />
        </div>

        <button className={styles.branchToggle} type="button" onClick={() => {if (branch === 'main') {setBranch('Master')} else {setBranch('main')}}}>
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                <line x1="6" y1="3" x2="6" y2="15" />
                <circle cx="18" cy="6" r="3" />
                <circle cx="6" cy="18" r="3" />
                <path d="M18 9a9 9 0 0 1-9 9" />
            </svg>
            {branch || 'Branch'}
        </button>

        <button className={styles.submit} type="submit" disabled={loading || !url.trim()}>
          {loading ? (
            <span className={styles.spinner} />
          ) : (
            <>
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                <circle cx="11" cy="11" r="8" /><line x1="21" y1="21" x2="16.65" y2="16.65" />
              </svg>
              Analyze
            </>
          )}
        </button>
      </div>

      {showBranch && (
        <div className={styles.branchRow}>
          <input
            className={styles.branchInput}
            type="text"
            placeholder="Branch name (default: repo default)"
            value={branch}
            onChange={e => setBranch(e.target.value)}
            disabled={loading}
          />
        </div>
      )}

      {loading && (
        <p className={styles.loadingHint}>
          Cloning repository and analyzing routes — this may take 30–60 seconds…
        </p>
      )}
    </form>
  );
}