# Affinity Map Documentation

**Team:** API Collaboration Study  
**Date:** April 8, 2026  
**Method:** Manual clustering (based on interview logs)  
**Total insights extracted:** 56  
**Total clusters identified:** 6

**Export evidence:** affinity-map.png committed in repository

---

## Cluster Summary

| #   | Cluster Name                         | Insight Count | Key Theme                                           | Interview Sources |
| --- | ------------------------------------ | ------------- | --------------------------------------------------- | ----------------- |
| 1   | Unreliable API Documentation         | 12            | Documentation does not reflect real API behavior    | VG, AD, SM, BC    |
| 2   | Missing / Delayed API Specifications | 9             | Developers lack API definitions when needed         | AD, SM            |
| 3   | Rework & Debugging Loops             | 10            | Integration issues cause repeated fixes and delays  | VG, AD, BC, LP    |
| 4   | Lack of Single Source of Truth       | 8             | API knowledge is fragmented across tools            | AD, SM, BC, LP    |
| 5   | Manual Validation Workarounds        | 9             | Developers rely on testing instead of trusting docs | SM, BC, LP        |
| 6   | Dependency on Human Communication    | 8             | Progress depends on asking other developers         | AD, SM, LP        |

---

## Cluster Details

### Cluster 1: Unreliable API Documentation

**Theme:** Documentation exists but is frequently outdated, incomplete, or incorrect.

**Sample insights:**

- "Actual API behavior doesn’t fully match documentation." (VG)
- "Behavior differed slightly from documentation." (BC)
- "Docs said date parameter but format was different." (SM)
- "Documentation can get outdated." (BC)
- "AI-generated documentation makes mistakes." (AD)

**Observation:**  
Developers do not trust documentation as a source of truth.

---

### Cluster 2: Missing / Delayed API Specifications

**Theme:** API definitions are unavailable when developers need them.

**Sample insights:**

- "I don’t know routes or types until documentation is written." (AD)
- "Most time consuming part is waiting for API definitions." (AD)
- "I don’t know request/response types until backend writes docs." (SM)
- "Frontend work is blocked." (AD)

**Observation:**  
Development is blocked early due to missing contracts.

---

### Cluster 3: Rework & Debugging Loops

**Theme:** Developers repeatedly fix issues caused by mismatches.

**Sample insights:**

- "Back-and-forth communication and debugging." (VG)
- "It can cost several hours or even days." (VG)
- "Feels repetitive and time wasting." (AD)
- "It takes hours or days to trace the issue." (BC)
- "Schema change caused rework." (LP)

**Observation:**  
The main cost is repeated iteration cycles.

---

### Cluster 4: Lack of Single Source of Truth

**Theme:** API knowledge is scattered across multiple tools and communication channels.

**Sample insights:**

- "We use Telegram and GitHub." (AD)
- "Swagger, Postman, Confluence all used." (LP)
- "Documentation written manually." (AD)
- "Clear code replaces documentation." (GG)

**Observation:**  
No single authoritative system exists.

---

### Cluster 5: Manual Validation Workarounds

**Theme:** Developers rely on manual testing to verify behavior.

**Sample insights:**

- "I use Postman to check endpoints." (SM)
- "Testing separately gives clearer understanding." (BC)
- "Only way to verify behavior is testing." (SM)
- "Developers rely on their own checks." (BC)

**Observation:**  
Manual testing replaces trust in systems.

---

### Cluster 6: Dependency on Human Communication

**Theme:** Developers must ask others to clarify API behavior.

**Sample insights:**

- "I frequently communicate with backend developer." (AD)
- "I asked backend what field type it was." (SM)
- "Waiting for feedback slows things down." (LP)
- "Different working hours delay responses." (SM)

**Observation:**  
Progress depends on human availability.

---

## Relationships Between Clusters

- Cluster 1 (documentation mismatch) directly causes Cluster 3 (rework loops)
- Cluster 2 (missing specs) leads to Cluster 6 (communication dependency)
- Cluster 4 (no source of truth) contributes to all other clusters
- Cluster 5 (manual validation) is a workaround for Clusters 1–4

---

## Outliers and Contradictions

- "Clear code and structure prevent issues." (GG)

**Interpretation:**  
The problem is less visible in high-performing teams but becomes critical in less structured environments.

---

## Key Takeaways

- The core issue is lack of **trust in API knowledge**
- Developers compensate with:
  - manual testing
  - direct communication
- Strong signal: **existing workarounds → real unmet need**

---
