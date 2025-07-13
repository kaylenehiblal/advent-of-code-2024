using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read all lines from the input file
        string[] lines = File.ReadAllLines("input.txt");

        // Dictionary to store ordering rules:
        // key = a number, value = list of numbers that must come after the key
        var rules = new Dictionary<int, List<int>>();

        // Variable to store the final answer
        int answer = 0;

        // Process each line in the input
        foreach (var line in lines)
        {
            if (line.Contains('|'))
            {
                // If line is a rule, split and add to the dictionary
                AddRule(line.Split('|').Select(int.Parse).ToArray());
            }
            else if (!string.IsNullOrEmpty(line))
            {
                // If line is a number list, process and validate it
                CheckUpdate(line);
            }
        }

        // Output the final computed answer
        Console.WriteLine(answer);

        // ----------------------
        // FUNCTION DEFINITIONS
        // ----------------------

        // Adds a rule to the dictionary
        void AddRule(int[] numbers)
        {
            // Ensure the key exists in the dictionary
            if (!rules.ContainsKey(numbers[0]))
                rules[numbers[0]] = new List<int>();

            // Add the required-following number to the list
            rules[numbers[0]].Add(numbers[1]);
        }

        // Validates and processes a list of numbers
        void CheckUpdate(string numbers)
        {
            // Convert comma-separated string to a sorted list using custom comparator
            var list = numbers.Split(',').Select(int.Parse).ToList();
            list.Sort(CompareNumbers);

            // If the sorted list is NOT the same as the original,
            // it means the input list violated the intended order
            if (!numbers.Equals(string.Join(",", list)))
            {
                // Add the middle number of the corrected list to the answer
                answer += list[list.Count / 2];
            }
        }

        // Custom comparator based on rule order
        int CompareNumbers(int a, int b)
        {
            // If 'a' must come before 'b', return -1 to sort 'a' earlier
            if (rules.TryGetValue(a, out List<int>? value) && value.Contains(b))
                return -1;

            // Otherwise, no order enforcement — place 'b' before 'a'
            return 1;
        }
    }
}
