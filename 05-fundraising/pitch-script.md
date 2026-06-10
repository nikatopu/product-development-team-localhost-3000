# Driftless — Demo Day Pitch Script

**Team:** localhost:3000
**Speakers:**

- **Nikoloz Topuridze** (Tech Lead) — Opens, closes
- **Toma Danelia** (Discovery Lead) — Problem validation and solution
- **Giorgi Tkebuchava** (Program Lead) — Market and business model
- **Nikoloz Bujiashvili** (Design Lead) — Competition, moat, roadmap

**Target duration:** 6 minutes
**Structure:** Hook → Problem & Validation → Solution → Demo → Market → Business Model → Competition & Moat → Roadmap → Ask

---

## SEGMENT 1 — Nikoloz T. | Hook and Problem | 0:00–1:00

_Stand centre stage. No slides yet, or title slide only._

"Raise your hand if you have ever opened a codebase, found the API documentation, tried to call an endpoint — and the response you got back looked nothing like what the docs described."

_Pause. Let the room respond._

"That is the daily reality for frontend, backend, and mobile developers working on small teams. It does not feel dramatic. It feels like twelve minutes wasted debugging a 400 error that turns out to be a renamed field. Then another thirty minutes on a call confirming what the actual schema is. Then another hour fixing the frontend after the contract silently changed again."

"We talked to six developers across different team setups. Five of the six reported the same root cause: documentation is written by hand, it goes stale the moment the code changes, and nobody trusts it."

_Advance to Problem & Validation slide._

"Our interviewees put it plainly: 'Actual API behavior doesn't match documentation.' 'Costs hours or days.' 'Blocked until backend writes something.' Across our interviews, this pattern — documentation mismatch — appeared in five of six conversations, and every one of those five rated the pain at maximum intensity."

"The problem is not that developers skip documentation. The problem is that documentation is structurally disconnected from implementation. It drifts the moment anyone commits a change."

_Transition:_

"Toma is going to walk you through exactly what we found and what we built."

---

## SEGMENT 2 — Toma | Validation and Solution | 1:00–2:30

_Same slide. Toma takes over the validation half._

"We conducted six discovery interviews, then validated with ten participants. Here is what the data shows."

"Eight out of ten validation participants had personally experienced the pain. Eight out of ten said Driftless was useful. Seven out of ten said they would try it. That is a 70 percent willingness-to-try signal from our validation cohort — without any marketing, without a finished product, and without a single paid promotion."

"One participant said directly: 'I don't know routes or request types until documentation is written.' Another: 'Blocked until backend writes documentation.' These are not edge cases. These patterns appeared in 66 to 83 percent of our interviews."

_Advance to Solution slide._

"What we built is called Driftless."

"Driftless is a web platform that takes a public GitHub repository URL for an ASP.NET Core project, and in seconds returns: every API route, HTTP method, controller, request schema, response schema, and TypeScript type definitions — all extracted directly from the implementation using Roslyn, Microsoft's own C# compiler API."

"No annotations required. No configuration. No documentation to write or maintain. The contract is derived from the code itself, so it cannot drift."

"The core insight is simple: if documentation is generated from implementation rather than written alongside it, it is always accurate by definition."

_Transition:_

"Nikoloz is going to show you the live product, then cover the market."

---

## SEGMENT 3 — Nikoloz B. | Live Demo Cue | 2:30–3:15

_Advance to Product slide. Pick up the device or move to the demo machine._

"This is driftless dot nikatopu dot dev. This is the live deployed product."

_[Run the live demo here — see demo-script.md for the full step-by-step sequence.]_

"What you just saw: a public GitHub repository URL pasted in, and within seconds every API endpoint discovered, every schema generated, and TypeScript interfaces ready to copy directly into a frontend codebase. No setup. No plugin. No CI pipeline change. You paste a URL and you get a contract."

_Transition:_

"Giorgi will cover who we are building this for and what the business looks like."

---

## SEGMENT 4 — Giorgi | Market and Business Model | 3:15–4:30

_Advance to Market slide._

"The Stack Overflow 2024 Developer Survey reports that ASP.NET Core is used by 19.1 percent of professional developers. Applied to the broader backend-and-API-relevant developer population, that gives us a serviceable market of approximately 2.8 million developers — worth $516 million per year at our pricing."

"Our year-one target is conservative: 1,290 total users and 206 paying customers by month twelve. At $15 per month per paying user, that is $3,090 MRR — enough to cover costs and show a $1,195 monthly profit by the end of year one."

"The expansion path is already validated. Six out of ten validation participants explicitly requested support for Express, FastAPI, Django, and Spring Boot. The TAM — backend developers who experience documentation drift across all frameworks — is 15 million developers and a $2.7 billion annual revenue opportunity."

_Advance to Business Model slide._

"The model is freemium. Free tier today to drive adoption, with two planned paid tiers: Pro at $15 per month for individual developers who need unlimited scans, exports, and integrations; Team at $50 per month for small teams who need collaboration and CI/CD integration."

