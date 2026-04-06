# Interview Log: Backend Developer Workflow

**Purpose:** Reference interview log based on real participant responses.  
**Simulates file:** `01-discovery/interview-logs/interview-03-BE-20260406.md`

---

## Participant Profile

| Field | Value |
|-------|-------|
| Pseudonym | LP |
| Role | Backend Developer (Spring Boot) |
| Relevant context | Works on backend systems, builds REST APIs, integrates databases, collaborates through pull requests and API integrations |
| How recruited | Personal network |
| Interview date | April 6, 2026 |
| Duration | ~15 minutes |
| Format | Remote (Zoom) |
| Interviewer(s) | Giorgi T. (lead) |

---

## Interview Setting

Remote interview conducted via Zoom. Participant was in a quiet environment and engaged throughout the session. Giorgi led the discussion and took notes.

---

## Verbatim Quotes

**Quote 1**  
Context: asked "Can you walk me through what a typical development workflow looks like for you?"

> "I usually start by reviewing requirements and designing the API or module. Then I set up the Spring Boot project or relevant module, write tests, implement features, and run them locally. I commit frequently, push to a branch, and open a pull request for review before merging."

What this tells us: The participant follows a structured and disciplined workflow. Strong reliance on PR reviews indicates team collaboration is central to development.

---

**Quote 2**  
Context: asked "What parts of working in a team do you find most frustrating or time-consuming?"

> "Waiting for feedback on pull requests can be slow, or when API contracts change without notice—it can require rework."

What this tells us: The main friction is not coding but coordination delays and unexpected changes. Lack of communication leads to wasted effort.

---

**Quote 3**  
Context: asked "Is there anything in your workflow that feels repetitive?"

> "Setting up similar boilerplate for new services or modules can feel repetitive, especially configuring properties, logging, and common exception handling."

What this tells us: Boilerplate setup is a recurring pain point. This suggests inefficiency that could be reduced with templates or automation.

---

**Quote 4**  
Context: asked about a recent situation where something didn’t go as planned

> "Once I started implementing a feature only to realize the database schema had changed. I had to refactor the repository layer and update tests, which set me back a day."

What this tells us: Lack of visibility into changes (like schema updates) causes rework and delays. Indicates a gap in team synchronization.

---

**Quote 5**  
Context: asked about working with APIs created by others

> "I had to integrate with an internal reporting API. First, I read the documentation, then tested endpoints in Postman. I implemented service classes in Spring Boot, handled exceptions, wrote integration tests, and then verified everything with the front-end team."

What this tells us: API integration is a structured but multi-step process involving several tools. It depends heavily on documentation quality.

---

**Quote 6**  
Context: asked about tools used for API collaboration

> "We mainly use OpenAPI/Swagger documentation, along with Postman collections. Sometimes we also use Confluence pages for additional explanations."

What this tells us: Multiple tools are used instead of a single source of truth, increasing the chance of inconsistencies.

---

**Quote 7**  
Context: asked "How well does that work for you?"

> "Swagger works well for endpoints and parameters, but it doesn’t always show examples of expected behavior or edge cases. Postman helps, but keeping collections up to date can be tricky."

What this tells us: Existing tools are useful but incomplete. Missing examples and outdated collections create friction and debugging overhead.

---

## Key Observations

- Participant showed most frustration when discussing delays and unexpected changes  
- Strong emphasis on clean architecture and maintainability  
- Repeated mention of communication gaps across teams  
- Workflow is solid, but external dependencies (APIs, reviews) introduce inefficiencies  

---

## Preliminary Insights

- The core problem is communication and coordination, not technical ability  
- Developers lose time due to unclear or changing API contracts  
- Boilerplate setup represents unnecessary repeated effort  
- Documentation tools lack real-world usage clarity (examples, edge cases)  

---

## JTBD Frame

> "The participant is hiring structured workflows (testing, PR reviews, documentation tools) to ensure reliable backend development. The functional job is delivering features correctly. The emotional job is avoiding unexpected breakages and rework. The social job is being perceived as a reliable and professional team member."

---

## Open Questions

- How frequently do API changes cause rework across teams?  
- Would automated boilerplate generation significantly reduce setup time?  
- How much time is lost weekly due to unclear documentation or delayed feedback?  
- What would an ideal API documentation tool include (examples, versioning, live testing)?  

---

## ICP Fit Assessment

- [x] Strong fit -- Backend developer actively working with APIs, team collaboration, and integration workflows  

---

## Interviewer Reflection

Most important learning: The biggest pain point is uncertainty—caused by delayed feedback, unclear documentation, and unexpected changes.

What to do differently: Ask more quantifiable follow-ups (e.g., exact time lost due to these issues) and dig deeper into how often these problems occur.

---

**Committed by:** giorgi-t  
**Date:** 2026-04-06
