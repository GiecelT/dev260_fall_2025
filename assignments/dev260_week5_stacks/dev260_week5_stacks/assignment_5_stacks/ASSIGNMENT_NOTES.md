# Assignment 5: Browser Navigation System - Implementation Notes

**Name:** Giecel Tumbaga

## Dual-Stack Pattern Understanding

**How the dual-stack pattern works for browser navigation:**
The dual-stack pattern uses two stacks a back and a forward stack to stimulate how real browsers navigate between pages. When I visit a new page, it gets pushed onto the back stack and the most recent page from the back stack, and the forward stack is cleared. When I press "back," the current page is pushed to the forward stack and the most recent page from the back stack becomes the new current page. When I press "forward," the opposite happens; the current page movesto the back stack and the top page from stack becomes current. This pattern works because stacks naturally support navigation history; always dealing with the most recent action first.

## Challenges and Solutions

**Biggest challenge faced:**
My biggest challange during this assignment was keeping the stack behavior straight, especially remembering to clear the forward stack when visiting a new page. If I forgot that step, the browser session allowed "forward" navigation to pages that shouldn't exist anymore.

**How you solved it:**
To solve this challenge, I stepped through example page visits manually and printed stack contents while testing. Drawing a quick timeline for back and forward actions helped me visualize when each stack should change. Once I looked at it that way, the logic made more sense.

**Most confusing concept:**
The most confusing concept for me was how to seperate the behavior between going back/forward vs. visiting a brand-new page. Back/forward moves pages between stacks, but visiting a new page pushes onto the back stack and clears the forward stack. Working through multiple scenarios and testing edge cases helped clarify this difference.

## Code Quality

**What you're most proud of in your implementation:**
The one thing I am most proud of in my implementation is keeping each method clean and focused, followed the expected formatting for displaying history, and amde good use of properties like CanGoBack and CanGoForward to keep conditions readable.

**What you would improve if you had more time:**
I would refactor the repeated printing logic in the history display methods into a helper function. I would also consider returning strings instead of printing directly so the class could be reused in different interfaces.

## Testing Approach

**How you tested your implementation:**
I tested realistic browsing actions, including visiting several pages, going back multiple times, moving forward, and then visiting a new page to ensure the forward stack cleareed. I also tested situations where no back or forward navigation was avilable and used display functions to verify order and correctness.

**Issues you discovered during testing:**
During testing, I found that forward history did not clear correctly when navigating to a new page until I adjusted the VisitUrl logic. I also added checks to make surethe program didn't navigate backward or forward when no history existed.

## Stretch Features

None Implemented

## Time Spent

**Total time:** 5 hours

**Breakdown:**

- Understanding the assignment: 0.5 hours
- Implementing the 6 methods: 2.5 hours
- Testing and debugging: 1.5 hours
- Writing these notes: 0.5 hours

**Most time-consuming part:** 
Debugging stack push/pop behavior and confirming that forward history cleared at the correct times. 
Making sure the logic matched real browser behavior took the most tiral and thought. 
