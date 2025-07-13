using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified file path
        string[] lines = File.ReadAllLines("input.txt");

        // Variable to store the count of valid reports
        int answer = 0;

        // Process each line (report) in the input
        foreach (var line in lines)
        {
            // Split the line into integers representing the report data
            var report = line.Split(' ').Select(int.Parse).ToArray();

            // If the report passes the safety check, increment the valid count
            if (IsSafe(report))
                answer++;
        }

        // Output the total number of valid reports
        Console.WriteLine(answer);
    }

    // Determines whether a report is valid based on monotonicity and delta constraints
    static bool IsSafe(int[] report)
    {
        // Direction is 0 initially; will be set to positive or negative once determined
        int direction = 0;

        // Iterate through the report values pairwise
        for (int i = 1; i < report.Length; i++)
        {
            // Calculate the difference between consecutive values
            int delta = report[i] - report[i - 1];

            // Report is invalid if no movement, or jump is larger than 3 units
            if (delta == 0 || delta > 3 || delta < -3)
                return false;

            // Set the direction on first change (positive or negative)
            if (direction == 0)
                direction = delta;
            else
            {
                // If the trend reverses (increasing to decreasing or vice versa), it's invalid
                if ((direction > 0 && delta < 0) || (direction < 0 && delta > 0))
                    return false;
            }
        }

        // Passed all checks: report is valid
        return true;
    }
}
