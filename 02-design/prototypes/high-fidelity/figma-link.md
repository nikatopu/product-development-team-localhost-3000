# High-Fidelity Prototype

**Team:** localhost:3000  
**Product:** Driftless  
**Date:** May 2026  
**Version:** 1.0  
**Owner:** Nikoloz Bujiashvili (Design Lead)

---

## Prototype Link

> **Stitch prototype:** [Driftless — High-Fidelity Prototype](https://stitch.withgoogle.com/share/driftless-hifi-prototype)

> **Note:** If the Stitch link above expires, the equivalent Figma export is at `02-design/prototypes/high-fidelity/driftless-hifi-export.fig`

---

## What the Prototype Covers

The prototype covers the complete core user flow end to end, navigable without explanation:

1. **Landing screen** — Hero section with product description, a GitHub URL input field with branch toggle, and an "Analyze" button.
2. **Loading state** — Spinner with a progress hint ("Cloning repository — this may take 30–60 seconds…").
3. **Results — Routes tab** — Metadata bar showing project name, route count, controller count, and detected framework. Filter bar with text search and HTTP method buttons (ALL, GET, POST, PUT, DELETE). Scrollable list of route cards, each showing method badge, route path, controller name, and expandable request/response schema tables.
4. **Results — TypeScript tab** — Generated TypeScript type definitions in a code panel with a copy-to-clipboard button.
5. **Error state** — Inline error message below the input when the repository URL is invalid or the backend returns an error.
6. **About screen** — Product description, problem statement, and team information.
7. **How to Use screen** — Step-by-step instructions with annotated screenshots.

---

## Prototype Tool

**Tool used:** Google Stitch  
**Rationale:** Stitch was used to scaffold the UI screens from acceptance-criteria-driven prompts. All output was reviewed against acceptance criteria before use. Design was manually adjusted to match the working frontend implementation.

---

## Connection to Activation Event

The activation moment in this prototype is the user seeing the first route card rendered on screen after analysis completes. This corresponds to the `analysis_completed` event in the event schema and directly drives the North Star Metric: **weekly analysis sessions per active developer**.

---

## Prototype Iteration Log

| Version | Date         | What Changed                                      | Reason                                                |
| ------- | ------------ | ------------------------------------------------- | ----------------------------------------------------- |
| v0.1    | May 5, 2026  | Initial screens generated via Stitch              | Lab 6 scaffolding                                     |
| v0.2    | May 10, 2026 | Branch toggle button relabelled to "Branch: main" | P1, P4 usability finding — button purpose was unclear |
| v0.3    | May 14, 2026 | Copy button added to TypeScript panel             | P4 usability finding — no way to copy generated types |
| v1.0    | May 18, 2026 | Final prototype reflecting all usability changes  | Sprint 1 review ready                                 |

---

_High-Fidelity Prototype | localhost:3000 | CS-PD-2026 | Spring 2026_
