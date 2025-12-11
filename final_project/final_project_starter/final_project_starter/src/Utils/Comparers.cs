using System;
using System.Collections.Generic;
using StudyPlanner.Models;

namespace StudyPlanner.Utils
{
    public class TaskItemComparer : IComparer<TaskItem>
    {
        public int Compare(TaskItem? x, TaskItem? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            int dateCompare = Nullable.Compare<DateTime>(x.DueDate, y.DueDate);
            if (dateCompare != 0) return dateCompare;

            int priorityCompare = y.PriorityScore.CompareTo(x.PriorityScore);
            if (priorityCompare != 0) return priorityCompare;

            return x.EstimatedDuration.CompareTo(y.EstimatedDuration);
        }
    }
}
