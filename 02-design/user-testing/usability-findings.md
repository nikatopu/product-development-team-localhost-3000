# Usability Testing Findings — Driftless

**Team:** localhost:3000  
**Product:** Driftless  
**Date:** May 6–12, 2026  
**Conducted by:** Nikoloz Bujiashvili (Design Lead), Toma Danelia (Discovery Lead)  
**Version:** 1.0

---

## Overview

Five usability tests were conducted with real users who are not team members. Each session lasted 25–40 minutes. Participants were asked to complete the core task without assistance: enter a public GitHub repository URL, analyze it, and find a specific route in the results.

**Prototype used:** Driftless high-fidelity prototype (Stitch, v0.2 at time of testing)  
**Test environment:** Laptop, Chrome browser, in-person or remote via Google Meet screen share  
**Recruitment method:** University peers and developer contacts outside the team

---

## Task Given to All Participants

> "You are a frontend developer joining a new project. The backend team uses ASP.NET Core. Using Driftless, find what the POST endpoint for the user registration looks like — what data it accepts and what it returns."

---

## Participant Profiles

| ID  | Description                                  | Background                          | Familiarity with API tools                                  |
| --- | -------------------------------------------- | ----------------------------------- | ----------------------------------------------------------- |
| P1  | 3rd-year CS student, KIU                     | Frontend-focused, React experience  | Used Postman, no experience with .NET                       |
| P2  | Junior frontend developer, 1 year experience | React + Vue, small agency           | Used Swagger UI, first time seeing automated doc generation |
| P3  | Backend developer, 2 years experience        | Node.js / Express background        | Familiar with OpenAPI, no .NET experience                   |
| P4  | Full-stack developer, 3 years experience     | Node.js + React, startup background | Comfortable with Swagger, used Insomnia                     |
| P5  | 2nd-year CS student, KIU                     | Beginner, learned Java in class     | No prior experience with API documentation tools            |

---

## Findings Per Participant

---

### P1 — 3rd-Year CS Student

**Session date:** May 6, 2026  
**Duration:** 28 minutes  
**Task completed:** Yes

**Observations:**

1. P1 opened the page, read the heading "Analyze any ASP.NET Core repository," and immediately started typing a URL. No confusion at this stage.

2. P1 noticed the small button labeled "main" next to the URL input and paused. "What is 'main' doing there? Is that something I need to change?" P1 did not click it and proceeded with the default. After analysis completed, P1 revisited it and tried clicking — was surprised to see a branch input appear. "Oh, it's for the branch. I didn't know it was a button."

3. P1 successfully used the method filter buttons to narrow results to POST routes. Found the target route in 45 seconds after results loaded.

4. P1 did not notice the TypeScript tab until prompted. When shown it, said: "That's actually really useful. I would have expected it to be more obvious."

5. P1 was confused by the "controllers" metadata badge: "What does '3 controllers' mean? Is that something I need to care about?"

**Issues identified:**

- Branch toggle button: visual affordance is unclear; users do not recognize it as interactive
- TypeScript tab: discoverability is low for users focused on the Routes view
- "Controllers" badge: term is unfamiliar to frontend developers without .NET background

---

### P2 — Junior Frontend Developer

**Session date:** May 7, 2026  
**Duration:** 35 minutes  
**Task completed:** Yes, with one retry

**Observations:**

1. P2 pasted a GitHub URL without the `.git` extension and pressed Analyze. "Do I need `.git` at the end? I wasn't sure." The analysis succeeded regardless; P2 was relieved.

2. During the 42-second loading period, P2 read the loading hint ("Cloning repository and analyzing routes — this may take 30–60 seconds…") and said "OK, that's helpful, I know it's working." Did not retry the submission.

3. When results loaded, P2 immediately noticed the Routes and TypeScript tabs. Clicked TypeScript first: "I want to see the types — that's what I came for as a frontend dev." Navigated back to Routes to find the specific endpoint.

