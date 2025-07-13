using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Load input grid from file
        var lines = File.ReadAllLines("input.txt");

        // Dictionary to track antenna positions by character
        var antennas = new Dictionary<char, List<(int row, int col)>>();

        // Set to hold unique antinode coordinates discovered on the grid
        var antinodes = new HashSet<(int row, int col)>();

        // Iterate through every cell in the grid
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                char currentChar = lines[y][x];

                // Skip empty spots represented by '.'
                if (currentChar == '.')
                    continue;

                // If this is the first occurrence of the antenna character, add it
                if (!antennas.TryGetValue(currentChar, out var positions))
                {
                    antennas[currentChar] = new List<(int, int)> { (y, x) };
                }
                else
                {
                    // For each existing antenna of the same type, calculate antinodes
                    foreach (var (antY, antX) in positions)
                    {
                        int deltaY = y - antY;
                        int deltaX = x - antX;

                        // Trace antinode positions forward from the new antenna
                        TraceAntinodePath(y, x, deltaY, deltaX);

                        // Trace antinode positions backward from the existing antenna
                        TraceAntinodePath(antY, antX, -deltaY, -deltaX);
                    }

                    // Add this antenna's position for future antinode calculations
                    positions.Add((y, x));
                }
            }
        }

        // Output the total count of unique antinodes found
        Console.WriteLine(antinodes.Count);

        // ----------------------
        // Helper function
        // ----------------------

        // Adds all positions along the vector (dY, dX) starting from (startY, startX)
        // Stops when the position goes out of grid bounds
        void TraceAntinodePath(int startY, int startX, int dY, int dX)
        {
            int currentY = startY;
            int currentX = startX;

            while (true)
            {
                // Stop if out of bounds
                if (currentY < 0 || currentY >= lines.Length || currentX < 0 || currentX >= lines[0].Length)
                    break;

                // Mark current position as an antinode
                antinodes.Add((currentY, currentX));

                // Move along the vector direction
                currentY += dY;
                currentX += dX;
            }
        }
    }
}
