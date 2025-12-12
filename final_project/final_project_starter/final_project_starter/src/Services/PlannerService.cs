using System;
using System.Collections.Generic;
using System.Linq;
using StudyPlanner.DataStructures;
using StudyPlanner.Models;
using StudyPlanner.Utils;

namespace StudyPlanner.Services
{
    public class PlannerService
    {
        public CourseGraph Courses { get; } = new CourseGraph();
        public SimplePriorityQueue UpcomingTasks { get; } = new SimplePriorityQueue();
        public Queue<TaskItem> TodaysQueue { get; } = new Queue<TaskItem>();
        public Dictionary<string, TaskItem> TaskLookup { get; } = new Dictionary<string, TaskItem>();
        // Track insertion order for tasks so Task IDs can be sequential and renumbered on delete
        private System.Collections.Generic.List<string> _taskInsertionOrder = new System.Collections.Generic.List<string>();

        private readonly PersistenceService _persistence = new PersistenceService();

        private int _courseCounter = 1;

        // ---------------------------
        // COURSE OPERATIONS
        // ---------------------------

        // UPDATED: automatically generates "C1", "C2", ...
        public void CreateCourse(Course c)
        {
            if (string.IsNullOrWhiteSpace(c.Id))
            {
                c.Id = $"C{_courseCounter++}";
            }

            Courses.AddCourse(c);
        }

        public bool UpdateCourse(Course c)
        {
            if (c.Id == null) return false;
            if (!Courses.ContainsCourse(c.Id)) return false;

            Courses.RemoveCourse(c.Id);
            Courses.AddCourse(c);
            return true;
        }

        public bool DeleteCourse(string courseId)
        {
            // Remove the course and get any renames that happened to keep IDs contiguous.
            var mapping = Courses.RemoveCourseAndShift(courseId);
            var removed = mapping.Count == 0 && !Courses.ContainsCourse(courseId);

            // Update tasks that reference renamed course IDs
            foreach (var kv in mapping)
            {
                var oldId = kv.Key;
                var newId = kv.Value;

                // update TaskItem.CourseId for tasks pointing to oldId
                foreach (var task in TaskLookup.Values)
                {
                    if (task.CourseId == oldId)
                        task.CourseId = newId;
                }
            }

            // If all courses are now deleted, reset the counter to start from C1 again
            if (Courses.ListAllCourses().Count == 0)
            {
                _courseCounter = 1;
                IdGenerator.LoadCounters(new System.Collections.Generic.Dictionary<string, int> { { "C", 0 } });
            }

            // If mapping empty but course was removed, return true; otherwise true if removal happened.
            return removed || mapping.Count >= 0;
        }

        public Course? GetCourse(string courseId) =>
            Courses.ListAllCourses().FirstOrDefault(c => c.Id == courseId);

        public List<Course> ListCourses() => Courses.ListAllCourses();

        public bool AddPrerequisite(string courseId, string prereqId) =>
            Courses.AddPrerequisite(courseId, prereqId);

        public bool RemovePrerequisite(string courseId, string prereqId) =>
            Courses.RemovePrerequisite(courseId, prereqId);

        public List<Course> GetPrerequisites(string courseId) =>
            Courses.GetPrerequisites(courseId);


        // ---------------------------
        // TASK OPERATIONS
        // ---------------------------
        public void AddTask(TaskItem t)
        {
            if (string.IsNullOrWhiteSpace(t.Id))
            {
                t.Id = IdGenerator.GenerateTaskId();
            }

            TaskLookup[t.Id!] = t;
            UpcomingTasks.Enqueue(t);
            if (!_taskInsertionOrder.Contains(t.Id))
                _taskInsertionOrder.Add(t.Id!);
        }

        public bool UpdateTask(TaskItem t)
        {
            if (t.Id == null) return false;
            if (!TaskLookup.ContainsKey(t.Id.ToString())) return false;

            TaskLookup[t.Id.ToString()] = t;
            UpcomingTasks.Remove(t.Id.ToString());
            UpcomingTasks.Enqueue(t);

            return true;
        }

