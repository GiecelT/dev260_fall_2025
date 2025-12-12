# Project Design & Rationale

**Instructions:** Replace prompts with your content. Be specific and concise. If something doesn't apply, write "N/A" and explain briefly.

---

## Data Model & Entities

**Core entities:**  

**Entity A:**

- Name: Course
- Key fields: Id (string), Title (string), Description (string)
- Identifiers: Sequential string ID (C1, C2, C3, ...)
- Relationships: Can have prerequisite courses (many-to-many via graph edges); TaskItems reference courses via CourseId

**Entity B:**

- Name: TaskItem (Study Session)
- Key fields: Id (string), Title (string), CourseId (string, nullable), DueDate (DateTime), PriorityScore (int), EstimatedDuration(TimeSpan), Completed (bool), InProgress (bool)
- Identifiers: Sequential string ID (T1, T2, T3, ...)
- Relationships: Optionally belongs to one Course; stored in multiple collections (priority queue, today's queue, lookup dictionary)

**Identifiers (keys) and why they're chosen:**  

Sequential string IDs (C1, C2, T1, T2) were chosen for:
- **User readability**: Easy to reference (e.g., "delete T3" vs "delete a8f2-3b9c-...")
- **Sequential numbering**: Maintains contiguous IDs even after deletions (T1, T2, T4 becomes T1, T2, T3)
- **Type distinction**: Prefix distinguishes courses (C) from tasks (T)
- **Simplicity**: No GUID complexity; works well for session-based data (not persisted to disk)
- **Predictability**: Users know C1 is the first course added, T1 is first task added

---

## Data Structures — Choices & Justification

### Structure #1

**Chosen Data Structure:**  
CourseGraph (custom directed graph: Dictionary<string, Course> nodes + Dictionary<string, List<string>> edges)

**Purpose / Role in App:**  
Manages courses and their prerequisite relationships. Powers "View Prerequisites" and "Add Prerequisite" features with cycle detection to prevent circular dependencies.

**Why it fits:**  
- **Access Pattern**: Fast course lookup by ID (O(1)), prerequisite traversal, cycle detection via DFS
- **Constraint Match**: Graph naturally represents dependency relationships; DAG enforcement prevents invalid prerequisites
- **Performance**: O(1) course access, O(V+E) cycle detection where V=courses, E=prerequisites (small scale: ~10 courses, <20 edges)
- **Memory**: Minimal overhead; adjacency list representation is space-efficient

**Alternatives considered:**  
- **Simple List<Course>**: No way to represent prerequisites; would need separate prerequisite storage
- **Tree structure**: Too restrictive; courses can have multiple prerequisites (not strictly hierarchical)
- **Nested Course objects**: Would create circular references and complicate serialization

---

### Structure #2

**Chosen Data Structure:**  
Dictionary<string, TaskItem> (TaskLookup)

**Purpose / Role in App:**  
Enables instant task retrieval by ID for edit, delete, complete, and schedule operations.

**Why it fits:**  
- **Access Pattern**: Direct lookup by task ID (user enters "T5" to edit/delete)
- **Performance**: O(1) lookup, insert, update, delete
- **Typical Size**: 10-100 tasks expected
- **Simplicity**: Standard .NET collection, no custom implementation needed

**Alternatives considered:**  
- **List<TaskItem>**: O(n) search to find task by ID; inefficient for frequent lookups
- **SortedDictionary**: Unnecessary overhead; tasks don't need sorted key access (priority queue handles sorting)
- **Custom hash table**: Dictionary is already optimized; no benefit to reimplementing

---

### Structure #3

**Chosen Data Structure:**  
SortedSet<TaskItem> (wrapped by SimplePriorityQueue)

**Purpose / Role in App:**  
Maintains all upcoming tasks in priority order (by due date → priority score → duration). Powers "List Study Sessions" with automatic sorting.

**Why it fits:**  
- **Access Pattern**: Insert tasks, retrieve in priority order, remove by ID
- **Performance**: O(log n) insert/remove, O(n) iteration for display
- **Automatic sorting**: Custom TaskItemComparer ensures tasks stay ordered without manual sorting
- **Simplicity**: SortedSet handles balancing and ordering; no custom tree implementation needed

**Alternatives considered:**  
- **List<TaskItem> + manual sorting**: O(n log n) sort on every display; inefficient for frequent updates
- **Priority queue (heap)**: Better for pure dequeue operations, but we need arbitrary removal by ID and full iteration
- **Custom BST**: More complex to implement; SortedSet provides same functionality with better testing/reliability


**Alternatives considered:**  
- **List<TaskItem> + manual sorting**: O(n log n) sort on every display; inefficient for frequent updates
- **Priority queue (heap)**: Better for pure dequeue operations, but we need arbitrary removal by ID and full iteration
- **Custom BST**: More complex to implement; SortedSet provides same functionality with better testing/reliability

---

### Additional Structures (if applicable)

**Structure #4: Queue<TaskItem> (TodaysQueue)**

- **Purpose**: FIFO queue for tasks scheduled for today; supports sequential processing
- **Why**: Natural fit for "start next task" workflow; maintains order while allowing reordering (move to top)
- **Performance**: O(1) enqueue/dequeue, O(n) iteration for display
- **Alternative**: List would work but Queue semantically signals intent (first-in-first-processed)

**Structure #5: List<string> (_taskInsertionOrder & CourseGraph.insertionOrder)**

- **Purpose**: Track creation order to enable sequential ID renumbering when items are deleted
- **Why**: When T3 is deleted, T4+ must shift to T3, T4... to keep IDs contiguous; List tracks positions
- **Performance**: O(n) removal + renumbering, but happens infrequently (only on delete)
- **Alternative**: No renumbering (accept gaps like T1, T2, T5) - rejected for poor UX and confusing IDs

---

## Comparers & String Handling

**Comparer choices:**  

**For keys:**
- String IDs use default string equality (case-sensitive)
- Sequential IDs (C1, T1) are system-generated, so case consistency is guaranteed
- No need for case-insensitive comparison since users don't manually enter IDs in ambiguous casing

**For display sorting (if different):**
- **TaskItemComparer** (custom IComparer<TaskItem>) for priority ordering:
  - Primary: Due date (earliest first) using `Nullable.Compare<DateTime>`
  - Secondary: Priority score (highest first) - inverted comparison for descending order
  - Tertiary: Estimated duration (shortest first) for tie-breaking
- Enables SortedSet to automatically maintain task priority without manual sorting

**Normalization rules:**  

- **Input validation**: `InputValidator.GetString()` rejects empty/whitespace-only strings
- **No automatic trimming**: User input accepted as-is to preserve intentional formatting
- **No case normalization**: Titles and descriptions maintain user's capitalization (case-sensitive display)
- **ID generation**: System-controlled (C1, T1) - no user input, so no normalization needed

**Bad key examples avoided:**  

- **Course/Task titles as keys**: ❌ Non-unique (two courses could have same name), mutable (user can edit titles)
- **GUIDs**: ❌ Not user-friendly ("delete a8f2-3b9c-..." vs "delete T3")
- **Auto-increment integers**: ❌ Type confusion (Course 1 vs Task 1 - no distinction)
- **User-entered IDs**: ❌ Prone to typos, inconsistent formatting, trailing spaces
- **Composite keys**: ❌ Unnecessary complexity for simple CRUD operations

**Chosen solution**: Sequential prefixed string IDs (C1, T1) balance readability, uniqueness, and system control.

---

## Performance Considerations

**Expected data scale:**  

- **Courses**: 5-20 courses per semester
- **Tasks/Study Sessions**: 20-100 active tasks (typical student workload)
- **Today's Queue**: 3-10 tasks scheduled per day
- **Prerequisites**: 1-3 prerequisites per course (total ~15-40 edges in graph)

All operations designed for **small-scale, single-user session data** (not enterprise-scale).

**Performance bottlenecks identified:**  

1. **Task renumbering on delete**: O(n) to renumber all tasks after deleted ID
   - **Mitigation**: Acceptable for small scale (<100 tasks); user benefits (contiguous IDs) outweigh cost
   
2. **Cycle detection on prerequisite add**: O(V+E) DFS traversal
   - **Mitigation**: Small graph (<20 courses, <40 edges); traversal is near-instant
   
3. **Full list iteration for display**: O(n) to display all tasks/courses
   - **Mitigation**: Inevitable for console display; SortedSet avoids additional sorting overhead

4. **Multiple collections for same tasks**: Dictionary + SortedSet + Queue hold references
   - **Mitigation**: References only, not copies; minimal memory overhead (<1KB per task)

**Big-O analysis of core operations:**  

**Courses:**
- Add: O(1) - Dictionary insert + List append
- Search: O(1) - Dictionary lookup by ID
- List: O(n) - Iterate insertion order list
- Update: O(1) - Remove + Add (both O(1))
- Delete: O(m) - Remove + renumber subsequent IDs (m = courses after deleted one)

**Tasks/Study Sessions:**
- Add: O(log n) - SortedSet insert (tree balancing) + O(1) Dictionary insert
- Search: O(1) - Dictionary lookup by ID
- List: O(n) - Iterate SortedSet in priority order (already sorted)
- Update: O(log n) - Remove + re-insert into SortedSet (priorities may change)
- Delete: O(n) - Remove from SortedSet O(log n) + renumber all subsequent tasks O(n)

**Prerequisites:**
- Add prerequisite: O(V+E) - DFS cycle detection
- Get prerequisites: O(k) - Retrieve k prerequisites for one course
- Remove prerequisite: O(1) - List removal from adjacency list

---

## Design Tradeoffs & Decisions

**Key design decisions:**  

1. **Sequential IDs with renumbering** (vs. GUIDs or gaps)
   - **Decision**: Maintain contiguous C1-Cn, T1-Tn even after deletions
   - **Why**: User-friendly, readable, predictable; worth O(n) renumbering cost for small scale

2. **Session-only data** (vs. persistent storage)
   - **Decision**: Sample data loaded on startup; no save to disk
   - **Why**: Simplifies demo/testing; focuses on data structures over file I/O
   - **Note**: PersistenceService exists but is disabled in current version

3. **Graph with cycle detection** (vs. simple prerequisite list)
   - **Decision**: Enforce DAG (directed acyclic graph) for prerequisites
   - **Why**: Prevents circular dependencies (A requires B, B requires A); validates data integrity

4. **Multiple collections for tasks** (Dictionary + SortedSet + Queue)
   - **Decision**: Store task references in 3 structures simultaneously
   - **Why**: Each serves different access pattern (lookup, priority order, today's FIFO); memory tradeoff for speed

5. **Custom comparer for priority** (vs. manual sorting)
   - **Decision**: TaskItemComparer with multi-level sorting (date → priority → duration)
   - **Why**: Automatic ordering in SortedSet; no need to re-sort on every display

**Tradeoffs made:**  

1. **Simplicity vs. Performance**:
   - **Chose simplicity**: O(n) renumbering on delete instead of complex ID management
   - **Justification**: Code clarity > micro-optimization for small datasets

2. **Memory vs. Speed**:
   - **Chose speed**: Store task references in 3 collections (Dictionary + SortedSet + Queue)
   - **Tradeoff**: ~3x memory for references, but O(1) lookup + O(log n) priority access + O(1) queue operations

3. **Flexibility vs. Constraints**:
   - **Chose constraints**: Sequential IDs, mandatory due dates, DAG enforcement
   - **Benefit**: Data integrity and predictable UX > unrestricted input

4. **Realism vs. Demo Focus**:
   - **Chose demo focus**: Session-based data instead of persistent storage
   - **Tradeoff**: Easier testing/grading, but data lost on exit

**What you would do differently with more time:**  

1. **Lazy renumbering**: Only renumber IDs when necessary (e.g., on display) instead of immediately on delete

2. **Undo/Redo**: Add Stack<Action> for command history to undo accidental deletions

3. **Task dependencies**: Extend graph to include task-to-task prerequisites (not just courses)

4. **Better edit UX**: Allow partial edits (press Enter to keep current value) instead of requiring re-entry

5. **Search/filter**: Add ability to filter tasks by course, date range, or priority level

6. **Recurring tasks**: Support weekly/daily recurring study sessions with template system

7. **Calendar view**: Display tasks in a weekly/monthly calendar format instead of just lists

8. **Progress tracking**: Add completion percentages for courses based on completed tasks

9. **Time tracking**: Log actual time spent vs. estimated duration for sessions

10. **Persistent storage**: Re-enable JSON persistence for data to survive restarts
