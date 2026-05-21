import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import posthog from 'posthog-js'
import './index.css'
import App from './App.tsx'

if (typeof window !== 'undefined') {
  posthog.init('phc_uR2ykKpvm9VFPUMr2iyemzvBHSwnAPBviitBaLARFJtm', {
    api_host: 'https://eu.i.posthog.com', 
    loaded: (ph) => {
      if (import.meta.env.DEV) ph.debug(); 
    }
  });
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)