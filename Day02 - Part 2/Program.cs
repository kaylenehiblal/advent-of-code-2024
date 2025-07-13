using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified file path
        string[] lines = File.ReadAllLines("input.txt");

        // Variable to store the number of valid reports (including with one removal)
        int answer = 0;

        // Process each line of the input
        foreach (var line in lines)
        {
            // Convert the space-separated numbers in the line into an integer array
            var report = line.Split(' ').Select(int.Parse).ToArray();

            // Check if the report is valid as-is, or can be made valid by removing one value
            if (IsSafe(report, true))
                answer++;
        }

        // Output the total number of valid reports
        Console.WriteLine(answer);
    }

    // Determines if a report is valid either directly or by removing one value
    static bool IsSafe(int[] report, bool allowRemoval)
    {
        int ups = 0;              // Tracks how many increasing steps
        int downs = 0;            // Tracks how many decreasing steps
        int badSteps = 0;         // Tracks how many invalid differences

        // Iterate through the report and compare adjacent numbers
        for (int i = 1; i < report.Length; i++)
        {
            // Count increases and decreases
            if (report[i] > report[i - 1])
                ups++;
            else if (report[i] < report[i - 1])
                downs++;

            // Calculate the absolute difference
            int diff = Math.Abs(report[i] - report[i - 1]);

            // A step is bad if it's zero or the change is more than 3
            if (diff == 0 || diff > 3)
                badSteps++;
        }

        // Valid if all steps are in one direction and no invalid jumps occurred
        if ((ups == 0 || downs == 0) && badSteps == 0)
            return true;

        // If we're allowed to try removing one number to fix the report
        for (int indexToRemove = 0; allowRemoval && indexToRemove < report.Length; indexToRemove++)
        {
            // Create a new report with one number removed
            var newReport = new List<int>();
            for (int i = 0; i < report.Length; i++)
                if (i != indexToRemove)
                    newReport.Add(report[i]);

            // Recursively check if the new report is safe without allowing another removal
            if (IsSafe([.. newReport], false))
                return true;
        }

        // If no valid version found, return false
        return false;
    }
}
