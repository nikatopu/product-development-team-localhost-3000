# Driftless Market Size

## Purpose

This document provides a transparent TAM/SAM/SOM estimate for Driftless using existing repository evidence and conservative public-market assumptions. The market size is directional and should be treated as an early fundraising checkpoint estimate, not a fully validated investor-grade market study.

## ICP

Driftless starts with ASP.NET/.NET API teams that lose time because API documentation does not stay synchronized with implementation.

Primary ICP:

- Small API-heavy teams of 2-6 developers
- Backend, frontend, full-stack, and mobile developers integrating APIs
- Teams using ASP.NET/.NET APIs today
- Teams with informal API communication, stale documentation, or missing request/response contracts

Pain evidence from the repository:

- 6 discovery interviews
- 5/6 showed strong pain
- 4-16 hours lost per developer per week from API documentation mismatch, miscommunication, and rework
- 10 validation participants
- 8/10 experienced the pain
- 8/10 found Driftless useful
- 7/10 were willing to try Driftless, a 70% willingness-to-try signal
- 1 non-binding design partner MOU with at least 2 committed feedback sessions

## Source Inputs

Public sources used:

- Stack Overflow 2024 Developer Survey, Technology section: C# was used by 28.8% of professional developers; ASP.NET Core by 19.1% of professional developers; .NET by 27.1% of professional developers. Source: https://survey.stackoverflow.co/2024/technology/
- GitHub Octoverse / GitHub public platform context: GitHub is a large global developer platform, and Driftless's initial GTM relies on GitHub repository workflows. Source: https://octoverse.github.com/

Repository inputs used:

- ARPU: $15/month
- Annual revenue per paid user: $180/year
- Current wedge: ASP.NET/.NET API teams
- Expansion path: FastAPI, Express.js, Django, Spring Boot, and broader API/backend frameworks
- 12-month expected model: 1,290 total users, 206 paid users, $3,090 MRR, $1,195 monthly profit

## Methodology

Because exact public counts for "developers on small API-heavy teams with documentation drift" are not directly available, this estimate uses a transparent bottom-up proxy:

1. Start with a broad developer-platform population.
2. Estimate the subset doing backend/API work.
3. Apply ASP.NET Core/.NET usage share for the current serviceable market.
4. Use the existing 12-month projection as the realistic early obtainable market.

All assumptions below are deliberately labeled as estimates.

## TAM: Broader API / Backend Documentation Automation Opportunity

Definition: developers who work on backend/API software and could benefit from automated API documentation or contract synchronization, regardless of framework.

Formula:

TAM developers = 150,000,000 GitHub-scale developers x 10% estimated backend/API documentation-relevant developers

TAM developers = 15,000,000

Revenue proxy:

TAM revenue = 15,000,000 developers x $180 annual revenue per paid user

TAM revenue = $2.7B annual revenue opportunity

Interpretation:

This is not a claim that Driftless can capture $2.7B. It is a directional estimate of the broad developer documentation automation opportunity if Driftless expands beyond .NET into major backend/API frameworks.

## SAM: ASP.NET/.NET API Teams

Definition: the portion of TAM aligned with Driftless's current wedge: ASP.NET Core/.NET API teams.

Public proxy:

Stack Overflow's 2024 Developer Survey reports ASP.NET Core usage among 19.1% of professional developers and .NET usage among 27.1% of professional developers. To avoid double counting, this model uses the lower ASP.NET Core figure for the current API-team wedge.

Formula:

SAM developers = 15,000,000 TAM developers x 19.1% ASP.NET Core professional developer share

SAM developers = 2,865,000

Revenue proxy:

SAM revenue = 2,865,000 developers x $180 annual revenue per paid user

SAM revenue = $515.7M annual revenue opportunity

Interpretation:

The SAM is focused but still meaningful because ASP.NET Core is a major professional web/API framework. Driftless starts here because the current product already analyzes ASP.NET/.NET repositories using Roslyn.

## SOM: Realistic Early Reachable Market

Definition: the market Driftless can realistically reach through the existing first-year channels: GitHub Developer Outreach, university/community validation, and LinkedIn B2B outreach.

The SOM is anchored to the existing expected 12-month projection rather than a new market-share assumption.

Formula:

Year-1 reachable users = 1,290 total users

Year-1 paid users = 206 paid users

Month-12 MRR = 206 paid users x $15 ARPU = $3,090

Annualized run-rate revenue at month 12 = $3,090 x 12 = $37,080

SOM revenue proxy = $37,080 annualized run-rate revenue

Interpretation:

The year-1 SOM is intentionally conservative. It reflects founder-led acquisition, a narrow .NET wedge, and planned monetization that has not yet been validated with real paid users.

## TAM / SAM / SOM Summary

| Market layer | Definition | User estimate | Revenue proxy |
| --- | --- | ---: | ---: |
| TAM | Broader backend/API documentation automation opportunity | 15,000,000 developers | $2.7B/year |
| SAM | ASP.NET Core/.NET API-team wedge | 2,865,000 developers | $515.7M/year |
| SOM | Expected year-1 reachable market from current GTM model | 1,290 total users, 206 paid users | $37,080 annualized run-rate |

## Expansion Path

Driftless should start narrow and expand in layers:

1. ASP.NET/.NET repositories: current MVP wedge and strongest technical fit.
2. GitHub-based developer workflows: product-led acquisition through repository analysis.
3. Additional backend frameworks: FastAPI, Express.js, Django, and Spring Boot, requested by 6/10 validation participants.
4. Team workflows: collaboration, exports, CI/CD integration, private repositories, and API-change monitoring.

Why the initial .NET wedge is focused:

- The product already works on ASP.NET/.NET repositories.
- Roslyn enables accurate source-code analysis for C# projects.
- A narrow wedge makes early documentation accuracy easier to validate.
- Existing validation shows the pain is real before broadening scope.

Why the wedge is expandable:

- The underlying pain is framework-agnostic: documentation drift, missing API contracts, and repeated clarification.
- Validation participants explicitly requested broader framework support.
- The GTM channel starts with GitHub repositories, which supports multi-framework expansion later.

## Assumptions And Limitations

| Assumption | Value | Why it is used |
| --- | ---: | --- |
| Backend/API-relevant share of broad developer population | 10% | Conservative proxy because exact public counts are unavailable |
| ASP.NET Core share | 19.1% | Stack Overflow 2024 professional developer usage share |
| Annual revenue per paid user | $180 | $15 ARPU x 12 months |
| Year-1 SOM paid users | 206 | Existing 12-month expected model |
| Year-1 SOM MRR | $3,090 | Existing 12-month expected model |

Limitations:

- Stack Overflow survey data is self-reported and survey-based.
- GitHub-scale developer population includes hobbyists, students, and non-commercial users.
- The 10% backend/API-relevant assumption should be replaced with stronger market research later.
- Pricing willingness is not yet validated through paid conversions.
- Market size should be updated after real usage, conversion, and retention data are collected.

## Conclusion

Driftless has a credible wedge-first market story: start with ASP.NET/.NET API teams where the product is technically strongest, prove demand through low-cost GitHub and community channels, then expand into broader backend/API frameworks after accuracy and workflow value are validated.

