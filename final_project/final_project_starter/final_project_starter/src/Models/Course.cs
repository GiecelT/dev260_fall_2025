namespace StudyPlanner.Models
{
    public class Course
    {
        public string CourseId { get; set; } = "";
        public string? Description { get; set; } = "";
        public string? Id { get; internal set; }
        public string? Title { get; internal set; }
    }
}