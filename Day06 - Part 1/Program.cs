using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all lines from the input file
        string[] lines = File.ReadAllLines("input.txt");

        // Initialize a 2D boolean map to track visited locations
        // Dimensions are based on input lines and length of each line
        var map = new bool[lines.Length, lines[0].Length];

        // Starting location coordinates (row, column)
        (int row, int col) location = (0, 0);

        // Direction index: 0 = up, 1 = right, 2 = down, 3 = left
        var direction = 0;

        // Movement deltas corresponding to directions (up, right, down, left)
        // Each inner array is (deltaRow, deltaCol)
        var deltaMap = new int[4, 2]
        {
            {-1, 0},  // Up
            {0, 1},   // Right
            {1, 0},   // Down
            {0, -1}   // Left
        };

        // Variable to count unique visited positions
        var answer = 0;

        // Find the initial location of the '^' character in the input grid
        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y].Contains('^'))
            {
                location = (y, lines[y].IndexOf('^'));
                break;  // Exit once found
            }
        }

        // Traverse until movement stops or we go out of bounds
        while (true)
        {
            // If this position has not been visited yet, increment answer
            if (!map[location.row, location.col])
                answer++;

            // Mark the current position as visited
            map[location.row, location.col] = true;

            // Calculate next position based on current direction
            var nextRow = location.row + deltaMap[direction % 4, 0];
            var nextCol = location.col + deltaMap[direction % 4, 1];

            // Check if next position is outside the grid boundaries
            if (nextRow < 0 || nextRow >= lines.Length || nextCol < 0 || nextCol >= lines[0].Length)
                break;  // Stop traversal if out of bounds

            // If the next position is a wall (#), turn clockwise (change direction)
            if (lines[nextRow][nextCol] == '#')
            {
                direction++;
            }
            else
            {
                // Otherwise, move to the next position
                location.row = nextRow;
                location.col = nextCol;
            }
        }

        // Output the total count of unique visited positions
        Console.WriteLine(answer);
    }
}
