# Interview Log — API Collaboration Study (Participant CD)

---

## Participant Profile

| Field            | Value                                                                 |
| ---------------- | --------------------------------------------------------------------- |
| Pseudonym        | SM                                                                  |
| Role              | Android / Kotlin Developer (TV application)                           |
| Relevant context  | Streaming platform (TV app), websocket and API integration           |
| How recruited     | Existing project team                                                 |
| Interview date    | April 6–7, 2026                                                      |
| Duration          | ~15–20 minutes                                                        |
| Format            | Structured interview (remote chat-based)                              |
| Interviewer(s)    | Toma Danelia                                                         |

---

## Interview Setting

Participant works on a TV streaming application using Kotlin. Team consists of backend developer and frontend web developer, with additional focus on websocket-based communication between services. Communication is primarily via Telegram, with GitHub used for version control.

---

## Verbatim Quotes

**Quote 1**  
Context: current workflow description

> "I am Kotlin Android developer currently working on streaming platform on TV devices, I have frequent communication with backend developer to ensure I implement API endpoints correctly."

**What this tells us:**  
API integration is heavily communication-dependent and requires constant alignment with backend.

---

**Quote 2**  
Context: team collaboration tools

> "We collaborate using Telegram for communication and use GitHub for version control."

**What this tells us:**  
There is no dedicated API contract or integration system; coordination is informal and chat-based.

---

**Quote 3**  
Context: main frustrations in workflow

> "Most time consuming part is adding features related to backend endpoints or websocket connections because I don’t know routes, request and response types until backend developer writes documentation."

**What this tells us:**  
Frontend (TV app) development is blocked by missing or delayed API specification.

---

**Quote 4**  
Context: debugging workflow

> "I use tools like Postman to manually check endpoints because I don’t have something like devtools window during TV APK development like browsers have."

**What this tells us:**  
There is no native inspection/debugging layer in TV environment, increasing reliance on external tools.

---

**Quote 5**  
Context: repetitive workflow

> "Spotting errors and understanding if they are because of my wrong implementation or API documentation is wrong or already refactored takes time and is frustrating."

**What this tells us:**  
Core inefficiency is attribution problem: unclear source of failure (frontend vs backend vs documentation).

---

**Quote 6**  
Context: example of real issue

> "When implementing EPG features, there were two endpoints... docs said date parameter but actually it had to be specific sequence of dd/mm/yy... only thing I could test it with was Postman."

**What this tells us:**  
Documentation is incorrect or misleading, and real behavior is discovered only through manual testing.

---

**Quote 7**  
Context: API integration experience

> "I had to show account id on profile section... I asked backend developer on Telegram what field name and type it was and implemented."

**What this tells us:**  
Even simple schema additions require direct human clarification instead of self-serve API discovery.

---

**Quote 8**  
Context: cost of clarification process

> "It costs time to ask for something to be explained because everyone has different working hours and I don’t want to text them when they are offline."

**What this tells us:**  
Coordination is asynchronous and blocked by availability constraints, creating delays in development flow.

---

**Quote 9**  
Context: documentation tooling

> "It takes time for backend developer to write documentation when something is refactored... using AI is frustrating because it makes many mistakes without enough context."

**What this tells us:**  
Documentation generation is slow and unreliable; AI-assisted documentation is not trusted due to accuracy issues.

---

## Key Observations

- API and websocket integration are primary bottlenecks
- TV app development lacks debugging tools compared to web (no DevTools equivalent)
- Developers rely on Postman as external validation layer
- Documentation is:
  - delayed
  - sometimes incorrect
  - hard to maintain after refactors
- Communication overhead increases due to time zone / availability mismatch
- Root cause identification is a frequent problem (API vs implementation vs docs)
- Even simple schema queries require manual human interaction

---

## Preliminary Insights

- Lack of real-time API contract visibility is a shared core issue
- TV platform constraints increase reliance on backend communication
- Debugging is externalized into manual tools (Postman) instead of integrated tooling
- Documentation does not function as a reliable source of truth
- Websocket-based features increase complexity of integration feedback loops
- Developer productivity is strongly constrained by dependency waiting time

---

## JTBD Frame

> "When implementing backend-dependent features in a TV application, the developer needs immediate and reliable access to correct API and websocket specifications, so they can implement and debug integrations without waiting for manual clarification or relying on external testing tools."

---

## Open Questions

- How often do API parameter mismatches occur in TV vs web apps?
- Would a shared schema definition reduce websocket/API confusion?
- How can TV app development include better debugging visibility?
- What is the average delay caused by waiting for backend clarification?
- Can API testing be embedded directly into development workflow for TV apps?

---

## ICP Fit Assessment

- [x] Strong fit with primary ICP:
  - Works with backend APIs and websockets
  - Collaborates closely with backend developers
  - Experiences repeated integration friction
  - Relies on manual testing and communication for API understanding

---

## Interviewer Reflection

**Most important learning:**  
TV app development introduces additional friction because developers lack native debugging tools, making API uncertainty even more costly than in web environments.

**What to improve next interviews:**
- Quantify time lost due to backend waiting time
- Separate websocket issues from REST API issues
- Ask frequency of API refactors
- Identify whether schema mismatches are systematic or occasional

---

## Summary Insight

> TV application developers face amplified API integration problems due to lack of debugging tools and delayed or inaccurate API specifications, forcing reliance on manual testing tools and asynchronous communication with backend developers.