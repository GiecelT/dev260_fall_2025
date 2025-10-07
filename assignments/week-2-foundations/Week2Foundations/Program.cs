// DEV 260
// Author: Giecel Tumbaga
// Data Structures & Big-O

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataStructureExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Array Example ===");
            ArrayExample();
            Console.ReadKey();

            Console.WriteLine("\n=== List<T> Example ===");
            ListExample();
            Console.ReadKey();

            Console.WriteLine("\n=== Stack<T> Example ===");
            StackExample();
            Console.ReadKey();

            Console.WriteLine("\n=== Queue<T> Example ===");
            QueueExample();
            Console.ReadKey();

            Console.WriteLine("\n=== Dictionary<TKey, TValue> Example ===");
            DictionaryExample();
            Console.ReadKey();

            Console.WriteLine("\n=== HashSet<T> Example ===");
            HashSetExample();
            Console.ReadKey();
        }

        // === Array Example + Mini Benchmark ===
        static void ArrayExample()
        {
            int[] numbers = new int[10];
            numbers[0] = 5;
            numbers[1] = 10;
            numbers[2] = 15;

            Console.WriteLine($"Value at index 2: {numbers[2]}");

            int searchValue = 10;
            bool found = false;
            foreach (int n in numbers)
            {
                if (n == searchValue)
                {
                    found = true;
                    break;
                }
            }
            Console.WriteLine($"Search for {searchValue}: {(found ? "Found" : "Not found")}");

            // Mini benchmark: membership test (linear search)
            RunMembershipBenchmark("Array", N => BuildArray(N), (arr, val) => Array.Exists(arr, x => x == val));
        }

        // === List Example + Mini Benchmark ===
        static void ListExample()
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            list.Add(6);
            list.Insert(2, 99);
            list.Remove(99);
            Console.WriteLine($"Final Count: {list.Count}");

            RunMembershipBenchmark("List<int>", N => BuildList(N), (lst, val) => lst.Contains(val));
        }

        // === Stack Example + Mini Benchmark ===
        static void StackExample()
        {
            Stack<string> pages = new Stack<string>();
            pages.Push("home.com");
            pages.Push("about.com");
            pages.Push("contact.com");

            Console.WriteLine($"Current page: {pages.Peek()}");

            Console.WriteLine("Back navigation order:");
            while (pages.Count > 0)
            {
                Console.WriteLine(pages.Pop());
            }

            // Benchmark push/pop
            RunStackBenchmark();
        }

        // === Queue Example + Mini Benchmark ===
        static void QueueExample()
        {
            Queue<string> printJobs = new Queue<string>();
            printJobs.Enqueue("Doc1");
            printJobs.Enqueue("Doc2");
            printJobs.Enqueue("Doc3");

            Console.WriteLine($"Next job: {printJobs.Peek()}");

            Console.WriteLine("Processing order:");
            while (printJobs.Count > 0)
            {
                Console.WriteLine(printJobs.Dequeue());
            }

            // Benchmark enqueue/dequeue
            RunQueueBenchmark();
        }

        // === Dictionary Example + Mini Benchmark ===
        static void DictionaryExample()
        {
            Dictionary<string, int> inventory = new Dictionary<string, int>
            {
                { "A123", 10 },
                { "B456", 5 },
                { "C789", 20 }
            };

            inventory.Add("D000", 15);

            if (inventory.TryGetValue("B456", out int qty))
            {
                Console.WriteLine($"B456 quantity: {qty}");
            }

            inventory.Remove("C789");

            Console.WriteLine($"Final inventory count: {inventory.Count}");

            RunMembershipBenchmark("Dictionary<int,bool>", N => BuildDictionary(N), (dict, val) => dict.ContainsKey(val));
        }

        // === HashSet Example + Mini Benchmark ===
        static void HashSetExample()
        {
            HashSet<int> set = new HashSet<int> { 1, 2, 3 };
            set.Add(4);
            bool exists = set.Contains(2);
            Console.WriteLine($"Set contains 2: {exists}");
            set.Remove(3);
            Console.WriteLine($"Final Count: {set.Count}");

            RunMembershipBenchmark("HashSet<int>", N => BuildHashSet(N), (set, val) => set.Contains(val));
        }

        // === Helper Builders ===
        static int[] BuildArray(int N)
        {
            int[] arr = new int[N];
            for (int i = 0; i < N; i++) arr[i] = i;
            return arr;
        }

        static List<int> BuildList(int N)
        {
            List<int> list = new List<int>(N);
            for (int i = 0; i < N; i++) list.Add(i);
            return list;
        }

        static Dictionary<int, bool> BuildDictionary(int N)
        {
            var dict = new Dictionary<int, bool>(N);
            for (int i = 0; i < N; i++) dict[i] = true;
            return dict;
        }

        static HashSet<int> BuildHashSet(int N)
        {
            var set = new HashSet<int>();
            for (int i = 0; i < N; i++) set.Add(i);
            return set;
        }

        // === Benchmark Methods ===
        static void RunMembershipBenchmark<T>(string label, Func<int, T> buildFunc, Func<T, int, bool> containsFunc)
        {
            int[] Ns = { 1_000, 10_000, 100_000 };
            foreach (int N in Ns)
            {
                var structure = buildFunc(N);
                int targetFound = N - 1;
                int targetMissing = -1;

                double timeFound = MeasureBestOfThree(() => containsFunc(structure, targetFound));
                double timeMissing = MeasureBestOfThree(() => containsFunc(structure, targetMissing));

                Console.WriteLine($"\n{label} N={N}");
                Console.WriteLine($"{label}.Contains({targetFound}): {timeFound:F3} ms");
                Console.WriteLine($"{label}.Contains({targetMissing}): {timeMissing:F3} ms");
            }
            Console.WriteLine("(Best of 3 runs per check)\n");
        }

        static void RunStackBenchmark()
        {
            int N = 100_000;
            Stack<int> stack = new Stack<int>();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < N; i++) stack.Push(i);
            sw.Stop();
            Console.WriteLine($"\nPushed {N} items: {sw.Elapsed.TotalMilliseconds:F3} ms");

            sw.Restart();
            while (stack.Count > 0) stack.Pop();
            sw.Stop();
            Console.WriteLine($"Popped {N} items: {sw.Elapsed.TotalMilliseconds:F3} ms\n");
        }

        static void RunQueueBenchmark()
        {
            int N = 100_000;
            Queue<int> queue = new Queue<int>();

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < N; i++) queue.Enqueue(i);
            sw.Stop();
            Console.WriteLine($"\nEnqueued {N} items: {sw.Elapsed.TotalMilliseconds:F3} ms");

            sw.Restart();
            while (queue.Count > 0) queue.Dequeue();
            sw.Stop();
            Console.WriteLine($"Dequeued {N} items: {sw.Elapsed.TotalMilliseconds:F3} ms\n");
        }

        static double MeasureBestOfThree(Action action)
        {
            double best = double.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                var sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                best = Math.Min(best, sw.Elapsed.TotalMilliseconds);
            }
            return best;
        }
    }
}
