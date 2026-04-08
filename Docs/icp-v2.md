# Ideal Customer Profile

**Team Name:** localhost:3000  
**Date:** April 8, 2026  
**Version:** v2.0 (post-interview, evidence-based)

---

## Primary ICP: API Collaboration Between Developers

### 1. Segment

Frontend, backend, and mobile developers (junior to mid-level) working in small to mid-sized teams (2–6 people), including:

- professional teams (companies, agencies)
- freelance collaborations
- university project teams

These developers:

- actively build and consume APIs
- work on shared codebases
- integrate backend services into frontend/mobile applications

**Refinement from interviews:**

- Strongest pain observed in teams **without strict API standards or tooling**
- Less visible in highly structured, senior-led teams

---

### 2. Context

The problem occurs during active development when:

- backend developers create or modify API endpoints
- frontend/mobile developers integrate those endpoints
- both sides work **asynchronously or in parallel**

This typically happens:

- during feature development cycles
- when APIs are refactored or updated
- when documentation is incomplete or outdated

**Observed environments:**

- Telegram / Slack communication
- GitHub-based workflows
- Manual documentation (Swagger, text files)

---

### 3. Primary Pain

Developers do not have a **reliable, real-time source of truth** for API behavior.

As a result:

- documentation does not match actual implementation
- API specifications are missing or delayed
- developers cannot trust available information

**Evidence:**

- "Actual API behavior doesn’t match documentation."
- "I don’t know request/response types until documentation is written."
- "Docs were incorrect… only way was testing."

---

### 4. Impact (Quantified)

This leads to measurable costs:

**Time loss:**

- 2 to 8 hours per issue
- Occurs 2–3 times per week
- → **4 to 16 hours lost per developer weekly**

**Workflow impact:**

- blocked frontend development
- delayed feature delivery
- repeated debugging cycles

**Emotional impact:**

- frustration ("time wasting", "repetitive")
- uncertainty about source of errors

---

### 5. Current Workarounds

Developers rely on:

- Manual documentation (Swagger, Postman, text notes)
- Messaging (Slack, Telegram)
- Manual API testing (Postman)
- Reading source code
- Trial-and-error debugging

**Limitations:**

- documentation becomes outdated
- communication depends on availability
- testing is time-consuming
- no single reliable system exists

---

### 6. Job to Be Done

"When I am integrating or building an API in a team, I want to immediately understand how endpoints behave and stay updated when they change, so I can implement features without debugging loops or repeated clarification."

---

### 7. Success Signal

- Developers no longer rely on manual clarification
- API behavior is trusted without testing
- Integration happens without repeated debugging

**Behavioral changes:**

- fewer messages about endpoints
- reduced Postman usage for verification
- faster feature completion

---

### 8. Where to Find Them

- Your coworkers (Studio Glitch, etc.)
- Freelance collaborators
- KIU classmates working on projects
- GitHub collaborators
- Developer group chats (Telegram, Slack)

---

### 9. Why This ICP Matters

From interviews:

- 5 out of 6 participants experienced strong pain
- Problems appear consistently across:
  - frontend
  - backend
  - mobile development

**Key insight:**

> The problem is not lack of tools, but lack of **trust in API knowledge**.

---

### Evidence Basis (Updated)

| Dimension      | Confidence | Basis                           |
| -------------- | ---------- | ------------------------------- |
| Segment        | High       | Confirmed across multiple roles |
| Context        | High       | Observed in real workflows      |
| Primary Pain   | High       | Strong repeated signals         |
| Impact         | High       | Time loss explicitly mentioned  |
| Workaround     | High       | Consistent across interviews    |
| JTBD           | High       | Clearly validated               |
| Success Signal | Medium     | Inferred from behavior          |

---

### Revision History

| Version | Date           | What changed                                   | Evidence      |
| ------- | -------------- | ---------------------------------------------- | ------------- |
| v1.0    | March 20, 2026 | Initial hypothesis                             | Pre-interview |
| v2.0    | April 8, 2026  | Refined ICP, quantified impact, validated pain | 6 interviews  |

---

---

## Secondary ICP: Design Handoff Between Designers and Developers

### Status: DEPRIORITIZED (Based on Interviews)

This ICP was initially considered but is not supported by strong evidence compared to the primary problem.

### Reason

- No strong signals from interviews
- Lower pain intensity
- Less consistent across participants

### Decision

Focus resources on **API collaboration problem**, which shows:

- higher frequency
- higher impact
- clearer validation

---
