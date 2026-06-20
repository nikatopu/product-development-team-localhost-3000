# Driftless — Launch Materials

---

## LinkedIn Post

---

🚀 **Excited to share Driftless — open-source API contract tracking for ASP.NET Core teams.**

API documentation drift is a silent killer. Your Swagger docs say one thing; your actual controller returns another. You find out when a frontend breaks in production.

**Driftless fixes this at the source.** Instead of maintaining documentation separately, it generates your API contract directly from C# source code using Roslyn — and watches for breaking changes across every commit.

**What it does:**
→ Parses your ASP.NET Core source code (no running server needed)
→ Detects every route, controller, parameter, request body, and response type
→ Emits ready-to-use TypeScript types and fetch functions for your frontend
→ Flags breaking changes automatically: removed endpoints, changed types, deleted fields
→ Streams real-time scan progress via SignalR
→ Sends alerts to Slack or Discord the moment a breaking change lands

Connect your GitHub repo → paste the URL → get a living API contract. That's the whole flow.

Built with ASP.NET Core 10, Roslyn, EF Core, React 19, and TypeScript.

GitHub: [link]
Live demo: [link]

---

## GitHub Repository Description

**One-line description:**

> Eliminate API documentation drift — generate contracts from ASP.NET Core source with breaking-change detection.

**About section (GitHub):**

```
Driftless generates accurate API contracts directly from ASP.NET Core C# source code using Roslyn static analysis — no running server, no annotations required.

Connect a GitHub repository, run a scan, and get:
• Full endpoint inventory (routes, methods, parameters, request/response bodies)
• TypeScript interfaces and fetch functions for your frontend
• Breaking change detection across scans (removed endpoints, type changes, deleted fields)
• Real-time progress via SignalR
• Slack and Discord alerts for breaking changes

Built with: ASP.NET Core 10 · Roslyn · EF Core 9 · PostgreSQL · SignalR · React 19 · TypeScript
```

**Topics (GitHub tags):**

```
aspnet-core, dotnet, api, openapi, roslyn, static-analysis, typescript, breaking-changes, documentation, api-contracts, signalr, react, developer-tools
```

---

## Product Hunt Tagline Options

1. **API contracts from source — no drift, no stale docs**
2. **Generate TypeScript types from your C# controllers. Automatically.**
3. **Know before your frontend does: breaking API change detection for .NET teams**
4. **Stop writing API docs. Let Roslyn generate them.**

**Recommended tagline:**

> Stop writing API docs. Let your source code generate them — with breaking-change detection.

**Product Hunt description (240 chars):**

> Driftless parses ASP.NET Core controllers with Roslyn and generates accurate TypeScript types, fetch functions, and breaking-change alerts. Connect a GitHub repo → get a living API contract. No annotations needed.

---

## Demo Day Script (3 minutes)

### Opening (0:00 – 0:30)

"How many of you have had a frontend bug caused by a backend API change nobody told you about?

That's API drift — and it's the default state of most teams. The backend team ships a change, the Swagger docs fall behind, and the frontend team finds out when something explodes in production.

Driftless is a tool that makes drift impossible to miss."

### Demo (0:30 – 2:00)

1. Open Driftless. Paste a GitHub URL for an ASP.NET Core project.
2. "Watch the real-time progress bar — Driftless is cloning the repo and parsing your C# with Roslyn. No build needed."
3. Results appear: endpoint list, request/response types, TypeScript interfaces.
4. "Every interface you see here was generated from your actual controller code. Not hand-written. Not from an OpenAPI YAML file. From the source."
5. Show TypeScript panel — copy a fetch function.
6. "You can paste this directly into your React app."
7. Show breaking changes banner. "Now watch what happens when a field gets removed upstream — Driftless catches it immediately and can fire a Slack or Discord alert."

### Closing (2:00 – 2:30)

"Driftless is open-source, built on ASP.NET Core 10 and Roslyn, and designed for .NET teams that want their API contracts to be as reliable as their unit tests.

We're looking for early users, feedback, and contributors.

GitHub and the live demo are at [URL]. Thank you."

---

## Twitter/X Thread

**Tweet 1:**
Just shipped Driftless — open-source API contract generation for ASP.NET Core teams. 🧵

It parses your C# controllers with Roslyn and gives you TypeScript types + breaking-change detection. No running server. No annotations. From source.

[screenshot of endpoint list]

**Tweet 2:**
The problem it solves: API documentation drift.

Your Swagger says `string`. Your controller returns `number | null`. Your frontend finds out in prod.

Driftless makes your source code the single source of truth.

**Tweet 3:**
Here's what you get from a single scan:
→ Full route + controller inventory
→ TypeScript interfaces for every DTO
→ Ready-to-copy fetch functions
→ Diff against your last scan (breaking changes highlighted)

**Tweet 4:**
Breaking changes are alerted to Slack/Discord automatically. So the backend team can't ship a removed field without the frontend team knowing.

**Tweet 5:**
Tech stack if you're curious:
• ASP.NET Core 10 + Roslyn for parsing
• EF Core 9 + PostgreSQL for history
• SignalR for real-time progress
• React 19 + TypeScript frontend
• GitHub OAuth

Open-source: [URL]
