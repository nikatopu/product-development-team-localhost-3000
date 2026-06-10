# Traction and Analytics Documentation

**Team Name:** localhost:3000  
**Product Name:** Driftless  
**Date:** June 4, 2026  
**Version:** 1.0  

---

## 1. Traction Overview

Driftless is an automated API documentation generator designed to solve the problem of "API drift" in cross-functional developer teams. Since launching our MVP at [driftless.nikatopu.dev](https://driftless.nikatopu.dev/), we have validated the product across both qualitative and quantitative cohorts.

### Key Traction Highlights
* **Active User Base:** 17 active developer sessions registered over our initial two-week launch window.
* **Problem Validation:** 80% of interviewed developers (8 out of 10) confirmed experiencing severe communication issues and type mismatches during API integration.
* **Adoption Willingness:** 70% of tested users (7 out of 10) expressed a clear intent to adopt or recommend Driftless for their team workflows.
* **Design Partner Signed:** One formal, non-binding Memorandum of Understanding (MOU) executed with an active external developer to pilot the product in a real-world project team.

---

## 2. Quantitative Analytics (PostHog Integration)

We use **PostHog** (free tier) as our primary product analytics platform to capture user actions in real time. This ensures we are measuring the product's actual utility rather than static page views.

### Live Metrics (As of June 4, 2026)
Below is the status of our core operational events captured by PostHog:

| Event Name | Trigger | Lifetime Total Count | Notes / Insights |
|------------|---------|----------------------|------------------|
| `page_view` | User visits home page | 20 | High initial interest from community channels. |
| `analysis_completed` | Roslyn backend returns successfully | 10 | Indicates users are actively testing the analyzer against real public repos. |
| `copy_button_clicked` | User copies generated TS code | 9 | 90% of completed analyses resulted in a code copy action (strong utility signal). |
| `analysis_error` | Repositories fail to parse | 8 | Mostly caused by non-ASP.NET repos or invalid branch names. |

### Activation Rate Analysis
Our defined North Star Metric is **weekly analysis sessions per active developer**. Our primary activation event is `analysis_completed` followed by `copy_button_clicked` (proving code generation value).
* **Calculated Activation Rate:** Of the 20 unique users who initiated an analysis session, 9 successfully copied generated TypeScript definitions. This represents a **45% Activation Rate** within our early-stage cohort.

---

## 3. Qualitative Validation & Usability Testing

We conducted five structured usability tests (P1 to P5) with developers outside our team. 

* **The Core Finding:** The two highest-friction design gaps identified were (1) the low discoverability of the branch selector button, and (2) the absence of a "Copy to Clipboard" button in the TypeScript code panel.
* **The Product Iteration:** In response to these findings, we upgraded our high-fidelity prototype (v0.2 to v1.0) and our deployed code to include an explicit "Branch: main" label and a copy-to-clipboard button. These iterations resulted in our current copy-to-clipboard activation rate of 44.6%.
* Full details are documented in [02-design/user-testing/usability-findings.md](../../02-design/user-testing/usability-findings.md).

---

## 4. Design Partner Pilot

We have established a formal, non-binding relationship with an external developer to validate the product inside an active development workflow.

* **Design Partner:** Saba Usanetashvili (Active developer and student)
* **Agreement Type:** Non-binding Memorandum of Understanding (MOU)
* **Pilot Window:** May 22, 2026 to May 25, 2026
* **Commitment:** Saba agreed to utilize Driftless to generate contracts between their ASP.NET backend and frontend consumers and provide two detailed feedback logs.
* Full MOU documentation is located in [04-gtm/traction/memorandum-of-understanding.md](../traction/memorandum-of-understanding.md).

---

## 5. Next Steps for Growth (Sprint 2+)

To build on our early traction, we have prioritized three growth initiatives:
1. **GitHub Developer Outreach:** Targeting open-source ASP.NET repositories on GitHub to offer automated documentation pull requests, establishing organic credibility.
2. **University Communities:** Distributing Driftless to KIU student project groups working on web development assignments to solve their coordination friction.
3. **MOU Expansion:** Expanding our design partner cohort from one pilot to three active small-team partners by the start of the next release cycle.