using System;

namespace StudyPlanner.Utils
{
    public static class IdGenerator
    {
        // Keeps the old behavior for callers that expect a GUID string.
        public static string GenerateId() => Guid.NewGuid().ToString();

        // Thread-safe counters for sequential IDs per prefix.
        private static readonly object _lock = new object();
        private static readonly System.Collections.Generic.Dictionary<string, int> _counters = new System.Collections.Generic.Dictionary<string, int>();

        // Generates sequential IDs like "C1", "C2", ... for the provided prefix.
        public static string GenerateSequentialId(string prefix)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));

            lock (_lock)
            {
                if (!_counters.TryGetValue(prefix, out var current))
                {
                    current = 0;
                }
                current++;
                _counters[prefix] = current;
                return string.Concat(prefix, current);
            }
        }

        // Convenience method for course IDs: "C1", "C2", ...
        public static string GenerateCourseId() => GenerateSequentialId("C");

        // Convenience method for task IDs: "T1", "T2", ...
        public static string GenerateTaskId() => GenerateSequentialId("T");

        // Export current counters (safe copy) so they can be persisted.
        public static System.Collections.Generic.Dictionary<string, int> GetCounters()
        {
            lock (_lock)
            {
                return new System.Collections.Generic.Dictionary<string, int>(_counters);
            }
        }

        // Load counters from persisted state. Overwrites in-memory counters.
        public static void LoadCounters(System.Collections.Generic.Dictionary<string, int>? counters)
        {
            if (counters == null) return;
            lock (_lock)
            {
                _counters.Clear();
                foreach (var kv in counters)
                    _counters[kv.Key] = kv.Value;
            }
        }
    }
}
