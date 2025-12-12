# Study Planner - Academic Task Management System

> A console-based study session planner that helps students organize courses, manage study sessions with priorities, and track daily progress using efficient data structures.

---

## What I Built (Overview)

**Problem this solves:**  
Students often struggle to manage multiple courses with varying deadlines and priorities. Study Planner addresses this by providing a structured system to track courses with prerequisites, schedule study sessions by priority, and maintain a daily task queue. It demonstrates how graph structures can model course dependencies while priority queues automatically organize study sessions by urgency.

**Core features:**  

- **Course Management**: Add, update, delete, and list courses with automatic sequential ID assignment (C1, C2, ...)
- **Prerequisite Tracking**: Define and view course prerequisites with cycle detection to prevent circular dependencies
- **Study Session CRUD**: Create, edit, delete study sessions with CourseID validation, mandatory due dates, and priority scoring
- **Priority Scheduling**: Automatic sorting by due date → priority score → duration using custom comparers
- **Today's Queue**: Schedule sessions for today with FIFO processing, move-to-top, and completion tracking
- **Session Status Management**: Track study sessions as Not Started, In Progress, or Completed with visual indicators
- **Input Validation**: Robust validation for dates (MM-DD-YYYY format), integers, and non-empty strings with helpful error messages
- **ID Renumbering**: Maintains contiguous sequential IDs even after deletions (e.g., T1, T2, T5 becomes T1, T2, T3)

## How to Run

**Requirements:**  

- **.NET 9.0** or later
- **OS**: Windows, macOS, or Linux
- **Dependencies**: None (uses standard .NET libraries only)

```bash
git clone https://github.com/GiecelT/dev260_fall_2025/tree/week-12-final-project
cd final_project_starter/final_project_starter
dotnet build
```

**Run:**  

```bash
dotnet run --project src/StudyPlanner.csproj
```

Or from the `final_project_starter` directory:

```bash
cd src
dotnet run
```

**Sample data (if applicable):**  

Sample data is automatically loaded on startup (see `Program.cs`):
- **6 sample courses** with descriptions (Data Structures, Web Development, Database Design, Machine Learning, Algorithms, Software Engineering)
- **4 prerequisite relationships** configured to demonstrate the graph structure:
  - Algorithms requires Data Structures
  - Machine Learning requires Algorithms
  - Software Engineering requires Algorithms
  - Database Design requires Data Structures
  
Data is session-only (not persisted to disk). All changes are lost on exit.

---

## Using the App (Quick Start)

**Typical workflow:**  

1. **View existing courses** → Main Menu → Option 1 (Courses) → Option 1 (List Courses) to see the 6 preloaded sample courses (C1-C6)
2. **Add a study session** → Main Menu → Option 2 (Study Sessions) → Option 2 (Add Study Session) → Enter title, course ID (e.g., C1), due date (MM-DD-YYYY), priority score, and estimated duration
3. **Schedule for today** → Study Sessions Menu → Option 5 (Schedule for Today) → Enter session ID (e.g., T1) to add it to today's queue
4. **Process today's tasks** → Main Menu → Option 3 (Today's Schedule) → Option 2 (Start Next) to mark the first session as In Progress, then Option 4 (Complete) to finish it

**Input tips:**  

- **Case Sensitivity**: IDs are case-sensitive (use "C1" not "c1"), but the system generates them so this is rarely an issue
- **Required Fields**: 
  - Due dates are **mandatory** (will loop until valid date entered in MM-DD-YYYY format)
  - Course ID is optional (press Enter to skip), but if entered must match an existing course (shows list if invalid)
  - Titles and descriptions cannot be empty (validator rejects whitespace-only input)
- **Error Handling**:
  - Invalid integers → "Invalid integer. Try again." with retry loop
  - Invalid dates → "Invalid date. Use yyyy-MM-dd or leave empty." with retry
  - Invalid Course ID → Shows list of available courses and re-prompts
  - Task/Course not found → "❌ [Item] not found" with graceful menu return
  - Circular prerequisites → "❌ Failed to add prerequisite. This may create a circular dependency."

