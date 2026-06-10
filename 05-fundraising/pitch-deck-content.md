# Driftless — Pitch Deck Content

**Team:** localhost:3000
**Product:** Driftless
**Live URL:** https://driftless.nikatopu.dev/
**Version:** Demo Day — 11 June 2026

This document contains the full content for all 10 slides. Export to PDF using Google Slides, Figma, or Canva. Commit the exported file as `05-fundraising/pitch-deck.pdf`.

Design principle: one idea per slide. Minimal text. Every number sourced from the repository. No placeholder content in the final export.

---

## Slide 1 — Cover

**Objective:** Establish identity and signal clarity before a word is spoken.

**Content:**

```
DRIFTLESS

API contracts that never go stale.

driftless.nikatopu.dev
```

**Visual recommendation:**
Dark background (near-black or deep navy). Product name in large, clean sans-serif. Tagline in a lighter weight below. Live URL as the third and smallest element. No logo clutter. No team names on this slide — that is Slide 9. Optional: a subtle animated or static illustration of two code blocks in sync, or a simple broken-link motif.

**Speaker notes:**
No speech on this slide. It appears while the team walks to position. Nikoloz T. opens with the hook question without referring to the slide. Transition to Slide 2 immediately after the hook.

---

## Slide 2 — Problem & Validation

**Objective:** Make judges feel the cost of API drift and immediately prove the pain is real.

**Content:**

```
API documentation is written by hand.
Code changes. Docs don't.

Developers lose 4–16 hours per week chasing the gap.

"Actual API behavior doesn't match documentation."
"Costs hours or days."
"Blocked until backend writes documentation."
— Interview participants (01-discovery/interview-logs/)

6 discovery interviews  |  10 validation participants  |  70% willing to try

Documentation Mismatch      — 83% of interviews, pain 5/5
Rework and Debugging Loops  — 83% of interviews, pain 4/5
Missing API Contracts       — 66% of interviews, pain 5/5
```

**Visual recommendation:**
Left half: code-commit timeline with a broken documentation icon diverging from it; three quotes in blockquote style beneath. Right half: three large stat callouts ("6 interviews", "10 validators", "70% willing to try") stacked above a ranked bar list of the three patterns. A thin vertical rule divides the two halves. Source reference small at the bottom.

**Speaker notes:**
Nikoloz T. opens with the hook and delivers the problem half; Toma takes over for the validation half. Key line: "Documentation Mismatch appeared in 5 of 6 interviews at maximum pain intensity — that is what the numbers confirm." If asked for sources: discovery interviews April 2026, validation cohort May 2026, both in `01-discovery/`. The design partner MOU with Saba Usanetashvili (20 May 2026) is the tangible external commitment that closes this slide.

---

## Slide 3 — Solution

**Objective:** State in one sentence what Driftless does and for whom.

**Content:**

```
DRIFTLESS

Paste a GitHub repository URL.
Get every API contract, route, and TypeScript type
extracted directly from the implementation.

No annotations. No configuration. No drift.

Built for ASP.NET Core teams.
Powered by Roslyn — Microsoft's C# compiler API.
```

**Visual recommendation:**
Left side: simple input box with a GitHub URL typed in. Arrow pointing right. Right side: a compact code block showing TypeScript interface output — a real snippet from the actual product output. Use the real output, not a fabricated example. Two-column layout. Product name as a header.

**Speaker notes:**
Toma delivers this slide. The phrase "extracted directly from the implementation" is the key differentiator — say it clearly and slowly. If judges ask what Roslyn is: "Roslyn is Microsoft's official C# compiler API. We use it to parse the AST — the abstract syntax tree — of the codebase, which gives us the same information the compiler has. That is why our output is accurate." Do not go deeper in the pitch. That detail belongs in Q&A.

---

## Slide 4 — Product Demo

**Objective:** Show the live deployed product on a real device completing the core flow.

**Content:**

```
[SCREENSHOT: driftless.nikatopu.dev with real result loaded]

Caption: A public ASP.NET Core repository URL submitted.
Every endpoint discovered. TypeScript interfaces generated.
Zero configuration required.

Live: driftless.nikatopu.dev
```

**Visual recommendation:**
Full-bleed screenshot of the actual product with a real result. Annotate three elements with callout arrows: (1) the input URL field showing a real GitHub URL, (2) the routes panel showing discovered endpoints, (3) the TypeScript interface output. Live URL as a footer, large and legible.

**Speaker notes:**
This slide appears during the live demo. Nikoloz B. is at the laptop. Do not narrate from the slide — narrate from what is happening on screen. The screenshot is the fallback reference, not the primary display. See demo-script.md for the full step-by-step sequence. The slide stays on screen while the demo runs; do not advance until the demo is complete.

---

## Slide 5 — Market Size

**Objective:** Show a credible, bottom-up market opportunity with visible arithmetic.

**Content:**

