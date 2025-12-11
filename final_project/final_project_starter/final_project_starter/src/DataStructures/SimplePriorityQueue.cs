using System.Collections.Generic;
using System.Linq;
using StudyPlanner.Models;
using StudyPlanner.Utils;

namespace StudyPlanner.DataStructures
{
    public class SimplePriorityQueue
    {
        private SortedSet<TaskItem> set;

        public SimplePriorityQueue()
        {
            set = new SortedSet<TaskItem>(new TaskItemComparer());
        }

        public void Enqueue(TaskItem item) => set.Add(item);

        public TaskItem Dequeue()
        {
            if (!set.Any()) return null!;
            var first = set.First();
            set.Remove(first);
            return first;
        }

        public TaskItem? Peek() => set.FirstOrDefault();
        public int Count => set.Count;

        public List<TaskItem> ToList() => set.ToList();
        public bool Remove(string taskId)
        {
            var item = set.FirstOrDefault(t => t.Id == taskId);
            if (item == null) return false;
            return set.Remove(item);
        }
    }
}
