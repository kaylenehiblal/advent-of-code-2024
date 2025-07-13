using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified file
        string[] lines = File.ReadAllLines("input.txt");

        // Regular expression to match patterns like "mul(3,4)" with 1–3 digit numbers
        var regex = new Regex(@"mul\(\d{1,3},\d{1,3}\)");

        // Variable to store the final answer
        int answer = 0;

        // Iterate through each line in the input
        foreach (var line in lines)
        {
            // For each regex match of "mul(x,y)" in the line
            foreach (Match match in regex.Matches(line))
            {
                // Extract the content inside parentheses: e.g. "3,4"
                var numbers = match.Value[4..^1].Split(',').Select(int.Parse).ToArray();

                // Multiply the two numbers and add the result to the total
                answer += numbers[0] * numbers[1];
            }
        }

        // Output the final answer
        Console.WriteLine(answer);
    }
}
