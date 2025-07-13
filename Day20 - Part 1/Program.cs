using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all lines from the input file
        var lines = File.ReadAllLines("input.txt");

        // Direction vectors for up, right, down, left movements
        var deltaMap = new int[4, 2]
        {
            { -1, 0 }, // Up
            { 0, 1 },  // Right
            { 1, 0 },  // Down
            { 0, -1 }  // Left
        };

        // Initialize racetrack grid, -1 means obstacle (#), other values will hold distances
        var racetrack = new int[lines.Length, lines[0].Length];

        // Starting and ending points on the track
        var start = new Point(0, 0);
        var end = new Point(0, 0);

        // Parse the input grid to mark obstacles and find start/end points
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                if (lines[y][x] == '#')
                    racetrack[y, x] = -1; // Obstacle
                else if (lines[y][x] == 'S')
                    start = new Point(x, y); // Start point
                else if (lines[y][x] == 'E')
                    end = new Point(x, y); // End point
            }
        }

        // Current position starts at the start point
        var pos = new Point(start.x, start.y);
        int previousX = -1, previousY = -1;

        // Traverse until reaching the end point
        while (!(pos.x == end.x && pos.y == end.y))
        {
            int dY, dX;

            // Check all four directions around current position
            for (var i = 0; i < 4; i++)
            {
                dY = pos.y + deltaMap[i, 0];
                dX = pos.x + deltaMap[i, 1];

                // Move only if not an obstacle and not going back to previous position
                if (racetrack[dY, dX] != -1 && !(dX == previousX && dY == previousY))
                {
                    // Mark the distance from the start
                    racetrack[dY, dX] = racetrack[pos.y, pos.x] + 1;

                    // Update previous position to current
                    previousX = pos.x;
                    previousY = pos.y;

                    // Move to the new position
                    pos.x = dX;
                    pos.y = dY;

                    break; // Exit direction loop after moving
                }
            }
        }

        var answer = 0;

        // Analyze the racetrack to identify special positions that are obstacles (-1)
        for (var y = 1; y < lines.Length - 1; y++)
        {
            for (var x = 1; x < lines[0].Length - 1; x++)
            {
                if (racetrack[y, x] == -1)
                {
                    // Check if obstacle is between two walkable vertical cells
                    if (racetrack[y - 1, x] != -1 && racetrack[y + 1, x] != -1)
                        cheat(racetrack[y - 1, x], racetrack[y + 1, x]);
                    // Or between two walkable horizontal cells
                    else if (racetrack[y, x - 1] != -1 && racetrack[y, x + 1] != -1)
                        cheat(racetrack[y, x - 1], racetrack[y, x + 1]);
                }
            }
        }

        // Output the final answer
        Console.WriteLine(answer);

        // Helper method to increment answer if distance difference indicates something special
        void cheat(int a, int b)
        {
            if (saved(a, b, 2) >= 100)
                answer++;
        }

        // Calculate saved distance between two positions, considering steps
        int saved(int a, int b, int steps)
        {
            return a > b ? a - b - steps : b - a - steps;
        }
    }

    // Simple Point class to hold X and Y coordinates
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
