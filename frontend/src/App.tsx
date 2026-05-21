import { useState, useEffect } from 'react';
import posthog from 'posthog-js';
import { Navbar } from './components/Navbar';
import { HomePage } from './pages/HomePage';
import { AboutPage } from './pages/AboutPage';
import { HowToUsePage } from './pages/HowToUsePage';

export type Page = 'home' | 'about' | 'how-to-use';

export default function App() {
  const [page, setPage] = useState<Page>('home');

  useEffect(() => {
    posthog.capture('$pageview', { page });
  }, [page]);

  return (
    <>
      <Navbar currentPage={page} onNavigate={setPage} />
      {page === 'home' && <HomePage />}
      {page === 'about' && <AboutPage />}
      {page === 'how-to-use' && <HowToUsePage />}
    </>
  );
}
