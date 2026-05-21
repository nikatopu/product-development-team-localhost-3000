# Driftless — Automated API Documentation for ASP.NET Core

**Course:** CS-PD-2026  
**Team:** localhost:3000

**Members:**

- Nikoloz Topuridze (Tech Lead)
- Toma Danelia (Discovery Lead)
- Giorgi Tkebuchava (Program Lead)
- Nikoloz Bujiashvili (Design Lead)

---

## Problem

Frontend, backend, and mobile developers working in small to mid-sized teams lose approximately **4 to 16 hours per week per developer** due to unreliable, delayed, or inconsistent API documentation and lack of a single source of truth for API behavior.

Because API specifications are not synchronised with implementation, developers are forced into:

- Repeated debugging loops
- Manual validation using tools like Postman
- Constant back-and-forth communication

This results in:

- Slower feature delivery
- Increased development costs
- Frustration and workflow inefficiency

**Key insight:**  
The problem is not lack of documentation, but lack of **trust in API understanding**.

---

## Evidence (From Interviews)

Based on 6 interviews with developers:

- "It can cost several hours or even days."
- "Actual API behavior doesn't match documentation."
- "I don't know request/response types until documentation is written."
- "It feels repetitive and time wasting."

**Observed patterns:**

- Documentation mismatch
- Missing API contracts
- Rework loops
- Manual validation as workaround
- Dependency on human communication

---

## Solution: Driftless

Driftless is an automated API documentation generator for ASP.NET Core repositories. A developer pastes a public GitHub repository URL, Driftless scans the .NET codebase using Roslyn to extract all API routes, HTTP methods, request/response schemas, and generates TypeScript type definitions — with no manual configuration required.

---

## Target Users

- Backend developers building APIs in ASP.NET Core
- Frontend developers integrating with .NET APIs
- Full-stack developers working across both layers
- Small to mid-sized teams without strict API governance
- Student teams and freelance developers

---

## Scope (Sprint 1 MVP)

### In Scope

- Public GitHub repository URL input
- Roslyn-based ASP.NET Core route and schema extraction
- HTTP method, path, controller, and request/response schema detection
- TypeScript interface generation from response models
- JSON route output and OpenAPI-compatible generation
- Results rendered in a web dashboard (routes tab + TypeScript tab)
- Usage analytics via PostHog

### Out of Scope (Sprint 2+)

- Authentication and private repository support
- Multi-framework support (Express, FastAPI, Django, Spring Boot)
- Persistent storage and result caching
- CI/CD pipeline integration
- Export-to-file and streaming progress

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | React 19 + Vite + TypeScript |
| Backend | ASP.NET Core 10.0 (C#) |
| Code Analysis | Microsoft.CodeAnalysis.CSharp (Roslyn) |
| Git Operations | LibGit2Sharp 0.30 |
| Hosting | Vercel (frontend) + Render (backend) |
| Analytics | PostHog (posthog-js, free tier) |

---

## Status

- [x] Interviews completed
- [x] Affinity mapping completed
- [x] Pattern analysis completed
- [x] Problem statement finalised
- [x] High-fidelity prototype completed and usability-tested (5 participants)
- [x] Solution validated (70% adoption threshold exceeded, 10 participants)
- [x] MVP built and deployed
- [x] Analytics instrumented (PostHog)
- [x] Growth strategy, unit economics, and financial model complete
- [x] Design partner MOU signed

---

## Deliverables

### Deployment
- **Live Application:** https://driftless.nikatopu.dev/
- **Analytics Dashboard:** [03-build/analytics/dashboard-link.md](03-build/analytics/dashboard-link.md) (PostHog — shared with instructor via email)

### Design
- **Prototype:** [02-design/prototypes/high-fidelity/figma-link.md](02-design/prototypes/high-fidelity/figma-link.md)
- **Usability Testing:** [02-design/user-testing/usability-findings.md](02-design/user-testing/usability-findings.md)

### Architecture
- **System Design:** [03-build/architecture/system-design.md](03-build/architecture/system-design.md)
- **Tech Stack:** [03-build/architecture/tech-stack.md](03-build/architecture/tech-stack.md)
- **Architecture Diagram:** [03-build/architecture/architecture-diagram.svg](03-build/architecture/architecture-diagram.svg)

### Experiment and MVP Evidence
- **Experiment Results:** [03-build/experiments/experiment-results.md](03-build/experiments/experiment-results.md)

### Growth and Financials
- **Growth Strategy:** [04-gtm/growth-strategy.md](04-gtm/growth-strategy.md)
- **Unit Economics:** [04-gtm/financials/unit-economics.md](04-gtm/financials/unit-economics.md)
- **Growth Model:** [04-gtm/financials/driftless-growth-model.xlsx](04-gtm/financials/driftless-growth-model.xlsx)
- **Traction Evidence (MOU):** [04-gtm/traction/memorandum-of-understanding.md](04-gtm/traction/memorandum-of-understanding.md)

### Process Documentation
- **AI Usage Log:** [Docs/ai-usage-log.md](Docs/ai-usage-log.md)
- **Standup Log:** [Docs/standup-log.md](Docs/standup-log.md)
