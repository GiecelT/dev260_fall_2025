// Course: DEV 260
// Author: Giecel Tumbaga
// Date: 9/5/2023
// Lab 2: Comparing Loops & Conditionals

// Task 1: Sum of Even Numbers from 1 to 100
// 1-1: Using a for loop



namespace LoopsAndConditionalsLab
{
    class Program
    {
        // Task 1: Sum of Even Numbers from 1 to 100
        static void Main(string[] args)
        {
            // 1-1: Using a for loop
            int sumForLoop = 0;
            for (int i = 1; i <= 100; i++)
            {
                if (i % 2 == 0)
                {
                    sumForLoop += i;
                }
            }
            Console.WriteLine($"Sum of even numbers from 1 to 100 using for loop: {sumForLoop}");
            Console.WriteLine("Press any key to continue to the next part...");
            Console.ReadKey();


            // 1-2: Using a while loop
            int sumWhileLoop = 0;
            int j = 1;
            while (j <= 100)
            {
                if (j % 2 == 0)
                {
                    sumWhileLoop += j;
                }
                j++;
            }
            Console.WriteLine($"\nSum of even numbers from 1 to 100 using while loop: {sumWhileLoop}");
            Console.WriteLine("Press any key to continue to the next part...");
            Console.ReadKey();


            // 1-3: Using a Foreach loop with a list of numbers from 1-100
            List<int> numbers = new List<int>();
            for (int k = 1; k <= 100; k++)
            {
                numbers.Add(k);
            }
            int sumForeachLoop = 0;
            foreach (int number in numbers)
            {
                if (number % 2 == 0)
                {
                    sumForeachLoop += number;
                }
            }
            Console.WriteLine($"\nSum of even numbers from 1 to 100 using foreach loop: {sumForeachLoop}");
            Console.WriteLine($"List of even numbers from 1 to 100: {string.Join(", ", numbers.Where(n => n % 2 == 0))}");
            Console.WriteLine("Press any key to continue to Task 2...");
            Console.ReadKey();
        }
        }

        // Task 1 Question: Which loop felt most natural for this task and why?
        // Answer: The loop that felt most natural for this task was the "forloop".
        // This is because we know the exact number of iterations (from 1 to 100), 
        // making it straightforward to set up the loop with a clear starting point, ending point, and increment.


        static void Main(string[] args)
        {
            // Task 2: Grading with Conditionals
            // 2-1: Using if-else statements 
            // Prompting the user for a score:

            // Ask the user to input a score between 0 and 100
            Console.Write("Enter a score between 0 and 100: ");
            string? input = Console.ReadLine(); // Read user input as text

            // Convert the user input to an integer
            int score = Convert.ToInt32(input);

            // Call the GetLetterGrade method and store the result
            string grade = GetLetterGrade(score);

            // Display the letter grade to the user
            Console.WriteLine($"\nThe letter grade is: {grade}");
            Console.WriteLine("Press any key to continue to the next part...");
            Console.ReadKey();
        }

        static string GetLetterGrade(int score)
        {
            // Task 2-1: Using if-else statements to determine the letter grade
            if (score >= 90)
            {
                return "A";
            }
            else if (score >= 80)
            {
                return "B";
            }
            else if (score >= 70)
            {
                return "C";
            }
            else if (score >= 60)
            {
                return "D";
            }
            else
            {
                return "F";
            }
        }


        static void Main(string[] args)
        {
            // Task 2: Grading with Conditionals
            // 2-2: Using switch-case statements 
            // Prompting the user for a score:

            // Ask the user to input a score between 0 and 100
            Console.Write("Enter a score between 0 and 100: ");
            string? input = Console.ReadLine(); // Read user input as text

            // Convert the user input to an integer
            int score = Convert.ToInt32(input);

            // Call the GetLetterGradeSwitch method and store the result
            string grade = GetLetterGradeSwitch(score);

            // Display the letter grade to the user
            Console.WriteLine($"The letter grade is: {grade}");
            Console.WriteLine("Press any key to continue to the Mini Challenge...");
            Console.ReadKey();
        }

        static string GetLetterGradeSwitch(int score)
        {
            // Task 2-2: Using switch-case statements to determine the letter grade
            switch (score / 10)
            {
                case 10:
                case 9:
                    return "A";
                case 8:
                    return "B";
                case 7:
                    return "C";
                case 6:
                    return "D";
                default:
                    return "F";
            }
        }

        // Which apporach is easier to read and maintain?
        // Answer: The switch-case approach is more concise which made it easier to read and maintain the code.
        // The if-else statements can become lengthy and a bit confusing to follow, especially with multiple conditions.
        // The switch-case structure provides a clearer overview of the grading criteria at a glance.


        // Task 3: Mini Challenge 
        // 3-1: Modify your sum of even numbers method so that if the sum is greater than 2000, 
        // it prints "That's a big number!" to the console.
        // Use at least two different conditional structures to implement this check.
        static void Main(string[] args)
        {
            // Task 3: Mini Challenge 
            // 3-1: Sum of Even Numbers from 1 to 100 with additional check
            int sumForLoop = 0;
            for (int i = 1; i <= 100; i++)
            {
                if (i % 2 == 0)
                {
                    sumForLoop += i;
                }
            }
            Console.WriteLine($"Sum of even numbers from 1 to 100 using for loop: {sumForLoop}");

            // Check using if statement
            if (sumForLoop > 2000)
            {
                Console.WriteLine("That's a big number!");
            }

            // Check using ternary operator
            string message = sumForLoop > 2000 ? "That's a big number!" : "";
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}