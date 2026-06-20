import { useState, useEffect } from 'react';
import posthog from 'posthog-js';
import { AuthProvider } from './contexts/AuthContext';
import { Navbar } from './components/Navbar';
import { HomePage } from './pages/HomePage';
import { AboutPage } from './pages/AboutPage';
import { HowToUsePage } from './pages/HowToUsePage';
import { RepositoriesPage } from './pages/RepositoriesPage';
import { DashboardPage } from './pages/DashboardPage';
import { NotificationsPage } from './pages/NotificationsPage';
import { AuthCallbackPage } from './pages/AuthCallbackPage';

export type Page = 'home' | 'about' | 'how-to-use' | 'repos' | 'dashboard' | 'notifications' | 'auth-callback';

const PATH_TO_PAGE: Record<string, Page> = {
  '/': 'home',
  '/repos': 'repos',
  '/dashboard': 'dashboard',
  '/notifications': 'notifications',
  '/auth/callback': 'auth-callback',
};

function detectInitialPage(): Page {
  return PATH_TO_PAGE[window.location.pathname] ?? 'home';
}

function AppInner() {
  const [page, setPage] = useState<Page>(detectInitialPage);

  useEffect(() => {
    posthog.capture('$pageview', { page });
  }, [page]);

  const navigateTo = (p: Page) => {
    setPage(p);
    const pathMap: Record<Page, string> = {
      'home': '/',
      'about': '/',
      'how-to-use': '/',
      'repos': '/repos',
      'dashboard': '/dashboard',
      'notifications': '/notifications',
      'auth-callback': '/auth/callback',
    };
    window.history.pushState({}, '', pathMap[p] ?? '/');
  };

  return (
    <>
      <Navbar currentPage={page} onNavigate={navigateTo} />
      {page === 'home'          && <HomePage />}
      {page === 'about'         && <AboutPage />}
      {page === 'how-to-use'    && <HowToUsePage />}
      {page === 'repos'         && <RepositoriesPage />}
      {page === 'dashboard'     && <DashboardPage onNavigate={navigateTo} />}
      {page === 'notifications' && <NotificationsPage />}
      {page === 'auth-callback' && <AuthCallbackPage onDone={() => navigateTo('home')} />}
    </>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <AppInner />
    </AuthProvider>
  );
}