```
TAM  15M developers
     Backend/API documentation automation
     $2.7B/year

SAM  2.86M developers
     ASP.NET Core professional developers
     (19.1% of professional devs — Stack Overflow 2024)
     $515.7M/year

SOM  1,290 total users | 206 paid users
     Year-1 expected model
     $3,090 MRR → $37,080 annualized run-rate

Source: Stack Overflow Developer Survey 2024 + 05-fundraising/market-size.md
```

**Visual recommendation:**
Three concentric circles or nested rectangles labelled TAM / SAM / SOM. Each circle contains the number and one-line definition. The SOM circle is the innermost and smallest — size proportional, not equal. Source reference visible at the bottom.

**Speaker notes:**
Giorgi delivers this slide. If asked how the TAM was calculated: "We used GitHub's scale as a proxy for the global developer population, applied a 10% estimate for backend and API-relevant developers, and multiplied by our $180 annual revenue per paid user. The detailed calculation is in `05-fundraising/market-size.md`." Do not claim Driftless will capture $2.7B — say "that is the addressable ceiling if we expand beyond .NET into all major backend frameworks, which six out of ten validation participants explicitly requested."

---

## Slide 6 — Business Model

**Objective:** Show how Driftless makes money and prove the unit economics are sound.

**Content:**

```
PRICING

Free      $0 / month       Limited scans, basic output
Pro       $15 / month      Unlimited scans, exports, integrations
Team      $50 / month      Collaboration, CI/CD integration

UNIT ECONOMICS (source: 05-fundraising/unit-economics.md)

ARPU                  $15 / month
Customer lifetime     8 months
LTV                   $120

Channel               CAC      LTV:CAC   Payback
GitHub outreach       $5       24:1      0.33 months
University / community $2.50   48:1      0.17 months
LinkedIn outreach     $20      6:1       1.33 months

Month-12 projection:  206 paid users | $3,090 MRR | $1,195 monthly profit
```

**Visual recommendation:**
Top: three pricing tier cards, minimal. Bottom: unit economics in a clean table. Highlight the GitHub channel row — it is the headline channel. The month-12 MRR figure should be prominent, not buried. Note "projected" near the month-12 numbers; monetization is planned, not yet active.

**Speaker notes:**
Giorgi delivers this slide. Be direct about the current state: "The product is free today. These tiers are our planned monetization, not yet activated. The 70% willingness-to-try signal from validation gives us confidence in the Pro tier at $15. The unit economics assume we activate paid tiers — at which point the GitHub channel has a 24:1 LTV to CAC ratio and a two-week payback period." Do not hide that this is pre-revenue. Frame it as: "Pre-revenue with a validated pain, a deployed product, and a clear monetization path."

---

## Slide 7 — Competition & Moat

**Objective:** Show command of the competitive landscape and name the structural advantage that makes Driftless hard to displace once adopted.

**Content:**

```
                    Auto-     ASP.NET   Real-time  Zero       TypeScript  No          Free
                    discovery  native    sync       annotation  output     annotations  tier
Swagger/Swashbuckle    ✗        ✓         ✗          ✗           ✗           ✗           ✓
Postman                ✗        ✗         ✗          ✗           ✗           ✗           ✓
NSwag                  ✗        ✓         ✗          ✗           ✓           ✗           ✓
Stoplight              ✗        ✗         ✗          ✗           ✗           ✗           ✗
Readme.com             ✗        ✗         ✗          ✓           ✗           ✓           ✗
Redocly                ✗        ✗         ✗          ✓           ✗           ✓           ✗
DRIFTLESS              ✓        ✓         ✓          ✓           ✓           ✓           ✓

MOAT — HELMER POWER: SWITCHING COSTS

Once TypeScript interfaces generated by Driftless are imported
into a frontend codebase, switching away means migrating every
generated type and rebuilding a manual process.

Switching cost grows with every week of adoption.
Roslyn-based extraction is compiler-accurate — months of work to replicate.
```

**Rows:** Swagger/Swashbuckle, Postman, NSwag, Stoplight, Readme.com, Redocly, Driftless
**Columns:** Auto-discovery, ASP.NET Core native, Real-time sync with implementation, Zero annotation required, TypeScript interface output, No developer configuration, Free entry tier

**Visual recommendation:**
Top two-thirds: clean competition grid. Driftless row highlighted in brand colour. Bottom third: a compact two-line moat section separated by a thin rule — a small diagram showing TypeScript types flowing into a frontend codebase with "switching cost" arrows pointing outward, and the Helmer label prominent. Keep the moat section tight; the argument lives in the speaker notes.

**Speaker notes:**
Nikoloz B. delivers this slide. On Swagger/Swashbuckle: "Swashbuckle generates Swagger UI from annotations developers add manually. If an endpoint changes without updating the annotation, the documentation is wrong. Driftless reads the implementation directly." On NSwag: "NSwag requires manual setup and does not provide real-time contract sync. Driftless regenerates on demand from any public repository." On the moat: "Switching Costs. Once Driftless output is woven into a team's frontend codebase as TypeScript types, the cost of removing it is the cost of replacing every generated interface. A well-funded competitor can match our features but cannot un-integrate us from teams that have already adopted the workflow." Evidence: `03-build/architecture/` (Roslyn pipeline), `01-discovery/synthesis/patterns-analysis.md` (validated pain data).

