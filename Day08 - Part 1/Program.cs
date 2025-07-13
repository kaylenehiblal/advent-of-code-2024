using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines from file
        var lines = File.ReadAllLines("input.txt");

        // Dictionary mapping each antenna character to a list of its coordinates (row, column)
        var antennaPositions = new Dictionary<char, List<(int row, int col)>>();

        // Set to store unique antinode coordinates detected on the grid
        var antinodeCoords = new HashSet<(int row, int col)>();

        // Iterate over every cell in the grid
        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[0].Length; col++)
            {
                char currentChar = lines[row][col];

                // Skip empty cells represented by '.'
                if (currentChar == '.')
                    continue;

                // If this is the first time we see this antenna character, create a new list
                if (!antennaPositions.TryGetValue(currentChar, out var positions))
                {
                    antennaPositions[currentChar] = new List<(int, int)> { (row, col) };
                }
                else
                {
                    // For each existing antenna with the same character,
                    // calculate symmetric points and mark them as antinodes
                    foreach (var (antRow, antCol) in positions)
                    {
                        int deltaY = row - antRow;
                        int deltaX = col - antCol;

                        // Place antinodes symmetrically relative to both antenna positions
                        MarkAntinode(row + deltaY, col + deltaX);
                        MarkAntinode(antRow - deltaY, antCol - deltaX);
                    }

                    // Add the current antenna position to the list
                    positions.Add((row, col));
                }
            }
        }

        // Output total count of unique antinodes found
        Console.WriteLine(antinodeCoords.Count);

        // --------------------------
        // Helper method definitions
        // --------------------------

        // Adds a coordinate to the antinode set if it's within bounds
        void MarkAntinode(int y, int x)
        {
            if (y >= 0 && y < lines.Length && x >= 0 && x < lines[0].Length)
                antinodeCoords.Add((y, x));
        }
    }
}
