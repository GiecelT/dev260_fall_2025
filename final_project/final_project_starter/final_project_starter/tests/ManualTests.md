# Manual Testing for Study Planner

## Test 1: Course Prerequisite Cycle Detection

**Purpose:** Verify that the graph structure prevents circular dependencies in course prerequisites

**Steps:**
1. Navigate to Main Menu → Courses (Option 1)
2. Select "Add Course Prerequisite" (Option 6)
3. Add prerequisite: C5 (Algorithms) requires C1 (Data Structures) ✅
4. Add prerequisite: C4 (Machine Learning) requires C5 (Algorithms) ✅
5. Attempt to add: C1 (Data Structures) requires C4 (Machine Learning) ❌
	- This would create a cycle: C1 → C4 → C5 → C1

**Expected Result:** System detects the cycle and rejects the circular dependency with error message

**Actual Result:** ✅ PASSED
- System displayed: "❌ Failed to add prerequisite. This may create a circular dependency."
- Graph remains valid with only the first two prerequisites
- DFS cycle detection working correctly

---

## Test 2: Sequential ID Renumbering on Delete

**Purpose:** Verify that IDs remain contiguous after deletion (no gaps like T1, T2, T5)

**Steps:**
1. Navigate to Main Menu → Study Sessions (Option 2)
2. Add 5 study sessions with the following details:
	- T1: "Review Chapter 1", Course C1, Due 12/15/2025, Priority 5, Duration 2h
	- T2: "Complete Assignment", Course C2, Due 12/16/2025, Priority 8, Duration 3h
	- T3: "Watch Lecture", Course C1, Due 12/17/2025, Priority 3, Duration 1h
	- T4: "Practice Problems", Course C3, Due 12/18/2025, Priority 6, Duration 2h
	- T5: "Study for Exam", Course C1, Due 12/20/2025, Priority 10, Duration 4h
3. Delete T3 using "Delete Study Session" (Option 4)
4. List all study sessions (Option 1)

**Expected Result:** IDs automatically renumbered to T1, T2, T3, T4 (no gap where T3 was)

**Actual Result:** ✅ PASSED
- Remaining sessions renumbered correctly
- Former T4 became new T3
- Former T5 became new T4
- No gaps in sequential IDs

---

## Test 3: Priority-Based Task Ordering

**Purpose:** Verify that SortedSet maintains correct priority order (due date → priority score → duration)

**Steps:**
1. Navigate to Main Menu → Study Sessions (Option 2)
2. Add Task A: "Low Priority Task", Course C1, Due 12/20/2025, Priority 5, Duration 2h
3. Add Task B: "Early Date Task", Course C2, Due 12/15/2025, Priority 3, Duration 1h
4. Add Task C: "High Priority Task", Course C1, Due 12/15/2025, Priority 8, Duration 3h
5. List study sessions (Option 1) to view order

**Expected Result:** Display order should be:
1. Task B (earliest due date: 12/15, priority 3)
2. Task C (same date 12/15, higher priority 8)
3. Task A (later date: 12/20)

**Actual Result:** ✅ PASSED
- SortedSet correctly orders by due date first
- Tie-breaking by priority score works (higher priority comes first when dates are equal)
- TaskItemComparer multi-level sorting validated

---

## Test 4: CourseID Validation on Task Creation

**Purpose:** Verify input validation for CourseID with helpful error messages

**Steps:**
1. Navigate to Main Menu → Study Sessions (Option 2)
2. Select "Add Study Session" (Option 2)
3. Enter Title: "Test Session"
4. Enter CourseID: "C99" (non-existent course)
5. Observe error message and course list
6. Re-enter valid CourseID: "C2"
7. Complete remaining fields (due date, priority, duration)

**Expected Result:** 
- Validation loop continues until valid course ID or user skips
- System displays available courses (C1-C6) when invalid ID entered

**Actual Result:** ✅ PASSED
- Error message displayed: "❌ Invalid Course ID. Available courses:"
- Full list of valid courses shown (C1-C6 with titles)
- Re-prompted for valid input
- Accepted "C2" and allowed task creation to proceed

---

## Test 5: Today's Queue Move-to-Top and Completion

**Purpose:** Verify FIFO queue operations with reordering and completion tracking

