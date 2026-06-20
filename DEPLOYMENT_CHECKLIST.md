# Driftless — Production Deployment Checklist

---

## Infrastructure Overview

| Service | Recommended Provider | Free Tier |
|---|---|---|
| Backend (ASP.NET Core) | Render | Yes (750 hrs/mo) |
| Frontend (React/Vite) | Vercel | Yes (unlimited for hobby) |
| PostgreSQL | Supabase or Render Postgres | Yes |
| File storage | Not needed (no file uploads) | — |

---

## Pre-Deployment

### Repository
- [ ] All secrets are in `.gitignore` (`appsettings.Development.json`, `.env.local`, `.env`)
- [ ] `back/appsettings.json` contains no real credentials (only placeholders)
- [ ] EF Core migrations are committed to the repo (`back/Data/Migrations/`)
- [ ] `README.md` is complete with setup instructions

### Build verification
- [ ] `dotnet build back/` completes with 0 errors, 0 warnings
- [ ] `cd frontend && npm run build` produces `dist/` with no TypeScript errors
- [ ] `dotnet test` passes (if tests exist)

---

## Backend Deployment (Render)

### Service setup
- [ ] Create account at https://render.com
- [ ] New → **Web Service** → connect GitHub repo
- [ ] **Root directory**: `back`
- [ ] **Build command**: `dotnet publish -c Release -o out`
- [ ] **Start command**: `dotnet out/ApiDocGen.dll`
- [ ] **Environment**: `Docker` or `Native` (Render supports .NET)

### Environment variables (Render → Environment)
- [ ] `ConnectionStrings__DefaultConnection` = Supabase/Render Postgres connection string
- [ ] `GitHub__ClientId` = GitHub OAuth App Client ID (production app)
- [ ] `GitHub__ClientSecret` = GitHub OAuth App Client Secret
- [ ] `GitHub__CallbackUrl` = `https://your-backend.onrender.com/api/auth/github/callback`
- [ ] `Jwt__Secret` = 32+ character random string (see MANUAL_SETUP.md §4)
- [ ] `Jwt__Issuer` = `Driftless`
- [ ] `Jwt__Audience` = `DriftlessApp`
- [ ] `Frontend__BaseUrl` = `https://your-frontend.vercel.app`
- [ ] `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] `ASPNETCORE_URLS` = `http://0.0.0.0:$PORT` (Render sets `$PORT` automatically)

### Post-deploy
- [ ] Visit `https://your-backend.onrender.com/swagger` — 200 OK
- [ ] Visit `https://your-backend.onrender.com/api/health` — 200 OK (if health endpoint exists)
- [ ] Migrations ran on startup (check Render logs for `Applying migrations…`)

---

## Database (Supabase)

- [ ] Create Supabase project (free tier: 500 MB, 2 projects)
- [ ] Note connection string from **Settings → Database → Connection string**
- [ ] Enable **SSL required** (already default)
- [ ] Set `ConnectionStrings__DefaultConnection` on Render to the Supabase pooler URI
- [ ] Verify migrations applied: open Supabase **Table Editor** → check for `Users`, `Repositories`, etc.

---

## Frontend Deployment (Vercel)

- [ ] Create account at https://vercel.com
- [ ] Import GitHub repo → **Root directory**: `frontend`
- [ ] **Build command**: `npm run build`
- [ ] **Output directory**: `dist`
- [ ] **Framework preset**: Vite

### Environment variables (Vercel → Settings → Environment)
- [ ] `VITE_API_URL` = `https://your-backend.onrender.com`
- [ ] `VITE_POSTHOG_KEY` = PostHog project key (optional)
- [ ] `VITE_POSTHOG_HOST` = `https://app.posthog.com` (optional)

### Vercel rewrite for SPA routing
Create `frontend/vercel.json`:
```json
{
  "rewrites": [{ "source": "/(.*)", "destination": "/index.html" }]
}
```
- [ ] `vercel.json` committed and deployed

### Post-deploy
- [ ] Visit `https://your-frontend.vercel.app` — loads correctly
- [ ] Sign in with GitHub works end-to-end
- [ ] Public analysis (paste URL → analyze) works without login

---

## GitHub OAuth (Production App)

- [ ] Created a **separate** GitHub OAuth App for production (not the dev app)
- [ ] Homepage URL: `https://your-frontend.vercel.app`
- [ ] Callback URL: `https://your-backend.onrender.com/api/auth/github/callback`
- [ ] Client ID and Secret added to Render environment variables

---

## CORS Configuration

Verify `back/Program.cs` CORS policy allows the production frontend origin:
- [ ] `Frontend:BaseUrl` in env vars is set to your Vercel URL
- [ ] The CORS policy in `Program.cs` reads `Frontend:BaseUrl` (not hardcoded)
- [ ] Browser dev tools show no CORS errors after deploy

---

## Security Checklist

- [ ] `Jwt:Secret` is at least 32 random characters
- [ ] GitHub OAuth Client Secret is not committed to git
- [ ] `ASPNETCORE_ENVIRONMENT=Production` (disables detailed error pages)
- [ ] Swagger UI is disabled in production OR protected behind auth
- [ ] Rate limiting is active (`AddRateLimiter` in `Program.cs`) — 10 req/min/IP
- [ ] Database connection uses SSL (`SSL Mode=Require` in connection string)
- [ ] Refresh tokens are SHA256-hashed before DB storage (implemented in `TokenService`)
- [ ] `HttpOnly` cookies or secure localStorage for refresh tokens — review OWASP guidance

---

## Monitoring & Observability

- [ ] Render provides basic logs — review on first deploy
- [ ] Consider adding Application Insights or Serilog + Seq for structured logs
- [ ] PostHog analytics wired in frontend (`VITE_POSTHOG_KEY`)
- [ ] Set up Render **Health Check** at `/swagger/index.html` or `/api/health`
- [ ] Set up Render **Alerting** for service crashes (email/Slack)

---

## Post-Launch

- [ ] Submit to Product Hunt (see `LAUNCH_MATERIALS.md`)
- [ ] Post LinkedIn announcement
- [ ] Open GitHub Discussions for feedback
- [ ] Tag `v1.0.0` release on GitHub with release notes
- [ ] Update `README.md` with live demo link and badges (CI, license, version)
