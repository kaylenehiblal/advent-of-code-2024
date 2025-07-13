using System;
using System.IO;

class Program
{
    // Racetrack grid representing the map:
    // -1 means wall, other numbers represent distance steps.
    static int[,] racetrack;

    // The final answer to be computed
    static int answer = 0;

    static void Main()
    {
        // Read all input lines from file
        var lines = File.ReadAllLines("input.txt");

        // Directions: Up, Right, Down, Left
        var deltaMap = new int[4, 2]
        {
            { -1, 0 }, // Up
            { 0, 1 },  // Right
            { 1, 0 },  // Down
            { 0, -1 }  // Left
        };

        // Initialize racetrack grid size based on input
        racetrack = new int[lines.Length, lines[0].Length];

        // Start and end points on the racetrack
        var start = new Point(0, 0);
        var end = new Point(0, 0);

        // Parse input lines to fill racetrack
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                char c = lines[y][x];
                if (c == '#')
                {
                    // Mark walls with -1
                    racetrack[y, x] = -1;
                }
                else if (c == 'S')
                {
                    // Record start position
                    start = new Point(x, y);
                }
                else if (c == 'E')
                {
                    // Record end position
                    end = new Point(x, y);
                }
            }
        }

        // Current position starts at start point
        var pos = new Point(start.x, start.y);

        // Variables to track previous position (to avoid going backwards)
        int previousX = -1, previousY = -1;

        // Traverse the racetrack until reaching the end
        while (!(pos.x == end.x && pos.y == end.y))
        {
            int dY, dX;

            // Check all four directions around current position
            for (var i = 0; i < 4; i++)
            {
                dY = pos.y + deltaMap[i, 0];
                dX = pos.x + deltaMap[i, 1];

                // If next position is not a wall and is not the previous position
                if (racetrack[dY, dX] != -1 && !(dX == previousX && dY == previousY))
                {
                    // Update the distance at new position
                    racetrack[dY, dX] = racetrack[pos.y, pos.x] + 1;

                    // Update previous position to current
                    previousX = pos.x;
                    previousY = pos.y;

                    // Move current position to the new one
                    pos.x = dX;
                    pos.y = dY;

                    break; // Exit for-loop once a move is made
                }
            }
        }

        // After reaching end, check all racetrack cells for possible pairs
        for (var y = 1; y < lines.Length - 1; y++)
        {
            for (var x = 1; x < lines[0].Length - 1; x++)
            {
                // Skip walls
                if (racetrack[y, x] != -1)
                {
                    // Look around the current position within 20 cells in each direction
                    for (var dY = y > 20 ? y - 21 : 1; dY < y + 21 && dY < lines.Length - 1; dY++)
                    {
                        for (var dX = x > 20 ? x - 21 : 1; dX < x + 21 && dX < lines[0].Length - 1; dX++)
                        {
                            // Consider only different positions and those with greater step count
                            if (racetrack[dY, dX] != -1 && !(y == dY && x == dX) && racetrack[dY, dX] > racetrack[y, x])
                            {
                                // Check if this pair qualifies under the problem's conditions
                                Cheat(x, y, dX, dY);
                            }
                        }
                    }
                }
            }
        }

        // Output the computed answer
        Console.WriteLine(answer);
    }

    /// <summary>
    /// Checks if two points (aX, aY) and (bX, bY) qualify under distance conditions,
    /// and increments answer if conditions met.
    /// </summary>
    static void Cheat(int aX, int aY, int bX, int bY)
    {
        // Calculate Manhattan distance in terms of x and y separately
        var picoseconds = Distance(aX, bX) + Distance(aY, bY);

        // Check if total distance is within 20 and saved time is at least 100
        if (picoseconds <= 20 && Saved(racetrack[aY, aX], racetrack[bY, bX], picoseconds) >= 100)
            answer++;
    }

    /// <summary>
    /// Calculates how much time is saved by going between two points with given steps.
    /// </summary>
    static int Saved(int a, int b, int steps) => a > b ? a - b - steps : b - a - steps;

    /// <summary>
    /// Calculates absolute distance between two points.
    /// </summary>
    static int Distance(int a, int b) => a > b ? a - b : b - a;

    /// <summary>
    /// Helper class to represent a point (x, y).
    /// </summary>
    class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
