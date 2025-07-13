using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified file into the grid
        string[] l = File.ReadAllLines("input.txt");

        // Variable to store the final answer (valid diagonal A-patterns)
        int answer = 0;

        // Iterate through the grid, skipping the edges to avoid out-of-bounds access
        for (int y = 1; y < l.Length - 1; y++)
        {
            for (int x = 1; x < l[0].Length - 1; x++)
            {
                // Check if the current character is 'A'
                if (l[y][x] == 'A')
                {
                    // Extract the characters from the four diagonals around 'A'
                    char nw = l[y - 1][x - 1]; // Northwest
                    char se = l[y + 1][x + 1]; // Southeast
                    char sw = l[y + 1][x - 1]; // Southwest
                    char ne = l[y - 1][x + 1]; // Northeast

                    // Check that NW+SE and SW+NE both form "SM" or "MS"
                    bool validNW_SE = $"{nw}{se}" == "SM" || $"{nw}{se}" == "MS";
                    bool validSW_NE = $"{sw}{ne}" == "SM" || $"{sw}{ne}" == "MS";

                    // If both diagonal checks are valid, count this 'A' as part of a pattern
                    if (validNW_SE && validSW_NE)
                        answer++;
                }
            }
        }

        // Output the final count of valid 'A' diagonal patterns
        Console.WriteLine(answer);
    }
}
