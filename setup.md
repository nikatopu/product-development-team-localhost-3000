# Project Setup

This project consists of two parts:

- **Backend** — ASP.NET Core 10.0 API (`back/`)
- **Frontend** — React 19 + Vite + TypeScript (`frontend/`)

---

## Prerequisites

Before running the project, make sure you have the following installed:

| Tool                                              | Version | Purpose                                |
| ------------------------------------------------- | ------- | -------------------------------------- |
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+   | Backend runtime                        |
| [Node.js](https://nodejs.org/)                    | 18+     | Frontend tooling                       |
| [Git](https://git-scm.com/)                       | Any     | Required by the backend (LibGit2Sharp) |

Verify installations:

```bash
dotnet --version
node --version
npm --version
```

---

## Backend Setup

The backend is a .NET project — no `package.json`, just the .NET CLI.

```bash
cd back
dotnet restore
dotnet run
```

The API will be available at:

- HTTP: `http://localhost:5141`
- HTTPS: `https://localhost:7256`
- Swagger UI: `http://localhost:5141/swagger`

Other useful commands:

```bash
# Build without running
dotnet build

# Run in a specific environment
dotnet run --environment Development

# Publish for deployment
dotnet publish -c Release
```

---

## Frontend Setup

The frontend uses npm (a `package-lock.json` is committed).

```bash
cd frontend
npm install
npm run dev
```

The dev server will start at `http://localhost:5173` by default.

Other available scripts:

```bash
# Type-check and build for production
npm run build

# Preview the production build locally
npm run preview

# Lint the codebase
npm run lint
```

---

## Running Both Together

Open two terminal windows and run each service in its own:

**Terminal 1 — Backend:**

```bash
cd back
dotnet run
```

**Terminal 2 — Frontend:**

```bash
cd frontend
npm install   # only needed the first time
npm run dev
```

The frontend communicates with the backend at `http://localhost:5141`. CORS is configured on the backend to allow all origins, so no additional proxy setup is needed during development.

---

## Project Structure

```
.
├── back/           # ASP.NET Core 10 API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── ApiDocGen.csproj
│   └── Program.cs
├── frontend/       # React 19 + Vite + TypeScript
│   ├── src/
│   ├── package.json
│   └── vite.config.ts
├── 01-discovery/   # Discovery phase docs
├── Docs/           # Additional documentation
└── README.md
```