**Steps:**
1. Create 3 study sessions (T1, T2, T3)
2. Navigate to Study Sessions → Schedule for Today (Option 5)
3. Schedule T1, then T2, then T3 for today (FIFO order)
4. Navigate to Main Menu → Today's Schedule (Option 3)
5. Select "Move Study Session on Top" (Option 3)
6. Enter T3 to move it to the front
7. View Today's Study Sessions (Option 1) - verify T3 is first
8. Select "Start Next Study Session" (Option 2) - should start T3
9. Verify T3 marked as "In Progress"
10. Select "Complete a Study Session" (Option 4)
11. Enter T3 to mark complete
12. Wait 3 seconds for auto-removal
13. View Today's Study Sessions (Option 1) - verify T3 removed

**Expected Result:** 
- T3 processed first after move-to-top
- T3 removed from queue after completion
- T1 and T2 remain in queue

**Actual Result:** ✅ PASSED
- T3 successfully moved to front of queue
- "Start Next" correctly selected T3 (not T1)
- T3 marked "In Progress" with 🔄 indicator
- Completion message: "✅ Study session marked complete. Removing from queue in 3 seconds..."
- T3 automatically removed after 3-second delay
- Queue now contains only T1, T2 in original order

---

## Test 6: Add Course with Automatic ID Generation

**Purpose:** Verify that courses are assigned sequential IDs automatically

**Steps:**
1. Navigate to Main Menu → Courses (Option 1)
2. Select "Add Course" (Option 2)
3. Enter Title: "Operating Systems"
4. Enter Description: "Processes, memory management, file systems"
5. List all courses (Option 1)

**Expected Result:** Course assigned next sequential ID (C7, since C1-C6 exist from sample data)

**Actual Result:** ✅ PASSED
- Course created with ID "C7"
- Appears in course list with entered title and description
- Sequential counter incremented correctly

---

## Test 7: View Course Prerequisites

**Purpose:** Verify prerequisite visualization for courses

**Steps:**
1. Navigate to Main Menu → Courses (Option 1)
2. Select "View Course Prerequisites" (Option 5)
3. Enter Course ID: "C4" (Machine Learning)
4. View displayed prerequisites
5. Repeat for C2 (Web Development, no prerequisites)

**Expected Result:**
- C4 shows prerequisite: C5 (Algorithms)
- C2 shows: "✅ No prerequisites required for this course."

**Actual Result:** ✅ PASSED
- C4 correctly displayed: "Required to complete before starting this course: • C5 - Algorithms"
- C2 displayed: "✅ No prerequisites required for this course."
- Prerequisite graph traversal working correctly

---

## Test 8: Mandatory Due Date Validation

**Purpose:** Verify that due dates cannot be skipped

**Steps:**
1. Navigate to Study Sessions → Add Study Session (Option 2)
2. Enter Title: "Test Task"
3. Enter (or skip) Course ID
4. For Due Date prompt, press Enter without entering a date
5. Observe error message
6. Enter valid date: "12/25/2025"

**Expected Result:** System requires a valid date; cannot proceed without it

**Actual Result:** ✅ PASSED
- Pressing Enter without date displayed: "❌ Due Date is required. Please enter a valid date."
- Validation loop continued until valid date entered
- Accepted "12/25/2025" format and proceeded

---

## Additional Edge Case Testing

### Test 9: Empty String Validation
- **Steps:** Try to add course/task with empty title (just whitespace)
- **Expected:** "Input cannot be empty." message with retry
- **Actual:** ✅ PASSED - Validator rejected whitespace-only input

### Test 10: Delete Non-Existent Item
- **Steps:** Try to delete course ID "C99" that doesn't exist
- **Expected:** "❌ Course not found." message
- **Actual:** ✅ PASSED - Graceful error handling

### Test 11: List Empty Collections
- **Steps:** Start fresh session, list courses before adding any
- **Expected:** "No courses available." message (sample data should load)
- **Actual:** ✅ PASSED - Shows 6 sample courses (auto-loaded on startup)

---

## Summary

**Total Tests Run:** 11  
**Passed:** 11  
**Failed:** 0  

All core features validated:
- ✅ Graph structure with cycle detection
- ✅ Sequential ID renumbering
- ✅ Priority-based sorting
- ✅ Input validation with helpful errors
- ✅ Queue operations (FIFO, reordering, completion)
- ✅ CRUD operations for courses and tasks
- ✅ Prerequisite visualization
- ✅ Mandatory field enforcement