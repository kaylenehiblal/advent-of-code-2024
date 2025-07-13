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

        // List to store all rule pairs (each as an int array of size 2)
        var rules = new List<int[]>();

        // Variable to store the final answer
        int answer = 0;

        // Process each line in the input
        foreach (var line in lines)
        {
            // If the line contains a rule (represented by two numbers separated by '|')
            if (line.Contains('|'))
            {
                // Split the line into the two rule elements and parse them into integers
                rules.Add(line.Split('|').Select(int.Parse).ToArray());
            }
            // Otherwise, if the line contains numbers (and isn't blank), process it
            else if (!string.IsNullOrEmpty(line))
            {
                // Split the line by commas and parse it into a list of integers
                var numbers = line.Split(',').Select(int.Parse).ToList();

                // Check whether the list satisfies all rule conditions
                CheckUpdate(numbers);
            }
        }

        // Output the final computed answer
        Console.WriteLine(answer);

        // Local function to validate the list against rules and update the answer
        void CheckUpdate(List<int> numbers)
        {
            // Loop through each rule
            foreach (var rule in rules)
            {
                int left = rule[0];
                int right = rule[1];

                // If both numbers are present in the list
                if (numbers.Contains(left) && numbers.Contains(right))
                {
                    // If the left value appears *after* the right, it's invalid — skip it
                    if (numbers.IndexOf(left) > numbers.IndexOf(right))
                        return;
                }
            }

            // If the list passed all rule checks, add the middle number to the answer
            answer += numbers[numbers.Count / 2];
        }
    }
}
