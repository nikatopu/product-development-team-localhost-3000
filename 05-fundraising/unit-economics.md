# Driftless Unit Economics

## Purpose

This document is the fundraising-ready unit economics summary for Driftless. It reconciles the existing GTM, financial model, discovery, and validation materials into one consistent set of numbers for the PDFSE final checkpoint.

Driftless is currently a free MVP for ASP.NET/.NET API teams. The fundraising model keeps the current free product position while presenting planned SaaS monetization through Free, Pro, and Team tiers.

## Product And Customer Context

Driftless automates API documentation generation for ASP.NET/.NET repositories. It reduces API documentation drift, backend/frontend miscommunication, and rework for API-heavy teams.

Primary ICP:

- Small API-heavy teams of 2-6 developers
- Backend, frontend, full-stack, and mobile developers integrating APIs
- Teams without strict API governance or reliable documentation automation
- Initial wedge: ASP.NET/.NET API teams
- Expansion path: FastAPI, Express.js, Django, Spring Boot, and broader backend/API frameworks

Validated pain:

- 6 discovery interviews
- 5/6 showed strong pain
- Target developers lose 4-16 hours per developer per week due to documentation mismatch, communication loops, and rework
- 10 validation participants
- 8/10 experienced the pain
- 8/10 found Driftless useful
- 7/10 were willing to try Driftless
- 70% willingness-to-try signal
- 1 non-binding design partner MOU with at least 2 feedback sessions committed

## Pricing Model

| Tier | Price | Intended customer | Notes |
| --- | ---: | --- | --- |
| Free | $0/month | MVP users, students, validation users | Current MVP position; limited scans and basic generated documentation |
| Pro | $15/month | Individual developers and high-intent users | Planned paid tier; unlimited scans, exports, integrations, priority support |
| Team | $50/month | Small API-heavy teams | Planned higher tier; collaboration, CI/CD integration, workflow tools |

Consistency note: the live MVP is free today. The fundraising model assumes paid monetization is introduced later through Pro and Team plans. The blended ARPU used across this fundraising package is $15/month, which is consistent with the existing unit economics and workbook assumptions.

## Core Revenue Assumptions

| Metric | Value | Formula / rationale |
| --- | ---: | --- |
| ARPU | $15/month | Blended paid-user revenue assumption |
| Customer lifetime | 8 months | Existing unit economics and workbook assumption |
| LTV | $120 | $15 ARPU x 8 months |
| Primary paid tier | Pro at $15/month | Matches ARPU assumption |
| Higher paid tier | Team at $50/month | Future team monetization option |

The existing workbook contains a separate 5% monthly churn scenario. This fundraising package does not use that churn assumption for LTV because it conflicts with the 8-month lifetime model requested for consistency. The active fundraising model is:

LTV = ARPU x Customer Lifetime = $15 x 8 = $120.

## CAC By Channel

| Channel | Role | Spend / basis | Acquired users | Paying customers | CAC | LTV:CAC | Payback period |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| GitHub Developer Outreach | Primary GTM channel | $300 content and maintenance over 6 months | 60 | 9 | $5 | 24:1 | 0.33 months |
| LinkedIn B2B Outreach | Secondary monetization channel | $800 outreach tools and founder time | 40 | 12 | $20 | 6:1 | 1.33 months |
| University / Community | Secondary validation channel | $200 demos, events, and community effort | 80 | 5 | $2.50 | 48:1 | 0.17 months |

Formulas:

- CAC = acquisition spend / acquired users
- LTV:CAC = $120 LTV / CAC
- Payback period = CAC / $15 monthly ARPU

## 12-Month Projection

This package uses the expected 12-month model from `04-gtm/financials/driftless-growth-model.xlsx`.

| Month 12 metric | Value |
| --- | ---: |
| Total users | 1,290 |
| Paid users | 206 |
| MRR | $3,090 |
| Monthly marketing cost | $1,330 |
| Monthly operating cost | $565 |
| Total monthly costs | $1,895 |
| Monthly profit | $1,195 |

Formula check:

- MRR = 206 paid users x $15 ARPU = $3,090
- Monthly profit = $3,090 MRR - $1,895 monthly costs = $1,195

## Strategic Interpretation

Strong signals:

- Low acquisition costs in founder-accessible channels
- Fast payback periods under the current ARPU assumption
- Clear primary channel: GitHub Developer Outreach
- Early willingness-to-try signal: 70%
- Existing design partner MOU

Current risks:

- Monetization is planned, not yet validated with real paid users
- The current wedge is intentionally narrow: ASP.NET/.NET API teams
- Scaling beyond founder-led channels may increase CAC
- Documentation accuracy must remain high to preserve trust
- Gross margin is not yet validated because production infrastructure cost per scan/user has not been measured

## Assumptions Register

| Assumption | Value used | Basis |
| --- | ---: | --- |
| ARPU | $15/month | Existing unit economics and workbook |
| Customer lifetime | 8 months | Existing unit economics and workbook |
| LTV | $120 | $15 x 8 months |
| Primary CAC | $5 GitHub CAC | Existing unit economics; best primary channel |
| Secondary CACs | $2.50 University, $20 LinkedIn | Existing unit economics |
| Current MVP price | $0 | Current frontend structured data and MVP status |
| Planned paid tiers | Pro $15/month, Team $50/month | Existing unit economics and workbook |
| Month 12 paid users | 206 | Existing expected 12-month workbook model |
| Month 12 MRR | $3,090 | Existing expected 12-month workbook model |
| Month 12 monthly profit | $1,195 | Existing expected 12-month workbook model |

## Readiness Conclusion

Driftless has a coherent early-stage unit economics story if framed carefully: free MVP today, planned paid tiers later, $15 ARPU, 8-month lifetime, $120 LTV, and low-cost founder-led acquisition channels. The strongest fundraising-ready channel is GitHub Developer Outreach because it matches the product workflow and has the lowest primary-channel CAC.