"Our lowest-cost acquisition channel is GitHub Developer Outreach: $5 CAC, 24:1 LTV to CAC ratio, and a 0.33-month payback period. The product lives on GitHub. The users live on GitHub. The acquisition is nearly zero-friction."

"We have a signed design partner MOU with Saba Usanetashvili, an industry developer who completed a pilot period and committed to two feedback sessions. This is our first external validation in a real working environment."

_Transition:_

"Nikoloz is going to cover the competitive landscape and why this is defensible."

---

## SEGMENT 5 — Nikoloz B. | Competition, Moat, Roadmap | 4:30–5:45

_Advance to Competition & Moat slide._

"The tools developers use today — Swagger, Postman, NSwag, Stoplight, Readme — all share the same structural limitation: they require a human to write or maintain the documentation. Swagger requires annotations in code. Postman requires manual collection maintenance. Stoplight requires design-first workflows that most small teams do not have."

"None of them offer what Driftless offers: zero-annotation, zero-configuration, automatic contract extraction directly from the ASP.NET Core implementation. That is a genuinely different position in the market."

"And once a team adopts Driftless, the moat kicks in. The Helmer power here is Switching Costs."

"The moment a frontend team starts consuming TypeScript interfaces generated by Driftless, those types get imported into their codebase — component interfaces, API call wrappers, validation schemas, all built on Driftless output. When the backend changes, the developer runs Driftless again and gets the updated contract. The workflow becomes load-bearing."

"Switching away means migrating every generated type, re-establishing a manual documentation process, and training the team on a new tool that requires annotations or configuration they did not previously need. The cost of switching grows with every week of adoption."

"A second, compounding advantage is our ASP.NET Core specialization. Roslyn-based extraction for C# is technically deep. Building the same level of accuracy for a new entrant would require months of compiler integration work — work that benefits from production edge cases we have already encountered."

_Advance to Roadmap slide._

"We are three sprints in. Today's product handles public ASP.NET Core repositories with zero configuration. The next two sprints add private repository support, CI/CD integration, and breaking change detection — which means Driftless becomes part of every pull request review. After that: multi-framework support starting with FastAPI and Express, which immediately triples our SAM."

_Transition:_

"Back to Nikoloz to close."

---

## SEGMENT 6 — Nikoloz T. | Ask and Close | 5:45–6:00

_Advance to Ask slide._

"We are at an inflection point. The core extraction engine works. The validation signal is strong. The unit economics are favourable. What we need now is capital to accelerate the roadmap before the window closes."

"We are raising $25,000 at a pre-money valuation of $100,000. That funds twelve months of cloud infrastructure and compliance, multi-framework development — starting with FastAPI and Express — and institutional outreach to three university development programs as our first paid pilots."

"The milestone: 500 paying users and $7,500 MRR by month eighteen. Driftless. driftless dot nikatopu dot dev. Thank you."

---

## Timing Reference

| Segment | Speaker    | Content                    | Target time |
| ------- | ---------- | -------------------------- | ----------- |
| 1       | Nikoloz T. | Hook and problem           | 0:00–1:00   |
| 2       | Toma       | Validation and solution    | 1:00–2:30   |
| 3       | Nikoloz B. | Live demo cue              | 2:30–3:15   |
| 4       | Giorgi     | Market and business model  | 3:15–4:30   |
| 5       | Nikoloz B. | Competition, moat, roadmap | 4:30–5:45   |
| 6       | Nikoloz T. | Ask and close              | 5:45–6:00   |

---

## Transition Lines Reference

| From                    | To          | Line                                                                               |
| ----------------------- | ----------- | ---------------------------------------------------------------------------------- |
| Nikoloz T. → Toma       | Validation (same slide) | "Toma is going to walk you through exactly what we found and what we built."       |
| Toma → Nikoloz B.       | Demo                    | "Nikoloz is going to show you the live product, then cover the market."            |
| Nikoloz B. → Giorgi     | Market                  | "Giorgi will cover who we are building this for and what the business looks like." |
| Giorgi → Nikoloz B.     | Competition & Moat      | "Nikoloz is going to cover the competitive landscape and why this is defensible."  |
| Nikoloz B. → Nikoloz T. | Close                   | "Back to Nikoloz to close."                                                        |

---

## Notes for Delivery

- The hook question is rhetorical. Do not wait for hands. Pause for 3 seconds and continue.
- When citing interview quotes, say them slowly and with conviction. They are the strongest credibility signals in the deck.
- The demo must be live on a real device. Do not use a screen recording as the primary demo. See demo-script.md for the fallback protocol.
- Every number spoken must match the slide. If you misspeak a number, correct it immediately rather than continuing.
- Q&A starts immediately after closing. See qna-preparation.md for the full question bank.

---

_Driftless | Demo Day Pitch Script | CS-PD-2026 | Spring 2026_
