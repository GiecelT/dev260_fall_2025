using System;
using StudyPlanner.Services;
using StudyPlanner.Models;
using StudyPlanner.UI;

namespace StudyPlanner
{
    class Program
    {
        static void Main(string[] args)
        {
            PlannerService planner = new PlannerService();
            Menu menu = new Menu(planner);

            Console.WriteLine("=== Welcome to Study Planner ===\n");

            // Add sample course data
            planner.CreateCourse(new Course
            {
                Title = "Data Structures",
                Description = "Learn arrays, linked lists, trees, and graphs"
            });

            planner.CreateCourse(new Course
            {
                Title = "Web Development",
                Description = "HTML, CSS, JavaScript, and frameworks"
            });

            planner.CreateCourse(new Course
            {
                Title = "Database Design",
                Description = "SQL, schema design, and optimization"
            });

            planner.CreateCourse(new Course
            {
                Title = "Machine Learning",
                Description = "Neural networks, algorithms, and deep learning"
            });

            planner.CreateCourse(new Course
            {
                Title = "Algorithms",
                Description = "Sorting, searching, dynamic programming, and complexity analysis"
            });

            planner.CreateCourse(new Course
            {
                Title = "Software Engineering",
                Description = "Design patterns, testing, and project management"
            });

            Console.WriteLine("Sample courses loaded.\n");

            // Show main menu
            menu.ShowMainMenu();

            Console.WriteLine("Thank you for using Study Planner!");
        }
    }
}
