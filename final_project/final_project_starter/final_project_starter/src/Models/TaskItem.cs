using System;

namespace StudyPlanner.Models
{
    public class TaskItem
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? CourseId { get; set; }
        public DateTime? DueDate { get; set; }
        public int PriorityScore { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public bool Completed { get; set; }
        public bool InProgress { get; set; }
    }
}