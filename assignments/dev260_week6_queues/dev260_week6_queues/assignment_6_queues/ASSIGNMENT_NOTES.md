# Assignment 6: Game Matchmaking System - Implementation Notes

**Name:** Giecel Tumbaga

## Multi-Queue Pattern Understanding

**How the multi-queue pattern works for game matchmaking:**
The multi-queue pattern divides players into separate queues based on game mode — Casual, Ranked, and QuickPlay — so each mode can use its own matching logic.  
- **Casual** uses a simple FIFO queue where players are paired in the order they joined.  
- **Ranked** adds a layer of complexity by checking skill ratings within ±2 levels, ensuring balanced matches.  
- **QuickPlay** prioritizes fast matching: it first tries to find close skill matches, but if the queue grows too large, it pairs anyone to reduce waiting time.  
This pattern allows flexible matchmaking behavior without mixing different types of players or goals.

## Challenges and Solutions

**Biggest challenge faced:**
The biggest challenge for me was implementing the skill-based in the Ranked queue. It was tricky to ensure that only players within a ±2 skill range were paired, while keeping the rest of the queue intact. Manging the queue during matching without causing skipped players or broken order took some of my time to solve.

**How you solved it:**
I solved this by copying the queue into a temporary list, searching for a compatible player, and then rebuilding the queue without the matched pair. This was to avoid errors caused by modifying the queue during iteration. Using the helper metod ' CanMatchInRanked()' made the logic clearer and resuable.

**Most confusing concept:**
The most confusing concept for me was learning how to manipulate queues safely during matching. At first, I tried removing elements directly inside a loop, but that caused issues. Understanding the difference between peeking at a player versus dequeing them helped me handle the queue logic correctly. 

## Code Quality

**What you're most proud of in your implementation:**
I’m most proud of how I implemented and organized the `TryCreateMatch()` method, especially the Ranked queue logic. Getting the skill-based matching to work correctly — where players are only matched if their skill levels are within ±2 points — took careful planning and testing. I’m proud that I found a clean solution using a temporary list copy of the queue to safely search for valid matches and then rebuild the queue without breaking its order. The rest of the method is also well structured, with clear logic for each game mode and helpful comments that make the code easy to understand and maintain.  

**What you would improve if you had more time:**
If I had more time, I would add a new feature that allows **party matchmaking**, where groups of friends can queue together as a team. The system could automatically try to match teams of similar average skill levels. This would make the matchmaking system more realistic and closer to what real multiplayer games use. I’d also like to implement MMR adjustments after matches and improve the display of match statistics for better player feedback.

## Testing Approach

**How you tested your implementation:**
I tested the matchmaking system by manually adding players with different skill ratings and preferred modes, then calling `TryCreateMatch()` to see if the right players were matched and removed from their queues. I also printed queue statuses frequently to confirm that the queues updated correctly.

**Test scenarios you used:**
- Two players in Casual mode (expected instant match)
- Ranked players with close skills (expected valid match)
- Ranked players with large skill gaps (expected no match)
- QuickPlay with more than four players (expected fast match even with skill difference)
- Empty or single-player queues (expected null match result)

**Issues you discovered during testing:**
One issue I found early was that players were sometimes not removed properly from the queue after being matched. This caused duplicate match results. Rebuilding the queue after removing players fixed that issue.

## Game Mode Understanding

**Casual Mode matching strategy:**
Casual mode uses a simple **FIFO (First-In-First-Out)** approach. Players are matched strictly in the order they join the queue — no skill filtering or sorting is done. This means that as soon as two players are available, they are paired into a match. This approach reflects casual gameplay where the focus is on quick access and fun rather than fairness or competitiveness. The queue behavior here directly demonstrates the classic queue principle where the first players added are the first to be processed.

**Ranked Mode matching strategy:**
Ranked mode implements a **skill-based matching algorithm**. Each player has a `SkillRating`, and the system only creates a match if two players’ ratings are within a ±2 range. This was handled by temporarily copying the queue into a list to safely check potential pairs without modifying the original queue. Once a valid match is found, those players are removed and added to a match object. This ensures competitive balance, simulating real-world ranking systems where players face opponents close to their own skill level. I’m particularly proud of how this logic was structured clearly and efficiently.

**QuickPlay Mode matching strategy:**
QuickPlay mode is designed as a hybrid between Casual and Ranked. It prioritizes **speed over perfect balance** but still tries to match players who aren’t drastically different in skill. The system first looks for any two players within a broader skill range (for example, ±5), but if it can’t find one quickly, it will relax the condition to ensure players get into matches faster. This mirrors real multiplayer games where QuickPlay is meant to minimize waiting time while still maintaining a reasonable level of fairness.

## Real-World Applications

**How this relates to actual game matchmaking:**
This system models real-world matchmaking systems used by games like *Overwatch*, *League of Legends*, or *Apex Legends*. Those games also separate modes by type and use different rules to balance fairness and queue speed. Ranked queues often use strict MMR brackets, while quick modes prioritize fast, fun gameplay.

**What you learned about game industry patterns:**
I learned that matchmaking systems must balance fairness and speed — too much focus on balance makes players wait too long, while too little can lead to unfair matches. The multi-queue pattern is a common solution that gives each game mode its own balance between those goals.

## Stretch Features
None Implemented

## Time Spent

**Total time:** ~ 7 hours

**Breakdown:**

- Understanding the assignment and queue concepts: 1 hours
- Implementing the 6 core methods: 3 hours
- Testing different game modes and scenarios: 1 hour
- Debugging and fixing issues: 1 hour
- Writing these notes: 1 hour

**Most time-consuming part:** 
The Ranked matchmaking algorithm — figuring out how to pair players within a skill range without breaking queue order took the most time and testing.

## Key Learning Outcomes

**Queue concepts learned:**
I learned how to use multiple queues to manage different types of players efficiently and how FIFO behavior affects fairness and timing in multiplayer systems.

**Algorithm design insights:**
I learned how to design matching algorithms that balance constraints (like skill difference and queue size) and how to rebuild collections safely when removing elements during iteration.

**Software engineering practices:**
I practiced writing modular, maintainable code by using helper methods, clean switch statements, and consistent formatting. I also learned the importance of testing edge cases and handling invalid input gracefully.
