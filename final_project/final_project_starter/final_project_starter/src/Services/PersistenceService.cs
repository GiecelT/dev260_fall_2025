using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using StudyPlanner.Models;

namespace StudyPlanner.Services
{
    public class PersistenceService
    {
        public void Save(string path, List<Course> courses, List<TaskItem> tasks, Queue<TaskItem> todaysQueue, System.Collections.Generic.Dictionary<string, int> counters)
        {
            var obj = new
            {
                courses,
                tasks,
                // store only the IDs for today's queue
                todaysQueue = todaysQueue.Select(t => t.Id).ToList()
                ,
                counters
            };

            var json = JsonSerializer.Serialize(obj,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(path, json);
        }

        public (List<Course>, List<TaskItem>, Queue<TaskItem>, System.Collections.Generic.Dictionary<string, int>) Load(string path)
        {
            var courses = new List<Course>();
            var tasks = new List<TaskItem>();
            var todaysQueue = new Queue<TaskItem>();
            var counters = new System.Collections.Generic.Dictionary<string, int>();

            if (!File.Exists(path))
                return (courses, tasks, todaysQueue, counters);

            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            // -------------------------
            // LOAD COURSES
            // -------------------------
            if (root.TryGetProperty("courses", out var coursesJson))
            {
                foreach (var c in coursesJson.EnumerateArray())
                {
                    courses.Add(new Course
                    {
                        Id = c.GetProperty("Id").GetString() ?? "",
                        Title = c.GetProperty("Title").GetString(),
                        Description = c.GetProperty("Description").GetString()
                    });
                }
            }

            // -------------------------
            // LOAD TASKS
            // -------------------------
            if (root.TryGetProperty("tasks", out var tasksJson))
            {
                foreach (var t in tasksJson.EnumerateArray())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = t.GetProperty("Id").GetString()!,
                        Title = t.GetProperty("Title").GetString(),
                        CourseId = t.GetProperty("CourseId").GetString(),
                        DueDate =
                            t.GetProperty("DueDate").ValueKind == JsonValueKind.Null
                                ? null
                                : t.GetProperty("DueDate").GetDateTime(),
                        PriorityScore = t.GetProperty("PriorityScore").GetInt32(),
                        EstimatedDuration =
                            TimeSpan.TryParse(
                                t.GetProperty("EstimatedDuration").GetString(),
                                out var duration)
                                ? duration
                                : TimeSpan.Zero,
                        Completed = t.GetProperty("Completed").GetBoolean()
                    });
                }
            }

            // -------------------------
            // LOAD TODAY'S QUEUE
            // -------------------------
            if (root.TryGetProperty("todaysQueue", out var queueJson))
            {
                foreach (var idEl in queueJson.EnumerateArray())
                {
                    var id = idEl.GetString();
                    if (!string.IsNullOrEmpty(id))
                    {
                        var task = tasks.FirstOrDefault(t => t.Id == id);
                        if (task != null)
                            todaysQueue.Enqueue(task);
                    }
                }
            }

            // -------------------------
            // LOAD COUNTERS
            // -------------------------
            if (root.TryGetProperty("counters", out var countersJson) && countersJson.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in countersJson.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.TryGetInt32(out var n))
                        counters[prop.Name] = n;
                }
            }

            return (courses, tasks, todaysQueue, counters);
        }
    }
}