---

## Data Structures (Brief Summary)

> Full rationale goes in **DESIGN.md**. Here, list only what you used and the feature it powers.

**Data structures used:**  

- `Dictionary<string, TaskItem>` (TaskLookup) → O(1) lookup for edit/delete/complete operations by task ID
- `Dictionary<string, Course>` (CourseGraph nodes) → O(1) course retrieval for prerequisite checks and display
- `Dictionary<string, List<string>>` (CourseGraph edges) → Adjacency list for prerequisite relationships (graph structure)
- `SortedSet<TaskItem>` (SimplePriorityQueue) → Automatic priority sorting by due date → priority score → duration
- `Queue<TaskItem>` (TodaysQueue) → FIFO processing of scheduled study sessions for today
- `List<string>` (_taskInsertionOrder, CourseGraph.insertionOrder) → Track creation order to enable sequential ID renumbering on delete
- `Custom Graph (CourseGraph)` → Non-linear structure for course prerequisites with cycle detection (DFS) to prevent circular dependencies

---

## Manual Testing Summary

**Test scenarios:**  

**Scenario 1: Course Prerequisite Cycle Detection**

- Steps:
  1. Add prerequisite: C5 (Algorithms) requires C1 (Data Structures) ✅
  2. Add prerequisite: C4 (Machine Learning) requires C5 (Algorithms) ✅
  3. Attempt to add: C1 (Data Structures) requires C4 (Machine Learning) ❌ (creates cycle)
- Expected result: System rejects the circular dependency with error message
- Actual result: ✅ "❌ Failed to add prerequisite. This may create a circular dependency." displayed; graph remains valid

**Scenario 2: Sequential ID Renumbering on Delete**

- Steps:
  1. Create 5 study sessions (T1, T2, T3, T4, T5)
  2. Delete T3
  3. List all study sessions
- Expected result: IDs automatically renumbered to T1, T2, T3, T4 (no gap)
- Actual result: ✅ Remaining sessions renumbered correctly; T4 became T3, T5 became T4

**Scenario 3: Priority-Based Task Ordering**

- Steps:
  1. Add Task A: Due 12/20/2025, Priority 5
  2. Add Task B: Due 12/15/2025, Priority 3
  3. Add Task C: Due 12/15/2025, Priority 8
  4. List study sessions
- Expected result: Display order = Task B (earlier date), Task C (same date, higher priority), Task A (later date)
- Actual result: ✅ SortedSet correctly orders by due date first, then priority score

**Scenario 4: CourseID Validation on Task Creation**

