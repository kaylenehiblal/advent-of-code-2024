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

        // Regex to match mul(x,y), do(), and don't() instructions
        var regex = new Regex(@"mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\)");

        // A flag that determines whether multiplication is currently enabled
        bool enabled = true;

        // Variable to hold the final result
        int answer = 0;

        // Process each line in the input
        foreach (var line in lines)
        {
            // Find all valid instructions in the line
            foreach (Match match in regex.Matches(line))
            {
                // Convert the match to string
                var instruction = match.Value;

                // If enabled and we see a mul(x,y), process the multiplication
                if (enabled && instruction.StartsWith("mul"))
                {
                    // Extract the two numbers inside mul()
                    var numbers = instruction[4..^1].Split(',').Select(int.Parse).ToArray();

                    // Multiply and add the result to the total
                    answer += numbers[0] * numbers[1];
                }
                // If "do()" appears, enable processing
                else if (instruction.Equals("do()"))
                {
                    enabled = true;
                }
                // If "don't()" appears, disable processing
                else if (instruction.Equals("don't()"))
                {
                    enabled = false;
                }
            }
        }

        // Output the final calculated answer
        Console.WriteLine(answer);
    }
}