4. P2 tried to copy code from the TypeScript panel by selecting text manually. "I'd really like a copy button here. I have to be careful not to miss anything." Completed the task by manual selection.

5. P2 attempted to use the text filter to search for "register" but noticed it only filtered by route path and controller name — the endpoint summary was not yet searchable. "I typed 'register' and it found nothing, but I can see a route called `/api/users` that probably is it."

**Issues identified:**

- No copy button in the TypeScript panel — users must manually select generated code
- Filter search does not match against summary text (resolved in code but not in prototype at time of testing)
- Expectation mismatch: users expect search to find endpoints by purpose, not just by path

---

### P3 — Backend Developer (Node.js/Express)

**Session date:** May 8, 2026  
**Duration:** 25 minutes  
**Task completed:** Yes

**Observations:**

1. P3 was the most technically confident participant. Read the tagline, entered the URL immediately, and clicked Analyze without hesitation.

2. While waiting, P3 explored the Navbar: "About, How to Use — I'll check those after." Returned to the home page when results loaded.

3. P3 found the route cards dense: "There's a lot of information here. I'd like to be able to collapse ones I'm not looking at." Expanded the request schema on a POST route and immediately found the relevant properties.

4. P3 switched to the TypeScript tab and said: "This is exactly what I'd send to the frontend team. If this were a copy button here it would be immediate." Noted the absence of an export-to-file option.

5. P3 noticed the method badge colors (GET = green, POST = blue, DELETE = red) without prompting: "Good use of color, I know what I'm looking at without reading the badge text."

**Issues identified:**

- Route cards: no collapse/expand at the card level — dense layout for repos with many endpoints
- TypeScript panel: no copy button, no export-to-file option
- Overall experience positive; no critical blockers for P3

---

### P4 — Full-Stack Developer

**Session date:** May 9, 2026  
**Duration:** 40 minutes  
**Task completed:** Yes (second attempt)

**Observations:**

1. P4 entered a URL and waited. After 38 seconds with the loading spinner still visible, P4 pressed F5 to refresh the page. "I thought it had stalled. There was no visual progress beyond the spinner." The page reloaded and P4 had to re-enter the URL.

2. On the second attempt, P4 waited out the full load and results appeared. "OK, so it was still working. The loading hint helped on the second try — I read it more carefully."

3. P4 noticed the branch toggle immediately on the second visit: "I see 'main' here, that's the branch selector. I'd label it more clearly — something like 'Branch: main'." This confirms the P1 finding independently.

4. P4 tried to copy TypeScript output and — finding no copy button — switched to the browser's developer tools to access the raw API response directly. "I know how to get it from the network tab but a normal user wouldn't."

5. P4 was satisfied with the route card information density: "As a full-stack developer I want to see everything. The table format for properties is clear."

**Issues identified (critical):**

- No visual progress indicator beyond spinner — users with slow connections interpret the loading state as a crash or stall
- Branch toggle button lacks clarity — second independent confirmation of P1 finding
- TypeScript panel: missing copy button — third confirmation of this gap

---

### P5 — 2nd-Year CS Student (Beginner)

**Session date:** May 12, 2026  
**Duration:** 38 minutes  
**Task completed:** Partially (found Routes results, could not interpret TypeScript output)

**Observations:**

1. P5 read the heading and tagline slowly. "Analyze any ASP.NET Core repository. What's ASP.NET Core?" When the researcher clarified it was a backend framework, P5 proceeded.

2. P5 had difficulty forming a valid GitHub URL. Typed the repository name ("community-dashboard") without the full URL. The input validated as a non-URL and the form rejected submission. "Oh, it needs the full link." When shown the example URL in the empty state, P5 said: "I didn't see that. Maybe it should be closer to the input."

3. After entering a correct URL, P5 waited patiently for results and was visibly pleased when route cards appeared. "These look like the API calls we studied in class."

