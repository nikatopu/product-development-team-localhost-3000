# Interview Log — API Collaboration Study (Participant AD)

---

## Participant Profile

| Field            | Value                                                                 |
| ---------------- | --------------------------------------------------------------------- |
| Pseudonym        | AD                                                                    |
| Role              | Frontend React Developer                                              |
| Relevant context  | Works on a streaming platform, responsible for API integration        |
| How recruited     | Internal project team (shared development environment)                |
| Interview date    | April 6–7, 2026                                                      |
| Duration          | ~20 minutes                                                           |
| Format            | Structured interview (remote chat-based)                              |
| Interviewer(s)    | Toma Danelia                                                         |

---

## Interview Setting

Participant is actively working in a 3-developer team (frontend, backend, mobile). Communication happens primarily through Telegram, with GitHub used for version control. Responses indicate real-time development context with ongoing API integration work.

---

## Verbatim Quotes

**Quote 1**  
Context: asked about current workflow

> "I am frontend react developer currently working on streaming platform on web, I have frequent communication with backend developer to ensure I implement API endpoints correctly."

**What this tells us:**  
Frontend integration is highly dependent on direct backend communication rather than stable interface definitions.

---

**Quote 2**  
Context: asked about team collaboration tools

> "We collaborate using telegram for communication and use GitHub for version control."

**What this tells us:**  
No dedicated API design or documentation layer is used; coordination is informal and chat-driven.

---

**Quote 3**  
Context: asked about most frustrating parts of workflow

> "Most time consuming part is implementing API endpoints in frontend because I don’t actually know what routes, requests and response types they have until backend developer writes documentation."

**What this tells us:**  
Primary friction point is late or incomplete API specification. Frontend work is blocked by missing contract definition.

---

**Quote 4**  
Context: asked about inefficiencies in workflow

> "Yes, spotting errors and contacting backend developer to correct them or requesting new one when he just finished building endpoint feels repetitive and time wasting."

**What this tells us:**  
There is a repeated rework loop caused by incorrect or evolving API definitions.

---

**Quote 5**  
Context: asked about a recent issue

> "Backend developer incorrectly wrote string instead of number for one parameter which backend returns as a JSON field."

**What this tells us:**  
Type mismatches between backend and frontend occur frequently, indicating weak schema enforcement.

---

**Quote 6**  
Context: asked about API usage process

> "I had to implement admin panel feature... I get documentation of involved routes from backend developer written in text format, read them and implemented."

**What this tells us:**  
API consumption relies on static, manually written documentation rather than structured or machine-readable contracts.

---

**Quote 7**  
Context: asked about documentation quality

> "It takes time for backend developer to write when something is refactored and using AI for that parts is really frustrating, it makes too many mistakes when there is not enough context given."

**What this tells us:**  
Documentation is both slow to produce and unreliable when auto-generated, creating additional friction.

---

## Key Observations

- API integration is the main bottleneck in frontend workflow
- Communication is reactive (errors → clarification → fix cycle)
- No real-time or structured API contract system is in place
- Type mismatches and endpoint inconsistencies are frequent
- Documentation is:
  - manually written
  - delayed
  - sometimes inaccurate
- AI-generated documentation is not trusted due to lack of context accuracy
- Tooling is minimal (Telegram + GitHub only)

---

## Preliminary Insights

- Core problem is absence of a single source of truth for API structure
- Frontend developers depend heavily on backend availability for progress
- Most issues are discovered during implementation, not design phase
- Rework loop is a dominant cost driver
- Even when documentation exists, it is not treated as authoritative
- Communication overhead replaces system-level guarantees

---

## JTBD Frame

> "When working on frontend features that depend on backend APIs, the developer needs an always-up-to-date and accurate definition of endpoints, request/response structures, and types, so they can implement integrations without repeated clarification or rework caused by mismatched expectations."

---

## Open Questions

- How often do API changes break frontend code in this project?
- Who is responsible for maintaining API documentation accuracy?
- Would a shared schema (e.g., OpenAPI/GraphQL) reduce coordination overhead?
- How much time is lost weekly due to API mismatches?
- Would real-time API contract validation prevent these errors?

---

## ICP Fit Assessment

- [x] Strong fit with primary ICP:
  - Actively consumes backend APIs
  - Works in small cross-functional team
  - Experiences repeated integration friction
  - Relies on manual communication for API understanding

---

## Interviewer Reflection

**Most important learning:**  
The main failure point is not communication itself, but lack of a structured API contract layer between backend and frontend.

**What to improve next interviews:**
- Quantify delay per API issue (hours per incident)
- Separate “documentation delay” vs “integration bug delay”
- Ask about frequency of endpoint changes
- Capture specific tools they tried and abandoned

---

## Summary Insight

> Frontend development is blocked by unstable and poorly specified API contracts, forcing repeated manual coordination with backend developers and creating a continuous rework cycle driven by mismatched types, missing specifications, and delayed documentation.