---

## Slide 8 — Roadmap

**Objective:** Show a credible near-term path that matches the ask and de-risks the investment.

**Content:**

```
SPRINT 1 (complete)
  Public ASP.NET Core repo analysis
  Route and schema extraction via Roslyn
  TypeScript interface generation
  Web dashboard deployed at driftless.nikatopu.dev
  PostHog analytics instrumented

SPRINT 2 (next)
  Private repository support (GitHub OAuth)
  Breaking change detection
  CI/CD integration (GitHub Actions hook)
  Export to file (OpenAPI JSON, TypeScript module)

SPRINT 3
  FastAPI and Express.js support
  Multi-framework dashboard
  Team collaboration features

FUNDED MILESTONE (Month 18)
  500 paying users | $7,500 MRR
  3 institutional pilots
```

**Visual recommendation:**
Horizontal timeline with three sprint blocks and one milestone flag. Sprint 1 ticked/complete. Sprint 2 and Sprint 3 as upcoming boxes. Milestone at the end clearly labelled with the funded target. Keep each sprint to three or four items maximum — judges read this at a glance.

**Speaker notes:**
Nikoloz B. or Giorgi delivers this slide. Breaking change detection in Sprint 2 is important to name explicitly — it is the feature most likely to generate adoption in team workflows because it directly answers the question "did the API I depend on change?" That is the moment of highest value for a frontend developer. Frame Sprint 3 as the market expansion that was explicitly requested by 6 of 10 validation participants.

---

## Slide 9 — Team

**Objective:** Show that this team can execute the roadmap.

**Content:**

```
LOCALHOST:3000

Nikoloz Topuridze       Tech Lead
  ASP.NET Core, Roslyn, system architecture
  Built the extraction pipeline and backend API

Toma Danelia            Discovery Lead
  6 interviews, 10 validation participants
  Authored patterns analysis and ICP framework

Giorgi Tkebuchava       Program Lead
  Growth strategy, financial model, GTM
  Owns unit economics and market size analysis

Nikoloz Bujiashvili     Design Lead
  High-fidelity prototype, usability testing
  Ran 5-participant usability study

Design partner: Saba Usanetashvili (MOU signed 20 May 2026)

driftless.nikatopu.dev
github.com/[team-repository-url]
```

**Visual recommendation:**
Four-column layout, one column per team member. Small professional photo if available. Name and role as the primary label. One-line credential beneath. No aspirational language. Design partner called out as a separate row with MOU date. Repository and live URL at the bottom.

**Speaker notes:**
No dedicated speaker for this slide — Nikoloz T. can walk through it in ten seconds: "Four of us. Nikoloz built the extraction engine. Toma ran discovery and validation. Giorgi owns the business model. Nikoloz designed the product and tested it. Saba Usanetashvili signed our design partner MOU and completed the pilot period." Do not read every credential. Judges will read the slide.

---

## Slide 10 — Ask and Close

**Objective:** State exactly what is needed, what it funds, and what milestone it enables.

**Content:**

```
WE ARE RAISING $25,000

Pre-money valuation: $100,000
Dilution: 20%

USE OF FUNDS

  Infrastructure and compliance       $8,000
  (12 months cloud hosting, security audit)

  Multi-framework development         $12,000
  (FastAPI, Express.js — 3× SAM)

  Institutional pilot program          $5,000
  (3 university development partnerships)

MILESTONE: Month 18
  500 paying users
  $7,500 MRR
  3 institutional pilots active

driftless.nikatopu.dev
[team contact email]
[GitHub repository URL]
```

**Visual recommendation:**
Clean layout. The $25,000 ask is the largest text on the slide after the title. Use of funds as a three-line table. Milestone box with the three targets, highlighted. Contact information at the bottom. The live URL appears on this slide for the third time — judges take photos of the closing slide.

**Speaker notes:**
Nikoloz T. delivers this slide. Valuation rationale if asked: "We used a straightforward early-stage multiple. Our month-12 expected MRR is $3,090. At a 3x annualized revenue multiple — conservative for a pre-revenue SaaS — the implied valuation is approximately $111,000. We rounded to $100,000 to be conservative and transparent about the stage we are at." Be honest about the stage: pre-revenue, deployed product, validated pain, clear path. Do not overclaim.

---

## Final Export Checklist

- [ ] All 10 slides have real content. No placeholder brackets remain.
- [ ] Every number matches its source document in the repository.
- [ ] Slide 4 shows the live deployed product, not a mockup.
- [ ] Competition matrix has 7 competitors and 7 dimensions (minimum rubric: 5 and 7).
- [ ] Competition & Moat slide names Switching Costs (Helmer) and cites two repository files.
- [ ] Live URL appears on slides 1, 4, and 10 at minimum.
- [ ] File exported as PDF.
- [ ] PDF named exactly `pitch-deck.pdf`.
- [ ] PDF committed to `05-fundraising/pitch-deck.pdf`.

---

*Driftless | Pitch Deck Content | CS-PD-2026 | Spring 2026*
