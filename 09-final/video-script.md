# 60-Second Launch Video Script

**Team Name:** localhost:3000  
**Product Name:** Driftless  
**Date Planned:** Thursday 4 June 2026  
**Target Shoot Date:** June 5, 2026  
**Live URL:** https://driftless.nikatopu.dev/

---

## Section 1: Hook (Seconds 0 to 10)
**One Job:** Make the viewer feel the pain of API drift before mentioning the product.

* **What you will say (verbatim):**  
  "You are building a frontend feature, you need the backend API types, but the documentation is either out of date or completely missing."
* **What will be on screen:**  
  Close-up of a developer sitting at a desk in the computer lab, looking frustrated at a broken Swagger page showing a `TypeError: Cannot read properties of undefined` on their screen.
* **Who is on camera:** Toma Danelia (Discovery Lead)

---

## Section 2: Problem (Seconds 10 to 25)
**One Job:** Show the old painful way and its real consequences.

* **What you will say (verbatim):**  
  "So you message the backend dev on Telegram. They are offline. You guess the types, write the code, and hit compile. It breaks because a string was actually an integer. You just lost two hours of your day."
* **What will be on screen:**  
  Quick cuts: 
  1. A chat window on Telegram with a pending "What is the response type for /api/users?" message.
  2. The developer manually typing out tentative TypeScript interfaces.
  3. A terminal compilation error showing type mismatches.
* **Who is on camera:** Toma Danelia (acting as the frustrated developer), with Nikoloz Topuridze's laptop screen in close-up.

---

## Section 3: Product (Seconds 25 to 45)
**One Job:** Show the real deployed product completing the core user flow in real time without cuts.

* **What you will say (verbatim):**  
  "Stop guessing. Open Driftless. Paste your ASP.NET GitHub repository URL, and click Analyze. Driftless clones your code, parses the syntax with Roslyn, and extracts all routes and schemas. Click the TypeScript tab, copy your auto-generated interfaces, and paste them straight into your project."
* **What will be on screen:**  
  A continuous, unedited over-the-shoulder shot of a smartphone in portrait orientation. 
  1. The browser is open to `driftless.nikatopu.dev`.
  2. The user pastes the URL: `https://github.com/tomadanelia/community-dashboard.git`.
  3. The user clicks the "Analyze" button.
  4. The loader spins for a few seconds, then routes and collapsible request/response tables render cleanly.
  5. The user switches to the "TypeScript" tab and clicks the "Copy" button.
* **Device and Orientation:** Mobile Safari on iPhone, portrait orientation. Held by Nikoloz Bujiashvili.

---

## Section 4: Proof (Seconds 45 to 55)
**One Job:** Show a real number that proves the product is working.

* **What you will say (verbatim):**  
  "Eight out of ten developers we tested experience this exact documentation drift. Driftless generates clean, accurate types in under thirty seconds."
* **What will be on screen:**  
  Quick cut to a laptop screen displaying the live PostHog analytics dashboard, highlighting active sessions, followed by a text overlay of our validation metric: *"70% of tested developers expressed immediate willingness to adopt."*
* **Who is on camera:** Giorgi Tkebuchava (Program Lead) pointing to the PostHog dashboard on screen.

---

## Section 5: Call to Action (Seconds 55 to 60)
**One Job:** Tell the viewer exactly where to go to try it.

* **What you will say (verbatim):**  
  "Eliminate API drift today. Try it for free at driftless.nikatopu.dev."
* **What will be on screen:**  
  The presenter holds up a clean, printed card showing a large QR code linking to the app alongside the text: `driftless.nikatopu.dev`.
* **Who is on camera:** Toma Danelia.

---

## Production Planning

* **Shoot Location:** Kutaisi International University (KIU) Library and Computer Labs. These locations accurately reflect the active study and development context of our target audience.
* **Team Responsibilities:**
  * **Toma Danelia:** Main presenter (on camera for Sections 1, 2, and 5).
  * **Nikoloz Topuridze:** Technical flow navigator (manages the laptop/phone UI interactions shown in Section 3).
  * **Giorgi Tkebuchava:** Co-presenter (on camera for Section 4) and director (monitors script timing).
  * **Nikoloz Bujiashvili:** Camera operator (filming on an iPhone 15 Pro in 4K 60fps, portrait mode).
* **Upload and Commit Plan:**  
  The video will be recorded in portrait orientation, edited using CapCut, and uploaded as an unlisted video to YouTube. The access link will be documented in `09-final/demo-video.md` and added to the root `README.md`.