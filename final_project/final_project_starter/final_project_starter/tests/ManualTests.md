# Manual Testing for Study Planner

## Test 1: Add a Course
- **Steps:** Navigate to Courses → Add Course → Enter ID, Title, Description, Hours
- **Expected:** Course appears in List Courses
- **Actual:** Worked as expected

## Test 2: Add a Task
- **Steps:** Navigate to Tasks → Add Task → Enter Title, Course ID, Due Date, Priority, Duration
- **Expected:** Task appears in List Tasks
- **Actual:** Worked as expected

## Test 3: Schedule Task for Today
- **Steps:** Navigate to Tasks → Schedule for Today → Enter Task ID
- **Expected:** Task moves from Upcoming Tasks to Today’s Queue
- **Actual:** Worked as expected

## Test 4: Start Next Task
- **Steps:** Navigate to Today → Start Next Task
- **Expected:** First task in Today’s Queue is removed and displayed
- **Actual:** Worked as expected

## Test 5: Add a Task, Purposefully leave CourseID blank
- **Steps:**  Navigate to tasks → Add a task → Enter Title, Leave Course ID blank
- **Expected:** Put error message, Display courses available to user either sample or added.
- **Actual:** Worked as expected