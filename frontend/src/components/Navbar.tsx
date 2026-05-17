import type { Page } from '../App';
import styles from './Navbar.module.css';

interface NavbarProps {
  currentPage: Page;
  onNavigate: (page: Page) => void;
}

export function Navbar({ currentPage, onNavigate }: NavbarProps) {
  return (
    <nav className={styles.nav}>
      <button type="button" className={styles.brand} onClick={() => onNavigate('home')}>
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5" strokeLinejoin="round">
          <polygon points="12,2 22,9 22,15 12,22 2,15 2,9" />
          <circle cx="12" cy="12" r="3" />
        </svg>
        <span>Driftless</span>
      </button>

      <div className={styles.links}>
        <button
          type="button"
          className={`${styles.link} ${currentPage === 'home' ? styles.active : ''}`}
          onClick={() => onNavigate('home')}
        >
          Home
        </button>
        <button
          type="button"
          className={`${styles.link} ${currentPage === 'how-to-use' ? styles.active : ''}`}
          onClick={() => onNavigate('how-to-use')}
        >
          How to Use
        </button>
        <button
          type="button"
          className={`${styles.link} ${currentPage === 'about' ? styles.active : ''}`}
          onClick={() => onNavigate('about')}
        >
          About
        </button>
      </div>
    </nav>
  );
}
