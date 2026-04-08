# Pattern Analysis

**Team:** API Collaboration Study  
**Date:** April 8, 2026  
**Based on:** 6 interviews

---

## Frequency Table

| Rank | Pattern Name             | Interviews Mentioning | Percentage | Pain Intensity (1-5) | Combined Score |
| ---- | ------------------------ | --------------------- | ---------- | -------------------- | -------------- |
| 1    | Documentation Mismatch   | 5 / 6                 | 83%        | 5                    | 4.15           |
| 2    | Rework & Debugging Loops | 5 / 6                 | 83%        | 4                    | 3.32           |
| 3    | Missing API Contracts    | 4 / 6                 | 66%        | 5                    | 3.3            |
| 4    | Manual API Validation    | 4 / 6                 | 66%        | 3                    | 1.98           |
| 5    | Communication Dependency | 4 / 6                 | 66%        | 3                    | 1.98           |

---

## Pattern 1: Documentation Mismatch (Rank #1)

**Frequency:** 5 of 6  
**Pain Intensity:** 5 / 5

**Quotes:**

- "Actual API behavior doesn’t match documentation." (VG)
- "Behavior differed from documentation." (BC)
- "Docs were incorrect." (SM)

---

### Five Whys

**Surface problem:** Documentation does not match real API behavior

**Why #1:** APIs behave differently than documented (VG, BC)  
**Why #2:** Documentation is manually written (AD, SM)  
**Why #3:** It is not updated after changes (AD)  
**Why #4:** No automation ensures synchronization (BC, LP)  
**Why #5:** Documentation is not integrated into development workflow

**Root cause:**  
Documentation is not systemically tied to implementation.

---

## Pattern 2: Rework & Debugging Loops (Rank #2)

**Frequency:** 5 of 6  
**Pain Intensity:** 4 / 5

**Quotes:**

- "Back-and-forth debugging." (VG)
- "Costs hours or days." (VG)
- "Feels repetitive." (AD)

---

### Five Whys

**Surface problem:** Developers repeatedly fix integration issues

**Why #1:** Expectations do not match behavior (VG)  
**Why #2:** API understanding is incorrect (BC)  
**Why #3:** Documentation is unreliable (AD)  
**Why #4:** Issues are discovered during implementation (SM)  
**Why #5:** No validation at contract/design level

**Root cause:**  
Errors are detected too late in the development cycle.

---

## Pattern 3: Missing API Contracts (Rank #3)

**Frequency:** 4 of 6  
**Pain Intensity:** 5 / 5

**Quotes:**

- "I don’t know routes or request types." (AD)
- "Blocked until backend writes documentation." (SM)

---

### Five Whys

**Surface problem:** Developers cannot start work due to missing API definitions

**Why #1:** API specs are not available early (AD)  
**Why #2:** Backend defines APIs during implementation (SM)  
**Why #3:** No shared contract exists beforehand (AD)  
**Why #4:** Teams rely on informal communication (SM)  
**Why #5:** No standardized API design process

**Root cause:**  
API contracts are not defined upfront as part of development workflow.

---

## Current Workarounds

| Workaround              | Who Uses It | Effectiveness | Limitation              |
| ----------------------- | ----------- | ------------- | ----------------------- |
| Postman testing         | SM, BC, LP  | High          | Time-consuming, manual  |
| Asking backend directly | AD, SM      | Medium        | Depends on availability |
| Reading source code     | GG          | Medium        | Not scalable            |
| Iterative debugging     | VG, AD      | Low           | Very costly             |

---

## Cross-Pattern Analysis

- Pattern 1 drives Pattern 2 directly
- Pattern 3 causes Pattern 5 (communication dependency)
- Pattern 4 (manual validation) compensates for all failures

**Key insight:**  
A reliable, real-time API contract system would eliminate multiple high-impact patterns simultaneously.

---
