using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read all input lines from the file
        var lines = File.ReadAllLines("input.txt");

        long answer = 0;

        // Process input in blocks of 4 lines as per data structure
        for (int i = 0; i < lines.Length; i += 4)
        {
            // Parse line i: coefficients A (skip first 12 chars, remove " Y+", split by comma)
            var A = lines[i][12..]
                .Replace(" Y+", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            // Parse line i+1: coefficients B similarly
            var B = lines[i + 1][12..]
                .Replace(" Y+", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            // Parse line i+2: prize coordinates (skip first 9 chars, remove " Y=", split by comma)
            var prize = lines[i + 2][9..]
                .Replace(" Y=", string.Empty)
                .Split(',')
                .Select(double.Parse)
                .ToArray();

            // Calculate 'a' and 'b' using linear algebra formulas
            double a = (prize[1] - (B[1] * prize[0] / B[0])) / (A[1] - (B[1] * A[0] / B[0]));
            double b = (prize[0] - (a * A[0])) / B[0];

            // Check if 'a' and 'b' are positive integers (within rounding tolerance)
            if (a > 0 && b > 0 &&
                Math.Round(a, 2) == Math.Round(a, 0) &&
                Math.Round(b, 2) == Math.Round(b, 0))
            {
                // Add weighted sum to answer (3 * a + b)
                answer += (long)Math.Round(a, 0) * 3 + (long)Math.Round(b, 0);
            }
        }

        // Output the final result
        Console.WriteLine(answer);
    }
}
