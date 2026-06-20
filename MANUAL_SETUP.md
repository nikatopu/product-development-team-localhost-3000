# Driftless — Manual Setup Guide

Complete guide for configuring every external service required to run Driftless in development or production.

---

## Table of Contents

1. [Environment Variables Reference](#1-environment-variables-reference)
2. [PostgreSQL Setup](#2-postgresql-setup)
3. [GitHub OAuth App](#3-github-oauth-app)
4. [JWT Secret](#4-jwt-secret)
5. [Slack Notifications](#5-slack-notifications)
6. [Discord Notifications](#6-discord-notifications)
7. [Frontend Environment](#7-frontend-environment)
8. [Running Locally](#8-running-locally)
9. [Running Migrations](#9-running-migrations)

---

## 1. Environment Variables Reference

### Backend (`back/`)

Create `back/appsettings.Development.json` (gitignored):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=driftless_dev;Username=postgres;Password=postgres"
  },
  "GitHub": {
    "ClientId": "YOUR_GITHUB_OAUTH_CLIENT_ID",
    "ClientSecret": "YOUR_GITHUB_OAUTH_CLIENT_SECRET",
    "CallbackUrl": "http://localhost:5141/api/auth/github/callback"
  },
  "Jwt": {
    "Secret": "a-random-256-bit-secret-at-least-32-characters-long",
    "Issuer": "Driftless",
    "Audience": "DriftlessApp"
  },
  "Frontend": {
    "BaseUrl": "http://localhost:5173"
  }
}
```

**Graceful degradation behaviour:**

- If `ConnectionStrings:DefaultConnection` is absent → uses EF Core InMemory (all data lost on restart; auth still works)
- If `Jwt:Secret` is absent → all `/api/auth/*` and `[Authorize]` endpoints return 401; public analysis endpoints still work

### Frontend (`frontend/`)

Create `frontend/.env.local`:

```
VITE_API_URL=http://localhost:5141
VITE_POSTHOG_KEY=your_posthog_key_here
VITE_POSTHOG_HOST=https://app.posthog.com
```

---

## 2. PostgreSQL Setup

### Option A — Local PostgreSQL

1. Install PostgreSQL 15+ from https://www.postgresql.org/download/
2. Start the service and open `psql`:
   ```sql
   CREATE DATABASE driftless_dev;
   CREATE USER driftless WITH PASSWORD 'postgres';
   GRANT ALL PRIVILEGES ON DATABASE driftless_dev TO driftless;
   ```
3. Set `ConnectionStrings:DefaultConnection` to:
   ```
   Host=localhost;Database=driftless_dev;Username=driftless;Password=postgres
   ```

### Option B — Supabase (Free Cloud Postgres)

1. Sign up at https://supabase.com → Create a new project
2. Go to **Project Settings → Database → Connection string** → copy the URI
3. Switch from `postgres://` to `Host=...;Database=...;Username=...;Password=...` Npgsql format:
   - Example Supabase URI: `postgresql://postgres:[password]@db.[ref].supabase.co:5432/postgres`
   - Converted: `Host=db.[ref].supabase.co;Port=5432;Database=postgres;Username=postgres;Password=[password];SSL Mode=Require`
4. Set that as `ConnectionStrings:DefaultConnection`

### Option C — Docker

```bash
docker run -d \
  --name driftless-pg \
  -e POSTGRES_DB=driftless_dev \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -p 5432:5432 \
  postgres:16-alpine
```

---

## 3. GitHub OAuth App

1. Go to https://github.com/settings/developers → **OAuth Apps** → **New OAuth App**
2. Fill in:
   - **Application name**: `Driftless (dev)` (or your production name)
   - **Homepage URL**: `http://localhost:5173`
   - **Authorization callback URL**: `http://localhost:5141/api/auth/github/callback`
3. Click **Register application**
4. On the next page, copy **Client ID** → set as `GitHub:ClientId`
5. Click **Generate a new client secret** → copy it → set as `GitHub:ClientSecret`

**For production**, create a second OAuth App with:

- Homepage URL: `https://your-frontend.vercel.app`
- Callback URL: `https://your-backend.onrender.com/api/auth/github/callback`

---

## 4. JWT Secret

Generate a cryptographically random 256-bit secret:

```bash
# Node.js
node -e "console.log(require('crypto').randomBytes(32).toString('base64'))"

# PowerShell
[System.Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))

# OpenSSL
openssl rand -base64 32
```

Paste the output as `Jwt:Secret`. The value must be at least 32 characters.

---

## 5. Slack Notifications

Driftless sends breaking-change alerts to Slack via incoming webhooks (no bot token needed).

1. Go to https://api.slack.com/apps → **Create New App** → **From scratch**
2. Name it `Driftless`, select your workspace → **Create App**
3. In the left sidebar → **Incoming Webhooks** → toggle **Activate Incoming Webhooks** to On
4. Click **Add New Webhook to Workspace** → pick a channel → **Allow**
5. Copy the webhook URL (format: `https://hooks.slack.com/services/T.../B.../...`)
6. In the Driftless UI (authenticated): go to a repository → **Notifications settings** → paste the webhook URL into **Slack webhook**

The webhook is stored per-repository in the DB. No environment variable needed.

---

## 6. Discord Notifications

1. Open your Discord server → right-click a channel → **Edit Channel**
2. Go to **Integrations** → **Webhooks** → **New Webhook**
3. Name it `Driftless`, optionally set an avatar, then click **Copy Webhook URL**
4. In Driftless UI: same repository notification settings → paste into **Discord webhook**

Discord webhook URL format: `https://discord.com/api/webhooks/[id]/[token]`
Driftless appends `/slack` to use Discord's Slack-compatible endpoint automatically.

---

## 7. Frontend Environment

| Variable            | Required | Description                                               |
| ------------------- | -------- | --------------------------------------------------------- |
| `VITE_API_URL`      | Yes      | Backend base URL, e.g. `http://localhost:5141`            |
| `VITE_POSTHOG_KEY`  | No       | PostHog project API key for analytics                     |
| `VITE_POSTHOG_HOST` | No       | PostHog instance URL (default: `https://app.posthog.com`) |

---

## 8. Running Locally

```bash
# Terminal 1 — Backend
cd back
dotnet run

# Terminal 2 — Frontend
cd frontend
npm install
npm run dev
```

Frontend: http://localhost:5173  
Backend / Swagger: http://localhost:5141/swagger

---

## 9. Running Migrations

Migrations require a real PostgreSQL connection. Use the design-time factory (already configured).

```bash
cd back

# Set the connection string env var (PowerShell)
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Database=driftless_dev;Username=postgres;Password=postgres"

# Apply all pending migrations
dotnet ef database update

# Add a new migration (after model changes)
dotnet ef migrations add YourMigrationName
```

The app also auto-migrates on startup when a real DB connection is configured (`Program.cs` calls `db.Database.MigrateAsync()`).
