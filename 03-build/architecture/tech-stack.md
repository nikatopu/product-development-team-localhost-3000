# Tech Stack Selection

**Team:** localhost:3000  
**Product:** Driftless  
**Date:** May 2026  
**Version:** 1.0

---

## 1. Decision Summary

The team is optimising for correctness of C# code analysis and fast Sprint 1 execution. The product's core value — accurate, automated API documentation for ASP.NET Core — dictates that the backend must run on .NET and use Roslyn, which rules out non-.NET runtimes. The frontend needs to be reactive and capable of rendering structured data cleanly; React with Vite is the team's existing competency and requires no new learning curve. The stack accepts the complexity of a two-runtime architecture (C# backend, TypeScript frontend) because no single-runtime alternative satisfies both the Roslyn dependency and the modern UI requirement.

The team is deferring persistent storage, authentication, and cloud hosting to Sprint 2. We are accepting a fully stateless, single-deployment model for Sprint 1 to reduce setup overhead and keep the demo path reliable. We are also deferring multi-framework support (Express, FastAPI, Django) until Roslyn-based .NET analysis is proven solid.

---

## 2. Stack by Layer

| Layer             | Selected technology                                                | Why this fits                                                                                                                                                              | Alternative considered                 | Why rejected                                                                                                          | Owner      |
| ----------------- | ------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------- | --------------------------------------------------------------------------------------------------------------------- | ---------- |
| Frontend          | React 19 + Vite + TypeScript                                       | Team has existing React experience; Vite provides fast HMR for development; TypeScript gives compile-time safety for the shared contracts with the backend response shapes | Next.js                                | Server-side rendering adds complexity not required by a client-rendered analysis tool; no SEO or SSR need in Sprint 1 | Nikoloz B. |
| Backend           | ASP.NET Core 10.0 (C#)                                             | Roslyn is a first-party .NET library — there is no viable equivalent outside the .NET ecosystem; C# provides the best parser for C# source code                            | Node.js + TypeScript                   | Cannot run Roslyn; would require calling a .NET subprocess, adding fragility and latency                              | Nikoloz T. |
| Code analysis     | Microsoft.CodeAnalysis.CSharp (Roslyn)                             | First-party C# compiler APIs; provides a complete, battle-tested syntax tree for any valid C# file; far more reliable than regex-based parsing                             | Regex-based parsing                    | Fragile against non-standard formatting; misses edge cases in attribute syntax; does not understand C# semantics      | Nikoloz T. |
| Git operations    | LibGit2Sharp 0.30                                                  | Pure .NET git library; no external git binary required on the host; synchronous clone API fits the request-per-analysis model                                              | Calling `git clone` as a shell command | Shell invocation creates injection risk and requires git to be installed on the host; harder to control cleanup       | Nikoloz T. |
| API documentation | Swashbuckle.AspNetCore (Swagger)                                   | Auto-generated interactive docs at `/swagger` with zero config; standard in ASP.NET Core projects; valuable for backend development and debugging                          | Manual Postman collection              | Postman collections are not auto-generated and drift from code changes                                                | Nikoloz T. |
| Frontend state    | React hooks (useState, custom useAnalysis hook)                    | Simple request-response pattern does not need a global state library; hooks are sufficient for the single-page analysis flow                                               | Redux or Zustand                       | Over-engineered for a two-state (loading / results) UI with no shared state across routes                             | Nikoloz B. |
| Styling           | CSS Modules                                                        | Scoped styles per component; no build-time dependency on a third-party UI library; team can iterate quickly on the visual language                                         | Tailwind CSS                           | Would require a build plugin and a new mental model; CSS Modules are already in the codebase                          | Nikoloz B. |
| Testing           | Manual flow test against a seeded repo                             | Sufficient for Sprint 1 to verify core extraction correctness                                                                                                              | Vitest + React Testing Library         | Setup time not justified in Sprint 1; added to Sprint 2 plan                                                          | Nikoloz T. |
| Hosting           | Vercel (frontend) + Render / Azure App Service (backend) (planned) | Vercel provides instant Next.js/Vite deploy from GitHub; Render provides free .NET hosting tier                                                                            | Railway                                | Less team familiarity; Vercel + Render combination is already understood from prior course work                       | Nikoloz T. |
| Diagramming       | Excalidraw                                                         | Fast collaborative editing; easy SVG export; no account required                                                                                                           | Lucidchart                             | No meaningful advantage for the team; requires account setup                                                          | Nikoloz B. |

---

## 3. Approved AI Tools for Sprint 1

| Tool             | Approved use                                                                                                                                                          | Not for                                                                                                    | Review rule                                                                                                                             | Owner                                             |
| ---------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------- |
| Google Stitch    | Scaffold UI screens for URL input form, route results, TypeScript panel, About, and How-to-Use                                                                        | Backend logic, analysis algorithms, API contract design                                                    | Every acceptance criterion checked item by item before accepting any generated screen; layout manually adjusted to match implementation | Nikoloz B.                                        |
| Claude Code      | Multi-file backend service logic: DocumentationController orchestration, Roslyn parsing in AnalysisService, OpenAPI and TypeScript generation in DocumentationService | Blind end-to-end generation without human reading; security-sensitive code (CORS policy, input validation) | Human read line by line; tested locally against at least one real ASP.NET Core repo; PR review before merge                             | Nikoloz T.                                        |
| GitHub Copilot   | Frontend component boilerplate (RouteCard, MethodBadge, PropertiesTable, CSS modules), React hook stubs, repetitive TypeScript patterns                               | Architecture decisions, new features, business logic                                                       | Never accept a suggestion without reading it; inline comments added to all AI-assisted code blocks explaining what the code does        | Nikoloz B., Nikoloz T.                            |
| Google AI Studio | Not used in Sprint 1                                                                                                                                                  | All Sprint 1 coding                                                                                        | —                                                                                                                                       | Reserved for Sprint 2+ AI-feature experimentation |

---

## 4. Deployment Target

- **Public deployment target:** Vercel preview URL (frontend) + Render free-tier URL (backend) — both provisioned before Sprint 1 review
- **Database region or environment:** Not applicable — no database in Sprint 1
- **How local and production differ:** Local uses `http://localhost:5141` as the API origin; production uses the Render service URL, configured via an environment variable read by the frontend at build time
- **What gets deployed first:** Backend service (Render) must be live and reachable before the frontend build can be configured with the correct API URL
- **What will stay local for now:** Seed scripts for testing, `.env.local` with development overrides

---

## 5. Rejected Architecture Paths

### Rejected Option 1

- **Option:** Single Node.js process running both the frontend and a proxy that calls a .NET subprocess for analysis
- **Why it was attractive:** Single deployment target; no cross-origin concerns
- **Why it was rejected now:** Launching .NET as a subprocess from Node introduces process lifecycle complexity, error propagation is harder to handle, and the team's C# expertise makes a native ASP.NET Core server the cleaner choice

### Rejected Option 2

- **Option:** Python backend using subprocess to call `dotnet script` for Roslyn
- **Why it was attractive:** Easier to deploy to common Python hosting platforms
- **Why it was rejected now:** Double-subprocess overhead; Python-to-dotnet interop adds fragility; team has no Python backend experience; total complexity exceeds ASP.NET Core direct approach

### Rejected Option 3

- **Option:** Full Next.js monorepo with API routes calling a serverless .NET function
- **Why it was attractive:** One repo, one deployment target (Vercel)
- **Why it was rejected now:** Vercel serverless functions have a 10-second execution limit; repository cloning alone takes 15–45 seconds for typical repos; the runtime constraint makes this approach unworkable

---

## 6. Technical Debt You Are Accepting on Purpose

| Shortcut                            | Why accepted now                                                                         | Risk created                                                            | When to revisit                                               |
| ----------------------------------- | ---------------------------------------------------------------------------------------- | ----------------------------------------------------------------------- | ------------------------------------------------------------- |
| No authentication or access control | Reduces setup complexity for Sprint 1 demo; only public repos targeted                   | Any user can submit any URL; potential for abuse if exposed publicly    | Sprint 2: add GitHub token input for private repo support     |
| No persistent storage or caching    | Stateless analysis is simpler to build and deploy                                        | Same repo analyzed multiple times causes repeated clone overhead        | Sprint 2: add result caching with configurable TTL            |
| No automated test suite             | Manual testing against a seeded repo is sufficient for Sprint 1 correctness verification | Regressions in Roslyn parsing logic may go undetected after refactoring | Sprint 2: add Vitest for frontend, xUnit for backend services |
| Public-only GitHub repos            | Covers 90% of demo and validation use cases without auth complexity                      | Cannot demo analysis of private enterprise codebases                    | Sprint 2: add GitHub personal access token field              |
| Single deployment environment       | No staging/production separation; demo and development share the same backend            | No safety net if demo breaks during Sprint Review                       | Before external pilot                                         |

---

## 7. Final Stack Lock

- **Frontend:** React 19 + TypeScript, bundled with Vite, deployed to Vercel
- **Backend:** ASP.NET Core 10.0, C#, deployed to Render
- **Code analysis:** Roslyn (Microsoft.CodeAnalysis.CSharp), running inside the ASP.NET Core process
- **Git operations:** LibGit2Sharp, running inside the ASP.NET Core process
- **Auth:** None in Sprint 1
- **Database:** None in Sprint 1
- **Analytics:** None in Sprint 1
- **Hosting:** Vercel (frontend) + Render (backend)

No TBD entries remain.

---

_Tech Stack Selection | localhost:3000 | CS-PD-2026 | Spring 2026_
