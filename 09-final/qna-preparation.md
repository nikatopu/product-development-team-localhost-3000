# Driftless — Q&A Preparation

**Team:** localhost:3000
**Event:** Demo Day, 11 June 2026
**Format:** Q&A begins immediately after closing. Expect 3–5 minutes. Judges may ask questions during the demo segment.

Every question below has been drawn from the pitch content and the known weak spots in early-stage developer tooling pitches. Every answer is grounded in actual repository evidence.

---

## How to handle Q&A

- Answer the question asked. Do not pivot to a different answer.
- If the number is in the deck, own it immediately. Do not check your notes.
- If you do not know something, say: "That is a good question and I do not have that number in front of me right now — I can follow up with you directly after the session." Do not guess.
- Objections are not attacks. Acknowledge the concern, then address it.
- Assign ownership before Demo Day: each team member is responsible for specific questions. Judges notice when the same person answers every question.

---

## Question Bank

---

### PRODUCT AND TECHNICAL

**Q: How accurate is the schema extraction? What happens if Roslyn misses an endpoint?**

Ownership: Nikoloz T. (Tech Lead)

Answer: "Roslyn is Microsoft's official C# compiler API — it has the same information the compiler has, which is complete by definition for syntactically valid code. The extraction accuracy is determined by how cleanly the repository uses standard ASP.NET Core routing patterns. Where we have seen gaps is with non-standard custom routing middleware. That is a known limitation and it is documented. The fallback today is that the results are clearly presented as discovered rather than claimed as exhaustive. Our validation participants understood and accepted this trade-off — eight out of ten rated Driftless as useful against the real-world repositories they work with."

Source reference: `03-build/architecture/` (Roslyn pipeline), `03-build/experiments/experiment-results.md`

---

**Q: What happens when the repository is private? Most real production repositories are private.**

Ownership: Nikoloz T. (Tech Lead)

Answer: "The current MVP requires a public GitHub repository URL. Private repository support requires GitHub OAuth integration and is explicitly scoped to Sprint 2. We launched with public repositories to validate the core extraction engine without authentication complexity — and the validation signal confirmed the engine is valuable. Private repository support is the first Sprint 2 deliverable. We have the design for the OAuth integration ready."

Source reference: README.md (Scope section — Out of Scope Sprint 2+)

---

**Q: Could GitHub itself build this as a native feature and make Driftless irrelevant?**

Ownership: Nikoloz T. or Nikoloz B.

Answer: "GitHub could theoretically build API extraction features. But GitHub's incentive is to provide generic developer infrastructure, not language-specific contract tooling. The Roslyn integration is deeply C#-specific and requires compiler-level understanding that is outside GitHub's core infrastructure layer. More practically: GitHub has not done this in the fifteen years ASP.NET Core has been a major framework, which suggests it is not on their roadmap. Our moat is not 'GitHub will never build this' — it is that by the time any platform-level feature ships, we have already embedded into team workflows through TypeScript type adoption, which creates switching costs that are independent of whether a competitor exists."

---

**Q: How does Driftless handle breaking change detection? That seems like the most valuable feature.**

Ownership: Nikoloz T.

Answer: "Breaking change detection is in Sprint 2, not the current MVP. The current product generates the contract on demand. Sprint 2 adds the ability to compare a new extraction against a previously stored contract and flag differences — removed fields, changed types, new required parameters — automatically. That is correct: it is one of the highest-value features, which is why it is the first major Sprint 2 addition. We scoped the MVP to validate the extraction engine first, so we are building on a proven foundation rather than building everything at once."

---

**Q: What is your approach to documentation accuracy if the codebase has complex inheritance or reflection-based routing?**

Ownership: Nikoloz T.

Answer: "Complex inheritance is a known edge case. Roslyn gives us full type resolution, so standard inheritance chains are handled correctly. Reflection-based routing — where routes are constructed at runtime rather than declared in the source — is genuinely hard for any static analysis tool and is out of scope for the current product. We document this limitation. The realistic user base for Driftless today is teams using standard ASP.NET Core routing conventions, which is the majority of the ASP.NET Core developer population."

---

### TRACTION AND VALIDATION

