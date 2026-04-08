# Final Problem Statement

**Team:** API Collaboration Study  
**Date:** April 8, 2026  
**Based on:** 6 interviews

---

## The Problem Statement

> Frontend, backend, and mobile developers working in small to mid-sized teams integrating APIs lose approximately 4 to 16 hours per week per developer due to unreliable, delayed, or inconsistent API documentation and lack of a single source of truth for API behavior. Because API specifications are not synchronized with implementation, developers are forced into repeated debugging loops, manual validation using tools like Postman, and constant back-and-forth communication, resulting in delayed feature delivery, reduced productivity, and increased frustration.

---

## Component Breakdown

### WHO

Developers (frontend, backend, mobile) working in team environments integrating APIs across services.

**Evidence:**

- Frontend developer integrating APIs (AD)
- Android developer working with APIs (SM)
- Backend engineers consuming APIs (BC, LP)
- Full-stack developers (VG, GG)

---

### WHAT

Developers cannot reliably understand API behavior from documentation or tools.

**Evidence:**

- "Actual API behavior doesn’t match documentation." (VG)
- "I don’t know request/response types until documentation is written." (AD)
- "Docs were incorrect." (SM)
- "Gap between documentation and implementation." (BC)

---

### WHEN / WHERE

- During API integration phase
- During feature development
- During debugging and issue resolution

Most severe in:

- Fast-changing systems
- Small teams with informal processes
- Asynchronous communication environments

---

### WHY (Root Cause)

API documentation and contracts are not automatically synchronized with implementation, leading to:

- outdated documentation
- missing specifications
- lack of trust in system knowledge

---

### IMPACT

**Time loss:**

- 2 to 8 hours per issue (VG, BC)
- Occurs 2–3 times per week → 4 to 16 hours lost weekly

**Productivity impact:**

- Blocked development (AD, SM)
- Delayed feature delivery

**Emotional impact:**

- "Repetitive and time wasting" (AD)
- "Frustrating" (VG)

**System impact:**

- Increased communication overhead
- Dependency on human availability

---

## Evidence Summary

| Interview | Confirms? | Key Quote                                       | Pain (1-5) |
| --------- | --------- | ----------------------------------------------- | ---------- |
| VG        | Yes       | "Several hours or days."                        | 5          |
| AD        | Yes       | "Time wasting and repetitive."                  | 5          |
| SM        | Yes       | "Docs were incorrect."                          | 5          |
| BC        | Yes       | "Gap between documentation and implementation." | 4          |
| LP        | Yes       | "API changes require rework."                   | 4          |
| AD (2)    | Yes       | "Blocked without API definitions."              | 5          |
| SM (2)    | Yes       | "Need to ask backend for details."              | 4          |
| BC (2)    | Yes       | "Manual testing needed."                        | 4          |
| GG        | Partial   | "Clear code avoids issues."                     | 2          |

**Confirmation rate:** 8+ confirmations across interviews

---

## Final Insight

> The core problem is not lack of tools or communication, but the absence of a **trusted, real-time, and authoritative source of API truth**, forcing developers to rely on manual validation and human coordination instead of system guarantees.

---

## Team Sign-Off

- [x] Nikoloz Topuridze — April 8, 2026
- [x] Toma Danelia — April 8, 2026
- [x] Giorgi Tkebuchava — April 8, 2026
