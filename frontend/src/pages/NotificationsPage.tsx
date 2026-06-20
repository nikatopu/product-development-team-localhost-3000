import { useState, useEffect, useCallback } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { fetchNotifications, markNotificationRead, markAllNotificationsRead } from '../lib/ApiClient';
import type { AppNotification } from '../types/api';
import styles from './NotificationsPage.module.css';

const TYPE_ICON: Record<string, string> = {
  BreakingChange: '⚠',
  ScanComplete: '✓',
  ScanFailed: '✕',
};

const TYPE_CLASS: Record<string, string> = {
  BreakingChange: 'typeWarning',
  ScanComplete: 'typeSuccess',
  ScanFailed: 'typeError',
};

export function NotificationsPage() {
  const { accessToken } = useAuth();
  const [notifications, setNotifications] = useState<AppNotification[]>([]);
  const [unreadOnly, setUnreadOnly] = useState(false);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(false);

  const load = useCallback(async (reset = false) => {
    if (!accessToken) return;
    const nextPage = reset ? 1 : page;
    try {
      const result = await fetchNotifications(accessToken, { unreadOnly, page: nextPage });
      setNotifications(prev => reset ? result.notifications : [...prev, ...result.notifications]);
      setHasMore(result.notifications.length === 20);
      if (reset) setPage(1);
    } catch { /* ignore */ }
    finally { setLoading(false); }
  }, [accessToken, unreadOnly, page]);

  useEffect(() => { setLoading(true); load(true); }, [unreadOnly, accessToken]); // eslint-disable-line react-hooks/exhaustive-deps

  const handleMarkRead = async (id: string) => {
    if (!accessToken) return;
    await markNotificationRead(accessToken, id);
    setNotifications(prev => prev.map(n => n.id === id ? { ...n, isRead: true } : n));
  };

  const handleMarkAll = async () => {
    if (!accessToken) return;
    await markAllNotificationsRead(accessToken);
    setNotifications(prev => prev.map(n => ({ ...n, isRead: true })));
  };

  const loadMore = () => {
    setPage(p => p + 1);
    load();
  };

  return (
    <div className={styles.page}>
      <div className={styles.header}>
        <div>
          <h1 className={styles.title}>Notifications</h1>
          <p className={styles.subtitle}>Breaking changes, scan results, and alerts.</p>
        </div>
        <div className={styles.headerActions}>
          <label className={styles.toggle}>
            <input
              type="checkbox"
              checked={unreadOnly}
              onChange={e => setUnreadOnly(e.target.checked)}
            />
            Unread only
          </label>
          <button className={styles.markAllBtn} onClick={handleMarkAll}>
            Mark all read
          </button>
        </div>
      </div>

      {loading ? (
        <div className={styles.loading}>Loading notifications…</div>
      ) : notifications.length === 0 ? (
        <div className={styles.empty}>
          <span className={styles.emptyIcon}>🔔</span>
          <h2>{unreadOnly ? 'No unread notifications' : 'No notifications yet'}</h2>
          <p>You'll see scan results and breaking change alerts here.</p>
        </div>
      ) : (
        <div className={styles.feed}>
          {notifications.map(n => (
            <div
              key={n.id}
              className={`${styles.item} ${n.isRead ? styles.itemRead : styles.itemUnread}`}
            >
              <span className={`${styles.icon} ${styles[TYPE_CLASS[n.type] ?? 'typeSuccess']}`}>
                {TYPE_ICON[n.type] ?? '•'}
              </span>
              <div className={styles.body}>
                <div className={styles.message}>{n.message}</div>
                <div className={styles.meta}>
                  {n.repositoryName && (
                    <span className={styles.repo}>{n.repositoryName}</span>
                  )}
                  <span className={styles.time}>
                    {new Date(n.createdAt).toLocaleString()}
                  </span>
                </div>
              </div>
              {!n.isRead && (
                <button
                  className={styles.readBtn}
                  onClick={() => handleMarkRead(n.id)}
                  title="Mark as read"
                >
                  ✓
                </button>
              )}
            </div>
          ))}

          {hasMore && (
            <button className={styles.loadMore} onClick={loadMore}>
              Load more
            </button>
          )}
        </div>
      )}
    </div>
  );
}