**Q: Your validation cohort is ten people. Is that large enough to conclude anything?**

Ownership: Toma

Answer: "Ten participants is a standard size for early-stage product validation, not a statistically significant market survey. What we can conclude from ten participants is directional: the pain is real, the product concept addresses it, and the willingness-to-try signal is strong enough to justify building. The six discovery interviews before the validation cohort were specifically structured to identify patterns rather than confirm hypotheses — and the top two patterns each appeared in 83 percent of conversations. We are not claiming this as market research. We are claiming it as enough signal to justify the MVP, which we then built and deployed."

Source reference: `01-discovery/synthesis/patterns-analysis.md`

---

**Q: You have a design partner MOU. What did that partner actually tell you?**

Ownership: Toma

Answer: "Saba Usanetashvili is an industry developer who used the product during a pilot period from 22 to 25 May 2026 and committed to two feedback sessions. The MOU is non-binding. The feedback confirmed that the core extraction flow works on a real project and identified usability refinements for the route display. We did not receive negative feedback suggesting the product concept was wrong — the feedback was focused on UX improvements. We are transparent that this is one design partner, not a paid customer."

Source reference: `04-gtm/traction/memorandum-of-understanding.md`

---

**Q: You have no paying users. How do you know anyone will pay $15 per month?**

Ownership: Giorgi

Answer: "We do not know for certain — we are pre-revenue and we say so clearly in the deck. What we have is a 70 percent willingness-to-try signal from ten validation participants, developer tooling benchmarks that put productivity SaaS in the $10 to $30 per user per month range, and a 24:1 LTV to CAC ratio on our primary acquisition channel that gives us strong unit economics even at modest conversion rates. The first paying customers will validate or disprove the price point. If the $15 price is wrong, we will know after the first cohort. Our risk is not that the product has no value — eight out of ten people said it was useful. Our risk is conversion rate from free to paid, which we will test in Sprint 2 with a soft launch of the Pro tier."

Source reference: `05-fundraising/unit-economics.md`

---

### MARKET AND BUSINESS MODEL

**Q: Your SOM is $37,000 in year one. That is very small. Why would an investor be interested?**

Ownership: Giorgi

Answer: "The SOM is deliberately conservative — it is our founder-led, channel-validated first year with a free product and no paid marketing. We present it honestly rather than projecting an inflated number. The investment thesis is not the year-one SOM. It is the SAM: 2.86 million ASP.NET Core developers, $516 million annual revenue opportunity, with a product that already works. The year-one numbers are the proof point that the acquisition mechanics function at a low cost — $5 CAC in the GitHub channel — before we invest in scaling them."

Source reference: `05-fundraising/market-size.md`

---

**Q: The ASP.NET Core market is narrowing. Is this a good wedge or a dead end?**

Ownership: Giorgi

Answer: "ASP.NET Core is used by 19.1 percent of professional developers according to Stack Overflow's 2024 survey, which puts it among the top five backend frameworks globally. It is not narrowing — it is a large, stable professional framework. More importantly: six out of ten of our validation participants explicitly requested FastAPI, Express, and Django support. The wedge is ASP.NET Core because that is where our Roslyn extraction is technically strongest, not because the other frameworks are unimportant. The expansion path to triple our SAM is planned and validated."

Source reference: `05-fundraising/market-size.md`

---

**Q: What prevents a developer from just building this themselves in a weekend?**

Ownership: Nikoloz T.

Answer: "The appearance of simplicity is the product's greatest strength as a user experience, but the implementation is not simple. Roslyn-based ASP.NET Core extraction requires understanding the compiler API, handling multiple routing patterns, resolving generic types, parsing controller inheritance, and managing edge cases in real production repositories. Our extraction pipeline represents weeks of work and is still improving through edge cases from real repositories. The switching cost moat compounds this: even a developer who could build this would need to rebuild all the TypeScript types they have already imported into their codebase, which they will not do unless there is a compelling reason to."

---

**Q: How does this compare to just running `dotnet swagger` and publishing the OpenAPI spec?**

Ownership: Nikoloz T.