- Steps:
  1. Attempt to add study session with CourseID "C99" (doesn't exist)
  2. System shows available courses (C1-C6)
  3. Re-enter valid CourseID "C2"
- Expected result: Validation loop continues until valid course or skip
- Actual result: ✅ Error message displayed with course list; re-prompts for valid input

**Scenario 5: Today's Queue Move-to-Top and Completion**

- Steps:
  1. Schedule T1, T2, T3 for today
  2. Move T3 to top of queue
  3. Start next session (should be T3)
  4. Complete T3 (auto-removal after 3 seconds)
- Expected result: T3 processed first, removed from queue, T1/T2 remain
- Actual result: ✅ T3 marked In Progress, completed, removed; queue now contains only T1, T2

---

## Known Limitations

**Limitations and edge cases:**  

- **No persistence**: Data is session-only; all courses and tasks are lost on exit (PersistenceService exists but is disabled)
- **Single-user only**: No multi-user support or concurrent access handling
- **No undo/redo**: Deletions are permanent (within session); no command history
- **Limited edit UX**: When editing tasks, must re-enter all fields (can't press Enter to keep current value except for some fields)
- **Scale limitations**: ID renumbering is O(n) on delete; would become slow with 1000+ items (designed for <100 tasks)
- **No task dependencies**: Only courses have prerequisites; tasks can't depend on other tasks
- **No date validation beyond format**: Accepts past dates; doesn't warn about unrealistic deadlines
- **Queue reordering is manual**: "Move to top" only; can't reorder entire queue or move to arbitrary positions

## Comparers & String Handling

**Keys comparer:**  

- **Default string equality** (case-sensitive) for all IDs (C1, T1, etc.)
- **Justification**: System-generated IDs have guaranteed consistent casing; no user input for keys means no case ambiguity
- **No custom StringComparer needed**: IDs are controlled (not user-entered), so ordinal comparison is sufficient and performant

**Normalization:**  

- **No automatic trimming**: User input for titles/descriptions preserved as-is
- **Validation instead of normalization**: `InputValidator.GetString()` rejects empty/whitespace-only input with error message
- **ID normalization is system-controlled**: Sequential IDs (C1, C2, T1, T2) are generated by system, not user-entered
- **Case preservation**: User's capitalization in titles/descriptions is maintained (no forced uppercase/lowercase)

---

## Credits & AI Disclosure

**Resources:**  

- Microsoft .NET Documentation (Dictionary, SortedSet, Queue APIs)
- C# Language Reference (LINQ, nullable types, expression-bodied members)
- Graph cycle detection algorithm (standard DFS approach from algorithms textbooks)

**AI usage (if any):**  

- **GitHub Copilot** used for:
  - Code completion suggestions (method signatures, LINQ queries)
  - Generating menu structure and UI formatting helpers (PrintMenu, PrintBox)
  - Refactoring suggestions for cleaner code organization
  - Documentation/comment improvements
- **Verification**:
  - All AI-suggested code was reviewed, tested, and often modified
  - Manual testing of all features to ensure correctness
  - Big-O analysis verified against known complexity for each data structure
  - Prerequisite cycle detection validated with multiple test cases

---

## Challenges and Solutions

**Biggest challenge faced:**  

The biggest challenge was implementing **sequential ID renumbering** while maintaining data consistency across multiple collections. When a course or task is deleted, all subsequent items must be renumbered (e.g., deleting C3 means C4→C3, C5→C4), but tasks also reference course IDs. This required:
1. Tracking insertion order in a List to know which items to renumber
2. Renaming keys in Dictionary structures
3. Updating all references to renamed courses in task objects
4. Maintaining graph edges with new IDs
5. Ensuring SortedSet and Queue remain valid after renumbering

**How you solved it:**  

Solution approach:
1. **Designed RemoveCourseAndShift()** to return a mapping of oldId→newId for all renamed courses
2. **Used the mapping** to update TaskItem.CourseId references in a second pass
3. **Tested with simple examples**: Created 5 courses, deleted C3, verified C4/C5 became C3/C4 and tasks referencing old IDs were updated
4. **Debugger breakpoints** to step through renaming logic and verify each structure was updated correctly
5. **Refactored** to separate concerns: CourseGraph handles course renaming, PlannerService handles task reference updates

This taught me the importance of **maintaining referential integrity** across multiple data structures.

**Most confusing concept:**  

Understanding **when to use Dictionary vs SortedSet vs List** for the same collection of tasks was initially confusing. I needed:
- Fast lookup by ID (Dictionary)
- Priority-ordered iteration (SortedSet)
- Insertion-order tracking (List)

The confusion was resolved by recognizing these aren't mutually exclusive - storing **references** (not copies) in multiple structures provides different "views" of the same data. This is a common pattern in real systems (e.g., databases with indexes) but was counterintuitive at first ("isn't this wasteful?"). Understanding that memory overhead is minimal (just pointers) while speed gains are significant (O(1) vs O(n) lookup) was key.

## Code Quality

**What you're most proud of in your implementation:**  

I'm most proud of the **CourseGraph implementation with cycle detection**. It demonstrates:
- **Proper graph abstraction**: Clean separation of nodes (Dictionary) and edges (adjacency list)
- **Algorithmic correctness**: DFS-based cycle detection prevents invalid prerequisites
- **Real-world relevance**: Mirrors how university systems enforce course prerequisites
- **Elegant error handling**: Automatically rolls back prerequisite if it would create a cycle
- **Complete feature set**: Add/remove prerequisites, get prerequisites, prerequisite visualization

The code is also well-organized with clear **separation of concerns** (Models, Services, UI, DataStructures, Utils), idiomatic C# (expression-bodied members, LINQ, nullable types), and comprehensive **input validation** with helpful error messages.

**What you would improve if you had more time:**  

1. **Better edit UX**: Allow partial field updates (press Enter to keep existing value) instead of requiring full re-entry
2. **Lazy renumbering**: Only renumber IDs when displaying to user, not immediately on delete (better performance)
3. **Undo/redo system**: Stack-based command pattern for undoing accidental deletions
4. **Task-to-task dependencies**: Extend graph to model prerequisite study sessions (e.g., "watch lecture before doing homework")
5. **Persistent storage**: Re-enable JSON save/load to preserve data across sessions
6. **Filter/search**: Find tasks by course, date range, or priority level
7. **Calendar view**: Display tasks in weekly/monthly grid format
8. **Time tracking**: Log actual time spent vs estimated duration
9. **Recurring tasks**: Support daily/weekly recurring study sessions with template system
10. **Performance monitoring**: Add logging to verify Big-O assumptions at scale

## Real-World Applications

**How this relates to real-world systems:**  

Study Planner mirrors several real-world systems:

1. **Task Management Apps** (Todoist, Asana, Trello):
   - Priority queues for task ordering by urgency
   - Dictionary lookups for fast task retrieval
   - Status tracking (Not Started → In Progress → Completed)

2. **University Course Registration Systems**:
   - Graph structures for prerequisite enforcement
   - Cycle detection to prevent circular dependencies
   - Sequential ID generation for course codes

3. **Project Management Software** (Jira, Monday.com):
   - Dependency graphs for task relationships
   - Priority-based scheduling
   - Queue management for daily work items

4. **Calendar/Scheduling Systems** (Google Calendar, Outlook):
   - Date-based prioritization
   - Today's queue for daily agendas
   - Duration estimation and time blocking

The data structure choices directly map to industry patterns: graphs for dependencies, priority queues for scheduling, hash tables for fast lookups, and queues for workflow management.

**What you learned about data structures and algorithms:**  

Key insights:

1. **Multiple data structures for same data**: Storing references in Dictionary + SortedSet + Queue provides O(1) lookup + automatic sorting + FIFO processing with minimal memory overhead

2. **Graph structures are essential for relationships**: Courses with prerequisites can't be modeled with linear structures; graph with cycle detection enforces business rules automatically

3. **Big-O matters at scale**: O(n) renumbering is fine for <100 items but would be problematic at 10,000+ items; design choices depend on expected scale

4. **Custom comparers enable complex sorting**: TaskItemComparer with multi-level logic (date → priority → duration) makes SortedSet automatically maintain priority order

5. **Good key design prevents bugs**: Sequential string IDs (C1, T1) are user-friendly and self-documenting compared to GUIDs or user-entered keys with inconsistent formatting

6. **Tradeoffs are everywhere**: Simplicity (renumbering IDs) vs performance (lazy renumbering), memory (multiple collections) vs speed (O(1) lookup), flexibility (any ID) vs integrity (sequential IDs)

7. **Data structures enable features**: Priority queue enables automatic urgency sorting; graph enables prerequisite validation; Queue enables FIFO workflow - the feature wouldn't exist without the right structure

## Submission Checklist

- ✅ Public GitHub repository link submitted
- ✅ README.md completed (this file)
- ✅ DESIGN.md completed
- ✅Source code included and builds successfully
- N/A (Optional) Slide deck or 5–10 minute demo video link (unlisted)

**Demo Video Link (optional):** N/A
