using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all lines from the input file
        var lines = File.ReadAllLines("input.txt");

        // Starting location coordinates (row, column)
        (int row, int col) location = (0, 0);

        // 2D arrays to track walls and visited locations during traversal
        var walls = new bool[lines.Length, lines[0].Length];
        var referenceMap = new bool[lines.Length, lines[0].Length];

        // Movement directions: up, right, down, left
        var deltaMap = new int[4, 2]
        {
            { -1, 0 },  // Up
            { 0, 1 },   // Right
            { 1, 0 },   // Down
            { 0, -1 }   // Left
        };

        // Find initial location of '^' and mark walls on the grid
        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y].Contains('^'))
                location = (y, lines[y].IndexOf('^'));

            for (int x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == '#')
                    walls[y, x] = true;
            }
        }

        // Mark all reachable locations from the starting point
        Walk(ref referenceMap, location);

        int answer = 0;

        // Check each reachable location except starting point for loop detection
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                if (referenceMap[y, x]) // If reachable
                {
                    // Skip starting location and test for loops with this obstacle
                    if (!(y == location.row && x == location.col) &&
                        DetectLoop(location, y, x))
                    {
                        answer++;
                    }
                }
            }
        }

        // Output the count of locations that cause loops
        Console.WriteLine(answer);

        // ----------------------
        // FUNCTION DEFINITIONS
        // ----------------------

        // Walks the grid from a starting location, marking reachable cells
        void Walk(ref bool[,] map, (int row, int col) loc)
        {
            int direction = 0;

            while (true)
            {
                // Mark current position as visited
                map[loc.row, loc.col] = true;

                // Calculate next position based on current direction
                int nextRow = loc.row + deltaMap[direction % 4, 0];
                int nextCol = loc.col + deltaMap[direction % 4, 1];

                // Stop if next position is outside grid boundaries
                if (nextRow < 0 || nextRow >= lines.Length || nextCol < 0 || nextCol >= lines[0].Length)
                    return;

                // If next position is a wall, turn clockwise (change direction)
                if (walls[nextRow, nextCol])
                {
                    direction++;
                }
                else
                {
                    // Otherwise, move forward to the next position
                    loc.row = nextRow;
                    loc.col = nextCol;
                }
            }
        }

        // Detects if walking the grid with a specific obstacle location causes a loop
        bool DetectLoop((int row, int col) startLocation, int obstacleY, int obstacleX)
        {
            // 3D array to track visited positions and directions:
            // visitedDirections[row, col, direction]
            var visitedDirections = new bool[lines.Length, lines[0].Length, 4];

            int direction = 0;
            var loc = startLocation;

            while (true)
            {
                // Mark current position and direction as visited
                visitedDirections[loc.row, loc.col, direction % 4] = true;

                // Calculate next position based on current direction
                int nextRow = loc.row + deltaMap[direction % 4, 0];
                int nextCol = loc.col + deltaMap[direction % 4, 1];

                // If next position is out of bounds, no loop possible
                if (nextRow < 0 || nextRow >= lines.Length || nextCol < 0 || nextCol >= lines[0].Length)
                    return false;

                // If we have visited this position and direction before, a loop is detected
                if (visitedDirections[nextRow, nextCol, direction % 4])
                    return true;

                // If next position is a wall or the obstacle location, turn clockwise
                if (walls[nextRow, nextCol] || (nextRow == obstacleY && nextCol == obstacleX))
                {
                    direction++;
                }
                else
                {
                    // Otherwise, move forward
                    loc.row = nextRow;
                    loc.col = nextCol;
                }
            }
        }
    }
}
