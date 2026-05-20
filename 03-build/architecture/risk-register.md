# Risk Register

**Team:** localhost:3000  
**Product:** Driftless  
**Date:** May 2026

---

## Top Technical Risks

| Risk ID | Risk statement                                                                                                                                                            | Likelihood | Impact | Earliest detection point                                   | Mitigation or spike                                                                                                                                                             | Owner             | Status |
| ------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------- | ------ | ---------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------- | ------ |
| R1      | Roslyn analysis misses ASP.NET controllers using minimal API syntax, attribute routing variants, or non-standard project structures, producing incomplete or empty output | Medium     | High   | First demo against an unfamiliar real-world ASP.NET repo   | Curate a set of 5 diverse seeded repos (simple, minimal API, versioned routes) and run the analysis pipeline against all of them before Sprint 1 review                         | Nikoloz Topuridze | Open   |
| R2      | Repository clone time exceeds 60 seconds on the live deployment environment due to repo size, network latency, or cold-start delay, causing users to abandon or retry     | Medium     | High   | Load test during deployment setup                          | Benchmark clone + analysis time for a 50 MB and 200 MB repo; add a file-size guard returning a clear error above the threshold; improve loading UX with a time estimate message | Nikoloz Topuridze | Open   |
| R3      | LibGit2Sharp temp directory cleanup fails on Windows due to file handle locks, causing disk exhaustion on the backend host                                                | Low        | Medium | Second or third consecutive analysis request in production | Wrap `Cleanup` in try-catch with explicit error logging; add a startup task that sweeps temp directories older than 1 hour; monitor disk usage after the first 10 live requests | Nikoloz Topuridze | Open   |
| R4      | CORS misconfiguration in production prevents the frontend (Vercel URL) from reaching the backend (Render URL), breaking the demo with a network error                     | High       | High   | First end-to-end test against the deployed environment     | Set `ALLOWED_ORIGINS` environment variable on Render to the Vercel production URL before Sprint 1 review; verify with a live browser request                                    | Nikoloz Topuridze | Open   |

---

## Notes on the Top 3

### R1 — Incomplete Roslyn Extraction

**Why this matters to Sprint 1:**  
The entire value of Driftless is that its output is accurate and complete. If a common ASP.NET controller pattern is silently missed, the developer receives a misleading partial view of their API. This undermines the product's core trust claim and would fail the Sprint 1 review if the demo repo's routes are not all extracted.

**What evidence would show the risk is real:**  
Running the analysis against `tomadanelia/community-dashboard` (the example URL shown in the empty state) returns fewer routes than the actual controller count visible in the repository source.

**What you will do first:**  
Clone and manually inspect 5 repos against Roslyn output. Prioritize: repos using `[Route]` attribute, `[HttpGet]`/`[HttpPost]` on actions, and one repo using minimal APIs (`app.MapGet`). Document which patterns are supported and which are out of scope.

---

### R2 — Clone Time Exceeds UX Target

**Why this matters to Sprint 1:**  
The Sprint 1 demo requires a live analysis in front of the instructor. A 90-second wait with no visible progress caused P4 to abandon in usability testing (see usability-findings.md). The same failure in the demo would damage credibility.

**What evidence would show the risk is real:**  
Timing the analysis of 3 real repos in the production environment shows median clone + parse time above 55 seconds.

**What you will do first:**  
Instrument the two phases (clone time, analysis time) with `Stopwatch` logging. If clone time dominates, explore shallow clone (`--depth 1`) via LibGit2Sharp to reduce transfer size. If analysis time dominates, profile Roslyn walker performance on large files.

---

### R3 — Temp Directory Cleanup Failure

**Why this matters to Sprint 1:**  
The backend is stateless by design. If cleanup silently fails, the temp directory grows with each request. On a free-tier host (Render) with limited disk, this could cause the service to crash mid-demo.

**What evidence would show the risk is real:**  
After 20 consecutive analysis requests in the production environment, the temp directory tree contains more than 2 residual folders.

**What you will do first:**  
Add a try-catch around `Directory.Delete(path, recursive: true)` in `GitService.Cleanup`. Log the full path and exception on failure. Verify on a Windows development machine by running 10 consecutive requests and checking `%TEMP%` for residual directories.

---

## Spike Plan

| Spike   | Question to answer                                                                                                                    | Timebox | Owner             | Output                                                                                                        |
| ------- | ------------------------------------------------------------------------------------------------------------------------------------- | ------- | ----------------- | ------------------------------------------------------------------------------------------------------------- |
| Spike 1 | Does the Roslyn walker correctly extract all routes and schemas from 5 diverse seeded repos without missing any documented endpoints? | 3 hours | Nikoloz Topuridze | List of supported and unsupported ASP.NET routing patterns; fix or scoping decision for each unsupported case |
| Spike 2 | What is the median clone + analysis time in the Render production environment for a 50 MB, 100 MB, and 200 MB repo?                   | 2 hours | Nikoloz Topuridze | Timing table; decision on whether to add shallow-clone flag or a size guard before Sprint 1 review            |

---

_Risk Register | localhost:3000 | CS-PD-2026 | Spring 2026_
