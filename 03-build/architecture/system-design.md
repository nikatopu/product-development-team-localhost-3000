# System Design

**Team:** localhost:3000  
**Product:** Driftless  
**Date:** May 2026  
**Version:** 1.0  
**Primary author:** Nikoloz Topuridze (Tech Lead)

---

## 1. Core Sprint 1 Request

```text
A developer pastes a public GitHub repository URL into Driftless, the system analyzes
the ASP.NET Core backend code, and the developer sees all extracted API routes and
generated TypeScript types on screen within 60 seconds.
```

**Current Sprint 1 boundary:**

- In scope: URL input, repository cloning, C# code analysis, route extraction, TypeScript generation, OpenAPI generation, Markdown generation, JSON route output, results display (routes tab, TypeScript tab), error handling, loading states
- Out of scope: authentication, persistent storage, user accounts, streaming analysis progress, CI/CD integration, multi-framework support (Express, FastAPI, Django, Spring Boot), export-to-file, notifications

---

## 2. System Goal

By Sprint 1 review, Driftless must support one complete analysis flow on a publicly accessible URL. A developer must be able to paste a GitHub repository URL, trigger analysis, and see extracted API routes (with HTTP method, path, controller, and request/response schemas) alongside generated TypeScript type definitions. The system is stateless: no data is persisted between requests, and no user account is required. The design prioritises correctness of extraction, speed of implementation, and demo reliability over completeness of framework coverage.

---

## 3. Component Breakdown

