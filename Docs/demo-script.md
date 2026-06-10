# Driftless — Live Demo Script

**Demo owner:** Nikoloz Bujiashvili (Design Lead)
**Demo duration:** 45 seconds (within pitch) + up to 2 minutes for extended Q&A demo
**Live URL:** https://driftless.nikatopu.dev/
**Device:** Laptop or desktop browser, full-screen, projected
**Fallback device:** Second team member's laptop loaded and ready

---

## Pre-Demo Checklist (Complete before entering the room)

- [ ] driftless.nikatopu.dev loaded in browser, not cached to a previous result
- [ ] Browser zoom set to 125% so text is legible when projected
- [ ] Demo repository URL copied to clipboard: `https://github.com/tomadanelia/community-dashboard.git`
- [ ] Fallback browser tab open on a pre-generated result screenshot in case backend is unreachable
- [ ] PostHog analytics tab open on a second monitor or second device (for traction slide)
- [ ] Second team member's laptop open to driftless.nikatopu.dev as a hot backup

---

## Primary Demo Sequence (45 seconds in-pitch)

### Step 1 — Land on the home page (0:00–0:05)

**What is on screen:**
The Driftless home page at driftless.nikatopu.dev. Input field visible. Clean empty state.

**What you say:**
"This is the live product. driftless dot nikatopu dot dev. I am going to paste in a public GitHub repository URL for an ASP.NET Core project."

**Action:**
Click the repository URL input field. Paste the pre-copied GitHub URL.

---

### Step 2 — Submit the URL and show analysis beginning (0:05–0:15)

**What is on screen:**
Loading/processing state. Progress indicator if one exists, or brief waiting state.

**What you say:**
"Driftless is now cloning the repository and running Roslyn analysis on the C# source code. No configuration. No annotation. It is reading the implementation directly."

**Action:**
Click the submit/analyze button. Do not narrate silence — let the tool work. If the loading takes more than 8 seconds, say: "The backend is running the Roslyn extraction — this takes a few seconds for a new repository."

---

### Step 3 — Show the Routes tab (0:15–0:25)

**What is on screen:**
The routes panel populated with discovered endpoints. Each row shows: HTTP method (GET/POST/PUT/DELETE), route path, controller name.

**What you say:**
"Every API endpoint discovered. HTTP method, route, controller — extracted directly from the implementation. No human wrote this."

**Action:**
Scroll slowly through the route list so judges can see multiple entries. Do not rush. The density of the route list is itself evidence of capability.

---

### Step 4 — Show the TypeScript tab (0:25–0:40)

**What is on screen:**
TypeScript interface definitions generated from the response models. Clean, typed interfaces.

**What you say:**
"And here are the TypeScript interfaces generated from the response schemas. A frontend developer can copy this directly into their codebase today. The moment the backend changes and they run Driftless again, the contract updates automatically."

**Action:**
Click the TypeScript tab (or equivalent). Let the output render. Optionally, briefly highlight one interface with the cursor to draw attention to a specific field.

---

### Step 5 — Close the demo (0:40–0:45)

**What is on screen:**
The TypeScript output or the full results view.

**What you say:**
"Paste a URL. Get a contract. That is Driftless."

**Action:**
Step away from the laptop slightly to signal the demo is complete. Do not close the browser — leave the result on screen while the pitch continues.

---

## Extended Demo (Q&A, up to 2 minutes)

Use this sequence if judges ask to see the product in more detail during Q&A.

### Extended Step A — Show a specific route in detail

**What to show:**
Click into an individual route if the UI supports it. Show the request parameters and response schema for one endpoint.

**What to say:**
"If I click into this endpoint, you can see the full request schema — the expected parameters, types, and whether they are required or optional — and the full response model, all extracted from the C# source without a single annotation."

---

### Extended Step B — Show a second repository

**What to show:**
Paste a second, different ASP.NET Core repository URL and run analysis again.

**What to say:**
"I can run it on any public ASP.NET Core repository. Here is a different project. Same result — a full contract in seconds."

**Action:**
Submit the second repository. Let it process. Confirm the results populate.

---

### Extended Step C — Show the TypeScript output being usable

**What to show:**
Highlight and copy a TypeScript interface from the results. Paste it into a temporary text editor tab to visually demonstrate it is plain, usable TypeScript.

**What to say:**
"This is plain TypeScript. No Driftless-specific import. No dependency on our platform. Copy it into your project and it works immediately."

---

## Fallback Protocol

### Fallback 1 — Backend is slow or shows a spinner for more than 15 seconds

**Action:** Say: "The backend is processing — Roslyn analysis on a large repository can take up to 20 seconds. While it loads, I will describe what you are about to see."

Continue narrating the expected output. If it loads before you finish, transition smoothly: "And here it is."

---

### Fallback 2 — Backend returns an error or empty result

**Action:** Switch to the pre-generated screenshot immediately. Say: "We have a connectivity issue with the live server — let me show you a result we generated earlier this morning."

Show the screenshot. Walk through the same steps verbally as if it were live. Do not apologise excessively. One sentence acknowledgment, then continue.

The fallback screenshot should show:

- A populated routes panel with at least 5 endpoints
- At least one TypeScript interface
- The repository URL visible at the top to prove it was real input

---

### Fallback 3 — Both live product and screenshot are unavailable

**Action:** Switch to Giorgi's laptop which should have a cached browser version of a previous result. If that also fails, describe the product from the slide screenshots and say: "We are experiencing an infrastructure issue during the demo. The product is live at driftless.nikatopu.dev and we are happy to show judges individually after the session."

Do not visibly panic. Judges evaluate how teams recover from failure.

---

### Fallback 4 — Wrong repository produces zero routes

**Action:** Switch immediately to the backup repository URL. Say: "Let me use a different repository that I know parses cleanly." Switch without further explanation.

**Backup repository URL (prepare this in advance):**
Use a repository the team has previously verified produces clean output. Document the exact URL here before Demo Day:
`[INSERT VERIFIED BACKUP REPOSITORY URL]`

---

## Demo Environment Preparation Checklist

Complete this the evening before Demo Day and again 30 minutes before the pitch.

| Check                                                 | Evening before | 30 min before |
| ----------------------------------------------------- | -------------- | ------------- |
| Live URL loads without error                          | [ ]            | [ ]           |
| Demo repository URL produces results                  | [ ]            | [ ]           |
| Routes tab populated with real data                   | [ ]            | [ ]           |
| TypeScript tab populated with real interfaces         | [ ]            | [ ]           |
| Browser zoom at 125%                                  | [ ]            | [ ]           |
| Fallback screenshot saved locally (not on cloud only) | [ ]            | [ ]           |
| Second laptop loaded and verified                     | [ ]            | [ ]           |
| Venue WiFi tested                                     | [ ]            | [ ]           |
| PostHog dashboard open for traction Q&A               | [ ]            | [ ]           |

---

## Speaker Notes During Demo

- **Do not read from the screen.** Know what is on each tab before you open it.
- **Do not apologise for loading time.** Fill it with narration or silence. Apologising signals anxiety.
- **Keep the cursor moving slowly.** Rapid mouse movement is hard to follow when projected.
- **Call out the key moments explicitly.** Judges will not read UI labels. Say "HTTP method", "route path", "TypeScript interface" out loud as each appears.
- **If a number appears in the UI** (e.g. number of routes found), read it aloud: "Fourteen endpoints discovered."
- **The demo ends with a one-line close.** "Paste a URL. Get a contract." Land it. Then step back.

---

_Driftless | Live Demo Script | CS-PD-2026 | Spring 2026_
