import { useState, useRef, useEffect } from "react";
import type { Page } from "../App";
import { useAuth } from "../contexts/AuthContext";
import { fetchNotifications } from "../lib/ApiClient";
import styles from "./Navbar.module.css";

interface NavbarProps {
  currentPage: Page;
  onNavigate: (page: Page) => void;
}

export function Navbar({ currentPage, onNavigate }: NavbarProps) {
  const { user, isAuthenticated, isLoading, login, logout, accessToken } =
    useAuth();
  const [unreadCount, setUnreadCount] = useState(0);
  const [showUserMenu, setShowUserMenu] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  // Poll unread notification count when authenticated
  useEffect(() => {
    if (!isAuthenticated || !accessToken) return;
    let mounted = true;

    const load = async () => {
      try {
        const { unreadCount: count } = await fetchNotifications(accessToken, {
          unreadOnly: true,
        });
        if (mounted) setUnreadCount(count);
      } catch {
        /* ignore */
      }
    };

    load();
    const interval = setInterval(load, 60_000);
    return () => {
      mounted = false;
      clearInterval(interval);
    };
  }, [isAuthenticated, accessToken]);

  // Close menu on outside click
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
        setShowUserMenu(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, []);

  return (
    <nav className={styles.nav}>
      <button
        type="button"
        className={styles.brand}
        onClick={() => onNavigate("home")}
      >
        <img
          src="/driftless-logo.svg"
          alt="Driftless logo"
          className={styles.logo}
        />
      </button>

      <div className={styles.center}>
        <button
          type="button"
          className={`${styles.link} ${currentPage === "home" ? styles.active : ""}`}
          onClick={() => onNavigate("home")}
        >
          Home
        </button>
        <button
          type="button"
          className={`${styles.link} ${currentPage === "how-to-use" ? styles.active : ""}`}
          onClick={() => onNavigate("how-to-use")}
        >
          How to Use
        </button>
        <button
          type="button"
          className={`${styles.link} ${currentPage === "about" ? styles.active : ""}`}
          onClick={() => onNavigate("about")}
        >
          About
        </button>
        {isAuthenticated && (
          <>
            <button
              type="button"
              className={`${styles.link} ${currentPage === "dashboard" ? styles.active : ""}`}
              onClick={() => onNavigate("dashboard")}
            >
              Dashboard
            </button>
            <button
              type="button"
              className={`${styles.link} ${currentPage === "repos" ? styles.active : ""}`}
              onClick={() => onNavigate("repos")}
            >
              Repositories
            </button>
          </>
        )}
      </div>

      <div className={styles.right}>
        {isLoading ? (
          <div className={styles.loadingPill} />
        ) : isAuthenticated && user ? (
          <div className={styles.authArea} ref={menuRef}>
            {/* Notification bell */}
            <button
              type="button"
              className={styles.bellBtn}
              title="Notifications"
              onClick={() => onNavigate("notifications")}
            >
              <svg
                width="16"
                height="16"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.75"
              >
                <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9" />
                <path d="M13.73 21a2 2 0 0 1-3.46 0" />
              </svg>
              {unreadCount > 0 && (
                <span className={styles.badge}>
                  {unreadCount > 9 ? "9+" : unreadCount}
                </span>
              )}
            </button>

            {/* Avatar / menu */}
            <button
              type="button"
              className={styles.avatarBtn}
              onClick={() => setShowUserMenu((v) => !v)}
            >
              <img
                src={user.avatarUrl}
                alt={user.username}
                className={styles.avatar}
                onError={(e) => {
                  (e.target as HTMLImageElement).style.display = "none";
                }}
              />
            </button>

            {showUserMenu && (
              <div className={styles.menu}>
                <div className={styles.menuHeader}>
                  <span className={styles.menuUsername}>@{user.username}</span>
                  {user.email && (
                    <span className={styles.menuEmail}>{user.email}</span>
                  )}
                </div>
                <div className={styles.menuDivider} />
                <button
                  type="button"
                  className={styles.menuItem}
                  onClick={() => {
                    setShowUserMenu(false);
                    onNavigate("repos");
                  }}
                >
                  Repositories
                </button>
                <div className={styles.menuDivider} />
                <button
                  type="button"
                  className={`${styles.menuItem} ${styles.menuItemDanger}`}
                  onClick={() => {
                    setShowUserMenu(false);
                    logout();
                  }}
                >
                  Sign out
                </button>
              </div>
            )}
          </div>
        ) : (
          <button type="button" className={styles.loginBtn} onClick={login}>
            <svg width="15" height="15" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 0C5.37 0 0 5.37 0 12c0 5.31 3.435 9.795 8.205 11.385.6.105.825-.255.825-.57 0-.285-.015-1.23-.015-2.235-3.015.555-3.795-.735-4.035-1.41-.135-.345-.72-1.41-1.23-1.695-.42-.225-1.02-.78-.015-.795.945-.015 1.62.87 1.845 1.23 1.08 1.815 2.805 1.305 3.495.99.105-.78.42-1.305.765-1.605-2.67-.3-5.46-1.335-5.46-5.925 0-1.305.465-2.385 1.23-3.225-.12-.3-.54-1.53.12-3.18 0 0 1.005-.315 3.3 1.23.96-.27 1.98-.405 3-.405s2.04.135 3 .405c2.295-1.56 3.3-1.23 3.3-1.23.66 1.65.24 2.88.12 3.18.765.84 1.23 1.905 1.23 3.225 0 4.605-2.805 5.625-5.475 5.925.435.375.81 1.095.81 2.22 0 1.605-.015 2.895-.015 3.3 0 .315.225.69.825.57A12.02 12.02 0 0 0 24 12c0-6.63-5.37-12-12-12z" />
            </svg>
            Sign in with GitHub
          </button>
        )}
      </div>
    </nav>
  );
}
