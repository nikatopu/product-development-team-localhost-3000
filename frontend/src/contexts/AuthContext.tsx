import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from 'react';
import type { User } from '../types/api';

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5141';

interface AuthState {
  user: User | null;
  accessToken: string | null;
  isLoading: boolean;
  isAuthenticated: boolean;
}

interface AuthContextValue extends AuthState {
  login: () => void;
  logout: () => Promise<void>;
  refreshAuth: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>({
    user: null,
    accessToken: null,
    isLoading: true,
    isAuthenticated: false,
  });

  const fetchUser = useCallback(async (token: string): Promise<User | null> => {
    try {
      const res = await fetch(`${API_URL}/api/auth/me`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      if (!res.ok) return null;
      return res.json();
    } catch {
      return null;
    }
  }, []);

  const refreshAuth = useCallback(async () => {
    const refreshToken = localStorage.getItem('driftless_refresh_token');
    if (!refreshToken) {
      setState(s => ({ ...s, isLoading: false }));
      return;
    }

    try {
      const res = await fetch(`${API_URL}/api/auth/refresh`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ refreshToken }),
      });

      if (!res.ok) {
        localStorage.removeItem('driftless_refresh_token');
        setState({ user: null, accessToken: null, isLoading: false, isAuthenticated: false });
        return;
      }

      const { accessToken, refreshToken: newRefresh } = await res.json();
      localStorage.setItem('driftless_refresh_token', newRefresh);

      const user = await fetchUser(accessToken);
      setState({ user, accessToken, isLoading: false, isAuthenticated: !!user });
    } catch {
      setState({ user: null, accessToken: null, isLoading: false, isAuthenticated: false });
    }
  }, [fetchUser]);

  useEffect(() => {
    refreshAuth();
  }, [refreshAuth]);

  const login = () => {
    window.location.href = `${API_URL}/api/auth/github/login`;
  };

  const logout = async () => {
    const refreshToken = localStorage.getItem('driftless_refresh_token');
    if (refreshToken && state.accessToken) {
      await fetch(`${API_URL}/api/auth/logout`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${state.accessToken}`,
        },
        body: JSON.stringify({ refreshToken }),
      }).catch(() => {});
    }
    localStorage.removeItem('driftless_refresh_token');
    setState({ user: null, accessToken: null, isLoading: false, isAuthenticated: false });
  };

  return (
    <AuthContext.Provider value={{ ...state, login, logout, refreshAuth }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}

export function storeTokensFromCallback() {
  const params = new URLSearchParams(window.location.search);
  const accessToken = params.get('access_token');
  const refreshToken = params.get('refresh_token');
  if (refreshToken) localStorage.setItem('driftless_refresh_token', refreshToken);
  return { accessToken, refreshToken };
}