| Component              | Layer          | Responsibility                                                                                                                                              | Owner      | Technology                              | AI touchpoint, if any                                                                                                             |
| ---------------------- | -------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------- | --------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------- |
| Web dashboard          | Client         | Renders URL input form, loading state, route cards, TypeScript panel, About and How-to-Use screens                                                          | Nikoloz B. | React 19 + Vite + TypeScript            | Stitch used to scaffold initial UI screens from AC prompts; Copilot used for inline completion of component code                  |
| HTTP API               | Server         | Exposes REST endpoints for markdown, OpenAPI, TypeScript, and JSON route outputs; validates requests; orchestrates service calls                            | Nikoloz T. | ASP.NET Core 10.0 (C#)                  | Claude Code used for multi-file service orchestration logic in DocumentationController; output reviewed and modified before merge |
| Git service            | Integration    | Clones a public GitHub repository to a temporary local directory; cleans up after each request                                                              | Nikoloz T. | LibGit2Sharp 0.30                       | No AI at runtime                                                                                                                  |
| Analysis service       | Processing     | Walks the cloned repository for C# files; uses Roslyn to parse syntax trees; extracts controller classes, route attributes, action methods, and model types | Nikoloz T. | Microsoft.CodeAnalysis.CSharp (Roslyn)  | Claude Code used to write initial Roslyn parsing logic; all output read line by line and tested locally before use                |
| Documentation service  | Generation     | Converts the analysis result into the requested output format: Markdown, OpenAPI YAML, TypeScript interfaces, or JSON                                       | Nikoloz T. | Pure C# string construction             | Claude Code assisted with OpenAPI schema serialisation; Copilot used for TypeScript template generation                           |
| Temporary file storage | Ephemeral data | Stores cloned repositories on local disk during a single request lifecycle; deleted by the git service in the finally block                                 | Nikoloz T. | OS temp directory (Path.GetTempPath())  | No AI                                                                                                                             |
| Shared contracts       | Cross-cutting  | TypeScript interface definitions mirroring the backend response shapes; used by the frontend to type API responses                                          | Nikoloz T. | TypeScript module (shared/contracts.ts) | No AI                                                                                                                             |

---

## 4. Key Data Objects

| Entity                    | What it represents                                                                                                          | Created by                 | Read by                                          | Stored where                                       |
| ------------------------- | --------------------------------------------------------------------------------------------------------------------------- | -------------------------- | ------------------------------------------------ | -------------------------------------------------- |
| Repository URL + branch   | User-supplied input identifying the target GitHub repository and target branch                                              | URL form (frontend)        | HTTP API controller                              | Not stored — passed as query param or request body |
| AnalysisResult            | Internal aggregate of all extracted routes and model types from the Roslyn pass                                             | AnalysisService            | DocumentationService, API controllers            | In-memory only, per-request lifetime               |
| RouteInfo                 | One extracted API route: HTTP method, path, controller name, action name, optional summary, request schema, response schema | AnalysisService            | DocumentationService, frontend route cards       | In-memory; returned as JSON in API response        |
| DocumentationResult       | Wrapper around the generated output (Markdown, OpenAPI, TypeScript, or JSON string) plus project metadata                   | DocumentationService       | API controller response, frontend                | Returned in HTTP response body                     |
| EndpointInfo / SchemaInfo | TypeScript-typed shapes matching RouteInfo on the frontend                                                                  | shared/contracts.ts        | Frontend components (RouteCard, TypeScriptPanel) | Client-side memory                                 |
| BreakingChange            | Tracks property-level API contract changes between two analysis results                                                     | Analysis pipeline (future) | Frontend diff view (future)                      | Not used in Sprint 1                               |

---

## 5. User Request Lifecycle

1. Developer opens the Driftless web app at the public URL.
2. Developer enters a public GitHub repository URL and optionally specifies a branch name via the branch toggle.
3. Developer clicks "Analyze." The frontend fires two parallel POST requests: `POST /api/documentation/json/routes` and `POST /api/documentation/json/typescript`.
4. A loading spinner and progress hint ("Cloning repository — this may take 30–60 seconds…") are displayed.
5. The ASP.NET Core API receives each request, validates that `RepoUrl` is present, and calls `IGitService.CloneRepositoryAsync(repoUrl, branch)`.
6. LibGit2Sharp clones the repository to a new temp directory (e.g., `%TEMP%/driftless-abc123/`).
7. `IAnalysisService.AnalyzeRepositoryAsync(localPath, repoUrl)` walks C# files, uses Roslyn to parse syntax trees, and returns an `AnalysisResult` with all extracted routes and schemas.
8. `IDocumentationService.GenerateRoutesJson(analysis)` and `GenerateTypeScriptJson(analysis)` produce the two output payloads.
9. The `finally` block calls `IGitService.Cleanup(localPath)`, deleting the cloned temp directory regardless of success or failure.
10. Both API responses arrive at the frontend; results are parsed and stored in React state.
11. The results view renders: a metadata bar, tab switcher, method filter buttons, and the list of route cards.
12. Developer clicks the TypeScript tab to view generated type interfaces.
13. Developer uses the copy button to copy TypeScript output to the clipboard.

---

## 6. Data Flow Notes

- **Data that enters from the user:** GitHub repository URL (string), branch name (string, optional)
- **Data that is validated:** URL must be non-empty (frontend form validation + backend guard). Branch name is optional and defaults to the repository's default branch. No authentication token validation in Sprint 1 (public repos only).
- **Data that is stored permanently:** Nothing. Driftless is fully stateless in Sprint 1. No database, no user session, no persistent cache.
- **Data that is temporary or computed:** Cloned repository files on local disk (deleted after each request); `AnalysisResult` in-memory object; serialized JSON response.
- **Data that should never be stored:** Repository source code, user-supplied URLs, any developer identity data.

---

## 7. APIs and Integrations

| Service or API        | Why it exists                                                                                 | Request direction                     | Risk                                                                                    | Fallback plan                                                                     |
| --------------------- | --------------------------------------------------------------------------------------------- | ------------------------------------- | --------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- |
| GitHub (git clone)    | Source repositories are hosted on GitHub; LibGit2Sharp uses the git protocol to clone them    | Backend → GitHub (outbound)           | GitHub rate-limiting for unauthenticated clones; large repos exceed 60-second UX target | Use seeded local repo mirror for Sprint 1 demo; add GitHub token auth in Sprint 2 |
| Swagger UI (built-in) | Auto-generated interactive API docs at `/swagger`; used for backend development and debugging | Browser → Backend (inbound)           | None — development tool only                                                            | Not a dependency for the user-facing flow                                         |
| PostHog (posthog-js)  | Captures frontend usage events (analysis sessions, route card views, TypeScript tab clicks)   | Browser → PostHog cloud (outbound)    | Free-tier dashboard is not publicly shareable; dashboard access shared with instructor via email | Dashboard invite sent to instructor; event data still captured regardless of share status |

No payment or authentication services are used in Sprint 1.

---

## 8. Deployment Topology

- Frontend hosted on: Vercel; `http://localhost:5173` for local development
- Backend hosted on: Render; `http://localhost:5141` for local development
- Database hosted on: Not applicable — no database in Sprint 1
- Domain or public URL: https://driftless.nikatopu.dev/
- Analytics platform: PostHog (posthog.com, free tier) — events captured from frontend via posthog-js; dashboard shared with instructor
- Auth provider: Not applicable in Sprint 1
- File storage: OS temp directory (`Path.GetTempPath()`), ephemeral per request

---

## 9. AI in the Build and AI in the Product

### AI in the build workflow

| Tool             | Used for what                                                                                                         | Owner                  | Review rule                                                                                                   |
| ---------------- | --------------------------------------------------------------------------------------------------------------------- | ---------------------- | ------------------------------------------------------------------------------------------------------------- |
| Google Stitch    | Scaffold initial UI screens for URL form, route results, TypeScript panel, About, and How-to-Use                      | Nikoloz B.             | Each screen reviewed against acceptance criteria before use; layout manually adjusted to match implementation |
| Claude Code      | DocumentationController orchestration, Roslyn-based analysis logic, OpenAPI serialization, TypeScript output template | Nikoloz T.             | Human read line by line before merge; locally tested with real ASP.NET repo; PR review required               |
| GitHub Copilot   | Frontend component boilerplate (RouteCard, MethodBadge, PropertiesTable), React hook stubs, CSS modules               | Nikoloz B., Nikoloz T. | No suggestion accepted without reviewing logic; inline comments added to AI-assisted blocks                   |
| Google AI Studio | Not used in Sprint 1 build                                                                                            | —                      | Reserved for future AI-feature experimentation                                                                |

### AI in the product

No runtime AI feature is present in Sprint 1. Driftless uses static code analysis (Roslyn) and string-based generation. AI is used only in the build workflow.

---

## 10. Security, Privacy, and Reliability Basics

- **Auth risks:** No authentication in Sprint 1. Any user can submit any public GitHub URL. Risk of abuse (high-frequency requests, very large repos) is accepted for the demo phase.
- **Sensitive data handled:** None. No user data is collected or stored. Repository source code is cloned to temp and immediately deleted.
- **Failure mode if main service goes down:** Analysis is unavailable; frontend shows an error state. No degraded mode is implemented in Sprint 1.
- **Logging and monitoring plan for Sprint 1:** ASP.NET Core default console logging captures exceptions. Swagger UI at `/swagger` allows manual API testing. PostHog captures frontend usage events (analysis sessions, errors, tab interactions); dashboard shared with instructor.
- **One thing we will not promise yet:** Support for private repositories, repositories requiring authentication tokens, or frameworks other than ASP.NET Core.

---

## 11. Technical Risks and Spikes

1. **Risk:** Roslyn analysis may miss controllers using non-standard route attribute patterns or minimal APIs
   - Why it matters: Incomplete extraction would make the product unreliable for real teams, breaking the core trust promise
   - Mitigation or spike: Test against a set of diverse seeded ASP.NET repos before Sprint 1 review; document which patterns are supported
   - Owner: Nikoloz Topuridze

2. **Risk:** Large repositories (>500 files) may exceed the 60-second UX target due to clone + parse time
   - Why it matters: Users abandon and retry when loading takes too long (P4 usability finding)
   - Mitigation or spike: Time the analysis against several real-world repos; add file-size guard that returns a clear error if the repo exceeds a safe threshold
   - Owner: Nikoloz Topuridze

3. **Risk:** LibGit2Sharp temp directory cleanup may fail on Windows if the cloned files are locked
   - Why it matters: Disk fills up silently; backend crashes on subsequent requests
   - Mitigation or spike: Wrap `Cleanup` in a try-catch that logs the failure; add a scheduled cleanup sweep for temp directories older than 1 hour
   - Owner: Nikoloz Topuridze

---

## 12. Open Questions

- Should the frontend use two parallel API calls (routes + typescript) or one combined endpoint to reduce round trips?
- Do we need a GitHub personal access token field in Sprint 1 to support private repositories, or is public-only sufficient for the demo?
- Should analysis results be cached for a configurable TTL to reduce repeated clone-and-parse overhead for the same repo?

---

## 13. Final Readiness Check

- [x] Every component has one clear job
- [x] Core request lifecycle is written end to end
- [x] Stack in this file matches `tech-stack.md`
- [x] Top technical risks are named
- [x] Out of scope items are explicit
- [x] Another developer could start work from this document

---

_System Design | localhost:3000 | CS-PD-2026 | Spring 2026_