4. P5 used the ALL/GET/POST method filter correctly without instruction: "I'll press POST to find the registration one." Identified the correct route.

5. P5 opened the TypeScript tab and was confused: "What are these? Are these like Java classes?" Could not interpret the output without background knowledge, but acknowledged it looked "very organized."

**Issues identified:**

- Example URL in empty state is positioned below the fold on smaller screens — first-time users miss it
- TypeScript output assumes prior knowledge of TypeScript interfaces — no inline explanation for beginners
- The product's target audience (experienced developers) aligns poorly with beginners; this finding is informational, not a blocker

---

## Issue Summary

| ID  | Issue                                                                                           | Participants affected | Severity |
| --- | ----------------------------------------------------------------------------------------------- | --------------------- | -------- |
| U1  | Branch toggle button is unclear — not visually identifiable as interactive, purpose not obvious | P1, P4                | High     |
| U2  | TypeScript panel has no copy button — users must manually select generated code                 | P2, P3, P4            | High     |
| U3  | No loading progress beyond spinner — slow connections cause users to abandon and retry          | P4                    | Medium   |
| U4  | TypeScript tab has low discoverability — users focused on Routes do not notice it               | P1                    | Medium   |
| U5  | Example URL positioned below the fold on smaller screens                                        | P5                    | Medium   |
| U6  | "Controllers" badge unfamiliar to frontend-only developers                                      | P1                    | Low      |
| U7  | Route cards cannot be collapsed in dense result sets                                            | P3                    | Low      |

---

## Design Changes Made in Response to Findings

### Change 1 — Branch Toggle Button Relabelled

**Finding:** U1 (P1 and P4 independently)  
**Observation:** P1: "What is 'main' doing there? Is that something I need to change?" P4: "I'd label it more clearly — something like 'Branch: main'."

**Before:** The branch toggle button displayed only the current branch value (e.g., "main") with a git branch SVG icon. Nothing indicated it was an interactive button or what it controlled.

**After:** The button label was updated to read "Branch: main" by default, making the semantic purpose explicit. A hover tooltip was added: "Click to change the target branch."

**Evidence in prototype:** Prototype v0.2 (May 10, 2026) reflects this change. Compare v0.1 (button reads "main") with v0.2 (button reads "Branch: main").

---

### Change 2 — Copy Button Added to TypeScript Panel

**Finding:** U2 (P2, P3, and P4)  
**Observation:** P2: "I'd really like a copy button here." P3: "If this were a copy button here it would be immediate." P4 resorted to the browser's network tab to extract the output.

**Before:** The TypeScript panel displayed generated type definitions in a code block with no way to copy the content other than manual text selection.

**After:** A "Copy" button was added to the top-right corner of the TypeScript panel. On click, the full TypeScript output is copied to the clipboard and the button label temporarily changes to "Copied ✓" for 2 seconds.

**Evidence in prototype:** Prototype v0.3 (May 14, 2026) reflects this change. The copy button appears in the TypeScript panel header.

---

## Findings Not Yet Acted Upon

| Issue                          | Reason deferred                                                   |
| ------------------------------ | ----------------------------------------------------------------- |
| U3 (loading progress)          | Requires a streaming backend implementation; deferred to Sprint 2 |
| U5 (example URL position)      | Minor layout adjustment; deferred to Sprint 2 styling pass        |
| U6 (controllers badge tooltip) | Low severity; deferred                                            |
| U7 (route card collapse)       | Feature scope exceeds Sprint 1; added to backlog                  |

---

## Conclusion

The two highest-severity findings (U1 and U2) were acted upon before Sprint 1 review. The branch toggle relabelling and the copy button addition are documented with before/after descriptions and the specific participant observations that motivated each change. The prototype reflects both changes as of v1.0.

---

_Usability Testing Findings | localhost:3000 | CS-PD-2026 | Spring 2026_
