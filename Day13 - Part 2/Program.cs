using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read all lines from input file
        var lines = File.ReadAllLines("input.txt");

        long answer = 0;

        // Process every 4 lines as a group of inputs
        for (int i = 0; i < lines.Length; i += 4)
        {
            // Parse coefficients A and B from the formatted lines
            var A = lines[i][12..]
                .Replace(" Y+", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            var B = lines[i + 1][12..]
                .Replace(" Y+", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            // Parse prize coordinates and add a large offset to avoid negative or precision issues
            var prize = lines[i + 2][9..]
                .Replace(" Y=", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            // Add a large constant offset to each coordinate (adjust as per problem domain)
            for (int j = 0; j < prize.Length; j++)
            {
                prize[j] += 1_000_000_000_0000; // 10 trillion offset
            }

            // Solve for 'a' and 'b' using linear algebra formulas based on inputs
            double a = (prize[1] - (B[1] * prize[0] / B[0])) / (A[1] - (B[1] * A[0] / B[0]));
            double b = (prize[0] - (a * A[0])) / B[0];

            // Validate that 'a' and 'b' are positive integers (within 2 decimal places rounding)
            if (a > 0 && b > 0 &&
                Math.Round(a, 2) == Math.Round(a, 0) &&
                Math.Round(b, 2) == Math.Round(b, 0))
            {
                // Add weighted sum to the answer
                answer += (long)Math.Round(a, 0) * 3 + (long)Math.Round(b, 0);
            }
        }

        // Print the final calculated answer
        Console.WriteLine(answer);
    }
}