        public bool DeleteTask(string taskId)
        {
            if (!_taskInsertionOrder.Contains(taskId)) return false;

            // find index and remove
            var idx = _taskInsertionOrder.IndexOf(taskId);
            if (idx < 0) return false;

            // remove the task itself
            _taskInsertionOrder.RemoveAt(idx);
            if (TaskLookup.ContainsKey(taskId))
                TaskLookup.Remove(taskId);
            UpcomingTasks.Remove(taskId);

            // remove from today's queue
            var todayList = TodaysQueue.ToList();
            todayList.RemoveAll(t => t.Id != null && t.Id == taskId);
            TodaysQueue.Clear();
            foreach (var t in todayList)
                TodaysQueue.Enqueue(t);

            // renumber subsequent tasks (shift IDs down)
            for (int i = idx; i < _taskInsertionOrder.Count; i++)
            {
                var oldId = _taskInsertionOrder[i];
                // parse prefix and number
                var prefix = new string(oldId.TakeWhile(c => !char.IsDigit(c)).ToArray());
                var numberPart = new string(oldId.SkipWhile(c => !char.IsDigit(c)).ToArray());
                if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(numberPart))
                    continue;
                if (!int.TryParse(numberPart, out var n)) continue;

                var newId = prefix + (n - 1).ToString();

                // update TaskItem object
                if (TaskLookup.ContainsKey(oldId))
                {
                    var task = TaskLookup[oldId];
                    TaskLookup.Remove(oldId);
                    task.Id = newId;
                    TaskLookup[newId] = task;
                }

                // update _taskInsertionOrder entry
                _taskInsertionOrder[i] = newId;
            }

            // sync IdGenerator counter for tasks to current count
            var counters = IdGenerator.GetCounters();
            counters["T"] = _taskInsertionOrder.Count;
            IdGenerator.LoadCounters(counters);

            return true;
        }

        public List<TaskItem> ListUpcomingTasks() => _taskInsertionOrder
            .Select(id => TaskLookup.ContainsKey(id) ? TaskLookup[id] : null)
            .Where(t => t != null)
            .Select(t => t!)
            .ToList();

        public void ScheduleForToday(string taskId)
        {
            if (!TaskLookup.ContainsKey(taskId)) return;

            var t = TaskLookup[taskId];
            UpcomingTasks.Remove(taskId);
            TodaysQueue.Enqueue(t);
        }

        public TaskItem? NextTodayTask()
        {
            if (TodaysQueue.Count == 0) return null;
            var first = TodaysQueue.Peek();
            if (first != null)
                first.InProgress = true;
            return first;
        }

        public void MarkTaskComplete(string taskId)
        {
            if (!TaskLookup.ContainsKey(taskId)) return;
            TaskLookup[taskId].Completed = true;
        }

        // ---------------------------
        // PERSISTENCE OPERATIONS
        // ---------------------------
        public void Save(string path)
        {
            var counters = IdGenerator.GetCounters();

            // save tasks in insertion order so they can be restored with the same ordering
            var tasksOrdered = _taskInsertionOrder.Select(id => TaskLookup.ContainsKey(id) ? TaskLookup[id] : null).Where(t => t != null).Select(t => t!).ToList();

            _persistence.Save(
                path,
                Courses.ListAllCourses(),
                tasksOrdered,
                TodaysQueue,
                counters
            );
        }

        public void Load(string path)
        {
            var (courses, tasks, todayQueue, counters) = _persistence.Load(path);

            // load persisted counters into IdGenerator
            IdGenerator.LoadCounters(counters);

            // ensure internal course counter stays ahead of persisted counter for "C"
            if (counters != null && counters.TryGetValue("C", out var persistedC) && persistedC + 1 > _courseCounter)
            {
                _courseCounter = persistedC + 1;
            }

            foreach (var c in courses)
            {
                // ensure counter stays ahead of max loaded ID
                if (c.Id != null && c.Id.StartsWith("C"))
                {
                    if (int.TryParse(c.Id.Substring(1), out int n) && n >= _courseCounter)
                        _courseCounter = n + 1;
                }

                Courses.AddCourse(c);
            }

            // Load tasks, sanitize IDs that don't match the expected sequential format (T1, T2, ...)
            var maxTaskNum = 0;
            var regex = new System.Text.RegularExpressions.Regex("^T([1-9][0-9]*)$");
            foreach (var t in tasks)
            {
                if (string.IsNullOrWhiteSpace(t.Id) || !regex.IsMatch(t.Id))
                {
                    // assign a fresh sequential task id
                    t.Id = IdGenerator.GenerateTaskId();
                }
                else
                {
                    var m = regex.Match(t.Id);
                    if (m.Success && int.TryParse(m.Groups[1].Value, out var n))
                        maxTaskNum = Math.Max(maxTaskNum, n);
                }

                AddTask(t);
            }

            // Ensure IdGenerator's T counter is at least the max found
            var currentCounters = IdGenerator.GetCounters();
            if (!currentCounters.ContainsKey("T") || currentCounters["T"] < maxTaskNum)
            {
                currentCounters["T"] = maxTaskNum;
                IdGenerator.LoadCounters(currentCounters);
            }

            foreach (var t in todayQueue)
                TodaysQueue.Enqueue(t);
        }

        internal bool UpdateCourse(string upId, string upTitle, string upDesc)
        {
            throw new NotImplementedException();
        }
    }
}
