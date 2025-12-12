using System;

namespace StudyPlanner.UI
{
    public static class InputValidator
    {
        public static int GetInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value)) return value;
                Console.WriteLine("Invalid integer. Try again.");
            }
        }

        public static string GetString(string prompt)
        {
            string? input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) return input;
                Console.WriteLine("Input cannot be empty.");
            }
        }

        public static DateTime? GetDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
                if (DateTime.TryParse(input, out var dt)) return dt;
                Console.WriteLine("Invalid date. Use yyyy-MM-dd or leave empty.");
            }
        }
    }
}