import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import posthog from 'posthog-js'
import './index.css'
import App from './App.tsx'

const posthogKey = import.meta.env.VITE_POSTHOG_KEY;

if (typeof window !== 'undefined' && posthogKey) {
  posthog.init(posthogKey, {
    api_host: 'https://us.i.posthog.com',
    capture_pageview: false, 
    loaded: (ph) => {
      if (import.meta.env.DEV) ph.debug();
    },
  });
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
