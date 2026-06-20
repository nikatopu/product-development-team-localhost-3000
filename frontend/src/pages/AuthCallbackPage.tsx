import { useEffect } from 'react';
import { storeTokensFromCallback } from '../contexts/AuthContext';

interface Props {
  onDone: () => void;
}

export function AuthCallbackPage({ onDone }: Props) {
  useEffect(() => {
    storeTokensFromCallback();
    // Give AuthProvider time to re-initialize, then redirect home
    const timer = setTimeout(onDone, 100);
    return () => clearTimeout(timer);
  }, [onDone]);

  return (
    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', color: '#7d8590', fontFamily: 'system-ui' }}>
      Signing you in…
    </div>
  );
}
