using System;
using System.Collections.Generic;
using System.Linq;
using StudyPlanner.Models;

namespace StudyPlanner.DataStructures
{
    public class CourseGraph
    {
        private Dictionary<string, Course> nodes = new();
        private Dictionary<string, List<string>> edges = new();
        // maintain insertion order so ListAllCourses can return courses in the order they were added
        private List<string> insertionOrder = new();

        public void AddCourse(Course course)
        {
            if (!nodes.ContainsKey(course.Id!))
            {
                nodes[course.Id!] = course;
                edges[course.Id!] = new List<string>();
                insertionOrder.Add(course.Id!);
            }
        }

        public bool RemoveCourse(string courseId)
        {
            if (!nodes.ContainsKey(courseId)) return false;
            nodes.Remove(courseId);
            edges.Remove(courseId);
            insertionOrder.Remove(courseId);
            foreach (var e in edges.Values)
                e.Remove(courseId);
            return true;
        }

        // Removes the specified course and renumbers subsequent course IDs so there are no gaps.
        // Returns a mapping of oldId -> newId for all renamed courses (does not include the removed course).
        public System.Collections.Generic.Dictionary<string, string> RemoveCourseAndShift(string courseId)
        {
            var mapping = new System.Collections.Generic.Dictionary<string, string>();
            if (!nodes.ContainsKey(courseId)) return mapping;

            // snapshot the current insertion order
            var order = insertionOrder.ToList();
            var idx = order.IndexOf(courseId);
            if (idx < 0) return mapping;

            // identify courses to rename (those after the removed one)
            var toRename = new System.Collections.Generic.List<string>();
            for (int i = idx + 1; i < order.Count; i++)
            {
                toRename.Add(order[i]);
            }

            // remove the course
            nodes.Remove(courseId);
            edges.Remove(courseId);
            insertionOrder.RemoveAt(idx);
            foreach (var e in edges.Values)
                e.Remove(courseId);

            // perform renames in order
            foreach (var oldId in toRename)
            {
                // compute new ID only for IDs that look like prefix + number (e.g., C3)
                var prefix = new string(oldId.TakeWhile(c => !char.IsDigit(c)).ToArray());
                var numberPart = new string(oldId.SkipWhile(c => !char.IsDigit(c)).ToArray());
                if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(numberPart))
                {
                    // skip renaming non-matching IDs
                    continue;
                }

                if (!int.TryParse(numberPart, out var n)) continue;
                var newId = prefix + (n - 1).ToString();

                // rename node key
                var course = nodes[oldId];
                nodes.Remove(oldId);
                course.Id = newId;
                nodes[newId] = course;

                // rename edges key
                var edgeList = edges[oldId];
                edges.Remove(oldId);
                edges[newId] = edgeList;

                // update references in other edges
                foreach (var e in edges.Values)
                {
                    for (int j = 0; j < e.Count; j++)
                    {
                        if (e[j] == oldId) e[j] = newId;
                    }
                }

                // update insertionOrder list
                var pos = insertionOrder.IndexOf(oldId);
                if (pos >= 0)
                    insertionOrder[pos] = newId;

                mapping[oldId] = newId;
            }

            return mapping;
        }

        public bool AddPrerequisite(string courseId, string prereqId)
        {
            if (!nodes.ContainsKey(courseId) || !nodes.ContainsKey(prereqId)) return false;
            edges[courseId].Add(prereqId);
            if (HasCycle())
            {
                edges[courseId].Remove(prereqId);
                return false;
            }
            return true;
        }

        public bool RemovePrerequisite(string courseId, string prereqId)
        {
            return edges.ContainsKey(courseId) && edges[courseId].Remove(prereqId);
        }

        public bool ContainsCourse(string courseId) => nodes.ContainsKey(courseId);

        public List<Course> GetPrerequisites(string courseId)
        {
            if (!edges.ContainsKey(courseId)) return new List<Course>();
            return edges[courseId].Select(id => nodes[id]).ToList();
        }

        public bool HasCycle()
        {
            var visited = new HashSet<string>();
            var recStack = new HashSet<string>();

            bool DFS(string id)
            {
                if (!visited.Contains(id))
                {
                    visited.Add(id);
                    recStack.Add(id);
                    foreach (var neighbor in edges[id])
                    {
                        if (!visited.Contains(neighbor) && DFS(neighbor)) return true;
                        else if (recStack.Contains(neighbor)) return true;
                    }
                }
                recStack.Remove(id);
                return false;
            }

            foreach (var node in nodes.Keys)
            {
                if (DFS(node)) return true;
            }
            return false;
        }

        public List<Course> GetTopologicalOrder()
        {
            var visited = new HashSet<string>();
            var stack = new Stack<string>();

            void DFS(string id)
            {
                visited.Add(id);
                foreach (var neighbor in edges[id])
                    if (!visited.Contains(neighbor)) DFS(neighbor);
                stack.Push(id);
            }

            foreach (var id in nodes.Keys)
                if (!visited.Contains(id)) DFS(id);

            var list = new List<Course>();
            while (stack.Count > 0)
                list.Add(nodes[stack.Pop()]);
            return list;
        }

        public List<Course> ListAllCourses() => insertionOrder.Select(id => nodes[id]).ToList();
    }
}