Answer: "That is a fair comparison. `dotnet swagger` with Swashbuckle generates an OpenAPI spec from annotations or configuration that a developer has to set up and maintain. Two problems: first, it requires the developer to have already configured Swashbuckle in the project — most small teams and legacy projects have not. Second, the spec is only as accurate as the annotations, and annotations go stale when developers are busy. Driftless requires zero setup in the target repository and generates the contract directly from the source code, not from annotations. For a developer integrating with someone else's repository that they do not control, Driftless is the only option."

---

### COMPETITION AND MOAT

**Q: Why can't Postman or Swagger just add this feature?**

Ownership: Nikoloz B.

Answer: "They could add a public-repository-URL-based extraction feature. But adding it means cannibalizing their existing annotation and manual-documentation workflows, which is their core product and the basis for their paid tiers. A new entrant — us — can offer zero-annotation extraction as the primary value proposition without protecting an existing annotation-based business. This is Counter-Positioning in Helmer's framework, which compounds our Switching Costs moat. The larger the established player, the harder it is for them to commit to a direction that makes their existing product look like unnecessary overhead."

---

**Q: You said the moat is Switching Costs, but switching is easy if someone just stops using Driftless.**

Ownership: Nikoloz B.

Answer: "Stopping use of Driftless is easy. But the TypeScript interfaces that were generated and imported into a frontend codebase do not disappear when you stop using Driftless. The team now has to either maintain those types manually, write an alternative extraction, or migrate to a tool that requires annotation setup they never had. The switching cost is not about logging out of Driftless — it is about what happens to the generated output that is already embedded in the codebase. The longer a team uses Driftless, the more their codebase depends on Driftless-generated types, and the higher the cost of replacement."

---

### TEAM AND EXECUTION

**Q: You are a student team. What happens to this project after the course ends?**

Ownership: Nikoloz T.

Answer: "The course deadline is Thursday. The product is deployed and live beyond the course. The technical infrastructure is on Vercel and Render, both of which run on free tiers with no shutdown date. The GitHub repository will remain public. Our plan is to continue development through Sprint 2 — private repository support and breaking change detection — which are the features most likely to drive organic adoption in the developer community. Whether this becomes a funded company depends on what the post-course traction data shows, but the product does not disappear on Thursday."

---

**Q: There are four team members. Who will maintain this if the team disperses?**

Ownership: Nikoloz T.

Answer: "Nikoloz Topuridze owns the technical architecture and will maintain the core extraction pipeline and deployment infrastructure. The codebase is public and documented. The growth strategy and unit economics are documented for any future team member or investor to audit. This is a realistic early-stage risk that we acknowledge — single-developer maintenance is common for developer tools in their first year, and the infrastructure is deliberately simple to maintain."

---

## Quick-Reference Owner Table

| Topic | Primary owner | Secondary owner |
|-------|---------------|-----------------|
| Roslyn accuracy and edge cases | Nikoloz T. | — |
| Private repository timeline | Nikoloz T. | — |
| Breaking change detection timeline | Nikoloz T. | — |
| Validation methodology | Toma | — |
| Design partner MOU details | Toma | — |
| Unit economics and pricing | Giorgi | — |
| Market size calculation | Giorgi | — |
| Competition matrix | Nikoloz B. | Nikoloz T. |
| Switching costs moat | Nikoloz B. | — |
| Post-course plans | Nikoloz T. | Giorgi |

---

## Pre-Demo-Day Drill

Run this drill 48 hours before Demo Day. One team member asks the question. The assigned owner answers without looking at this document.

- [ ] "How accurate is the extraction?" → Nikoloz T.
- [ ] "You have no paying users." → Giorgi
- [ ] "Is ten validation participants enough?" → Toma
- [ ] "What is the moat?" → Nikoloz B.
- [ ] "Why not just use dotnet swagger?" → Nikoloz T.
- [ ] "What happens after the course?" → Nikoloz T.
- [ ] "Could GitHub build this?" → Nikoloz T. or Nikoloz B.

Time each answer. Target: under 60 seconds per answer. Answers over 90 seconds will eat into Q&A time for other questions.

---

*Driftless | Q&A Preparation | CS-PD-2026 | Spring 2026*
