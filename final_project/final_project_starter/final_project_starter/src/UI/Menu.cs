using System;
using StudyPlanner.Services;
using StudyPlanner.Models;
using StudyPlanner.Utils;

namespace StudyPlanner.UI
{
    public class Menu
    {
        private readonly PlannerService _planner;
        private bool _isFirstMenuDisplay = true;

        public Menu(PlannerService planner)
        {
            _planner = planner;
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                if (!_isFirstMenuDisplay)
                    Console.Clear();
                _isFirstMenuDisplay = false;

                PrintMenu("🎯 Study Planner", new string[] {
                    "1️⃣  Courses",
                    "2️⃣  Study Sessions",
                    "3️⃣  Today's Schedule",
                    "4️⃣  Quit"
                });
                var choice = InputValidator.GetString("Select an option: ");
                switch (choice)
                {
                    case "1": ShowCoursesMenu(); break;
                    case "2": ShowTasksMenu(); break;
                    case "3": ShowTodayMenu(); break;
                    case "4": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        public void ShowCoursesMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintMenu("📚 Courses", new string[] {
                    "1️⃣  List Courses",
                    "2️⃣  Add Course",
                    "3️⃣  Update Course",
                    "4️⃣  Delete Course",
                    "5️⃣  Back"
                });
                var choice = InputValidator.GetString("Select: ");
                switch (choice)
                {
                    case "1":
                        var courses = _planner.ListCourses();
                        if (courses.Count == 0)
                        {
                            PrintBox("No courses available.");
                        }
                        else
                        {
                            PrintBox("📚 Courses List");
                            foreach (var c in courses)
                            {
                                Console.WriteLine($"• {c.Id}  —  {c.Title} {(string.IsNullOrWhiteSpace(c.Description) ? "" : "| " + c.Description)}");
                            }
                            Console.WriteLine();
                        }
                        Pause();
                        break;
                    case "2":
                        var id = Utils.IdGenerator.GenerateCourseId();
                        var title = InputValidator.GetString("Title: ");
                        var desc = InputValidator.GetString("Description: ");
                        _planner.CreateCourse(new Course { Id = id, Title = title, Description = desc });
                        Console.WriteLine("✅ Course added.");
                        Pause();
                        break;
                    case "3":
                        var upId = InputValidator.GetString("Course ID to update: ");
                        var upTitle = InputValidator.GetString("New Title: ");
                        var upDesc = InputValidator.GetString("New Description: ");
                        var updatedCourse = new Course { Id = upId, Title = upTitle, Description = upDesc };
                        if (_planner.UpdateCourse(updatedCourse))
                            Console.WriteLine("✅ Course updated.");
                        else
                            Console.WriteLine("❌ Course not found.");
                        Pause();
                        break;
                    case "4":
                        var delId = InputValidator.GetString("Course ID to delete: ");
                        if (_planner.DeleteCourse(delId))
                            Console.WriteLine("🗑️ Course deleted.");
                        else
                            Console.WriteLine("❌ Course not found.");
                        Pause();
                        break;
                    case "5": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
        }

        private void ShowTasksMenu()
        {
            while (true)
    {
        Console.Clear();
        PrintMenu("📝 Study Sessions", new string[] {
            "1️⃣  List Study Sessions",
            "2️⃣  Add Study Task",
            "3️⃣  Edit Study Task",
            "4️⃣  Delete Study Task",
            "5️⃣  Schedule Study Task for Today",
            "6️⃣  Back to Main Menu"
        });

        var choice = InputValidator.GetString("\nSelect: ");
        switch (choice)
        {
            case "1":
                var upcoming = _planner.ListUpcomingTasks();
                if (upcoming == null || upcoming.Count == 0)
                {
                    PrintBox("No upcoming tasks.");
                }
                else
                {
                    PrintBox("📌 Upcoming Tasks");
                    foreach (var t in upcoming)
                    {
                        var status = t.InProgress ? "🔄 In Progress" : (t.Completed ? "✅ Completed" : "⏳ Not Started");
                        Console.WriteLine($"- {t.Id} | {t.Title} | Priority: {t.PriorityScore} | Due: {(t.DueDate.HasValue ? t.DueDate.Value.ToString("MM-dd-yyyy") : "None")} | Status: {status}");
                    }
                    Console.WriteLine();
                }
                Pause();
                break;

            case "2":
                var taskId = Utils.IdGenerator.GenerateTaskId();
                var title = InputValidator.GetString("Title: ");
                
                // Validate CourseID against existing courses
                string courseId = "";
                while (true)
                {
                    courseId = InputValidator.GetString("Course ID: ");
                    if (string.IsNullOrWhiteSpace(courseId))
                        break; // User skipped, leave as null
                    
                    var courses = _planner.ListCourses();
                    if (courses.Any(c => c.Id == courseId))
                        break; // Valid course ID found
                    
                    // Invalid course ID, show available courses
                    Console.WriteLine("\n❌ Invalid Course ID. Available courses:");
                    foreach (var c in courses)
                        Console.WriteLine($"  • {c.Id} - {c.Title}");
                    Console.WriteLine();
                }
                
                DateTime due;
                while (true)
                {
                    var dueParsed = InputValidator.GetDate("Due Date (MM-DD-YYYY): ");
                    if (dueParsed.HasValue)
                    {
                        due = dueParsed.Value;
                        break;
                    }
                    Console.WriteLine("❌ Due Date is required. Please enter a valid date.");
                }
                var prio = InputValidator.GetInt("Priority Score (higher = urgent): ");
                var durHours = InputValidator.GetInt("Estimated Duration Hours: ");
                var durMins = InputValidator.GetInt("Estimated Duration Minutes: ");

                _planner.AddTask(new TaskItem
                {
                    Id = taskId,
                    Title = title,
                    CourseId = string.IsNullOrWhiteSpace(courseId) ? null : courseId,
                    DueDate = due,
                    PriorityScore = prio,
                    EstimatedDuration = new TimeSpan(durHours, durMins, 0),
                    Completed = false
                });

                Console.WriteLine($"✅ Task {taskId} added: {title}");
                Pause();
                break;

            case "3":
                var editTaskId = InputValidator.GetString("Task ID to edit: ");
                if (!_planner.TaskLookup.ContainsKey(editTaskId))
                {
                    Console.WriteLine("❌ Task not found.");
                    Pause();
                    break;
                }

                var taskToEdit = _planner.TaskLookup[editTaskId];
                Console.WriteLine($"\n📝 Editing Task {editTaskId}: {taskToEdit.Title}");

                var newTitle = InputValidator.GetString($"Title [{taskToEdit.Title}]: ");
                if (!string.IsNullOrWhiteSpace(newTitle))
                    taskToEdit.Title = newTitle;

                var newCourseId = InputValidator.GetString($"Course ID [{taskToEdit.CourseId ?? "None"}]: ");
                if (!string.IsNullOrWhiteSpace(newCourseId) && newCourseId != "None")
                    taskToEdit.CourseId = newCourseId;

                DateTime newDue;
                while (true)
                {
                    var newDueParsed = InputValidator.GetDate($"Due Date [{(taskToEdit.DueDate.HasValue ? taskToEdit.DueDate.Value.ToString("MM-dd-yyyy") : "None")}] (MM-DD-YYYY): ");
                    if (newDueParsed.HasValue)
                    {
                        newDue = newDueParsed.Value;
                        break;
                    }
                    Console.WriteLine("❌ Due Date is required. Please enter a valid date.");
                }
                taskToEdit.DueDate = newDue;

                var newPrio = InputValidator.GetInt($"Priority Score [{taskToEdit.PriorityScore}]: ");
                if (newPrio >= 0)
                    taskToEdit.PriorityScore = newPrio;

                var newDurHours = InputValidator.GetInt($"Estimated Duration Hours [{taskToEdit.EstimatedDuration.Hours}]: ");
                var newDurMins = InputValidator.GetInt($"Estimated Duration Minutes [{taskToEdit.EstimatedDuration.Minutes}]: ");
                if (newDurHours >= 0 || newDurMins >= 0)
                    taskToEdit.EstimatedDuration = new TimeSpan(newDurHours >= 0 ? newDurHours : taskToEdit.EstimatedDuration.Hours, newDurMins >= 0 ? newDurMins : taskToEdit.EstimatedDuration.Minutes, 0);

                _planner.UpdateTask(taskToEdit);
                Console.WriteLine("✅ Task updated.");
                Pause();
                break;

            case "4":
                var delId = InputValidator.GetString("Task ID to delete: ");
                if (_planner.DeleteTask(delId))
                    Console.WriteLine("🗑️ Task deleted.");
                else
                    Console.WriteLine("❌ Could not find task.");
                Pause();
                break;

            case "5":
                var schedId = InputValidator.GetString("Task ID to schedule for today: ");
                _planner.ScheduleForToday(schedId);
                Console.WriteLine("📅 Task scheduled for today.");
                Pause();
                break;

            case "6":
                return;

            default:
                Console.WriteLine("❌ Invalid choice.");
                Pause();
                break;
        }
    }
}


        private void ShowTodayMenu()
{
    while (true)
    {
        Console.Clear();
        PrintMenu("🕒 Today's Schedule", new string[] {
            "1️⃣  View Today's Schedule",
            "2️⃣  Start Next Task",
            "3️⃣  Move Task on Top",
            "4️⃣  Complete a Task",
            "5️⃣  Back to Main Menu"
        });

        var choice = InputValidator.GetString("\nSelect: ");

        switch (choice)
        {
            case "1":
                Console.WriteLine("\n📋 Tasks for Today:");
                if (_planner.TodaysQueue.Count == 0)
                {
                    Console.WriteLine("No tasks scheduled for today.");
                }
                else
                {
                    foreach (var t in _planner.TodaysQueue)
                    {
                        var status = t.Completed ? "✅ Completed" : (t.InProgress ? "🔄 In Progress" : "⏳ Not started");
                        Console.WriteLine($"- {t.Id} | {t.Title} | Status: {status}");
                    }
                }
                Pause();
                break;

            case "2":
                var next = _planner.NextTodayTask();
                if (next == null)
                    Console.WriteLine("🎉 No tasks left for today!");
                else
                {
                    next.InProgress = true;
                    Console.WriteLine($"🚀 Starting task: {next.Title}");
                }
                Pause();
                break;

            case "3":
                var moveTaskId = InputValidator.GetString("Task ID to move on top: ");
                var todayList = _planner.TodaysQueue.ToList();
                var taskToMove = todayList.FirstOrDefault(t => t.Id == moveTaskId);
                if (taskToMove == null)
                {
                    Console.WriteLine("❌ Task not found in today's queue.");
                }
                else
                {
                    _planner.TodaysQueue.Clear();
                    _planner.TodaysQueue.Enqueue(taskToMove);
                    foreach (var t in todayList.Where(t => t.Id != moveTaskId))
                        _planner.TodaysQueue.Enqueue(t);
                    Console.WriteLine($"⬆️ Task {moveTaskId} moved to top.");
                }
                Pause();
                break;

            case "4":
                var completeTaskId = InputValidator.GetString("Task ID to mark complete: ");
                if (_planner.TaskLookup.ContainsKey(completeTaskId))
                {
                    _planner.MarkTaskComplete(completeTaskId);
                    Console.WriteLine("✔️ Task marked complete. Removing from queue in 3 seconds...");
                    System.Threading.Thread.Sleep(3000);
                    
                    // Remove the completed task from today's queue
                    var updatedList = _planner.TodaysQueue.ToList();
                    updatedList.RemoveAll(t => t.Id == completeTaskId);
                    _planner.TodaysQueue.Clear();
                    foreach (var t in updatedList)
                        _planner.TodaysQueue.Enqueue(t);
                }
                else
                {
                    Console.WriteLine("❌ Task not found.");
                    Pause();
                }
                break;

            case "5":
                return;

            default:
                Console.WriteLine("❌ Invalid choice.");
                Pause();
                break;
        }
    }
}

        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void PrintMenu(string title, string[] options)
        {
            int width = Math.Max(title.Length + 4, 44);
            foreach (var opt in options)
                width = Math.Max(width, opt.Length + 6);

            var horiz = "+" + new string('-', width - 2) + "+";
            Console.WriteLine(horiz);
            Console.WriteLine("| " + title.PadRight(width - 4) + " |");
            Console.WriteLine(horiz);
            foreach (var opt in options)
            {
                Console.WriteLine("| " + opt.PadRight(width - 4) + " |");
            }
            Console.WriteLine(horiz);
        }

        private void PrintBox(string title)
        {
            int width = Math.Max(title.Length + 4, 40);

            Console.WriteLine("+" + new string('-', width - 2) + "+");
            Console.WriteLine("| " + title.PadRight(width - 4) + " |");
            Console.WriteLine("+" + new string('-', width - 2) + "+");
        }
    }
}
