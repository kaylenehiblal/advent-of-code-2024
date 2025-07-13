using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines
        var lines = File.ReadAllLines("input.txt");

        // Directions: ^ = up, > = right, v = down, < = left
        var directions = "^>v<";
        var deltaMap = new int[4, 2] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };

        var mapList = new List<string>();
        var moves = new List<string>();
        var robot = new Robot(0, 0);  // Default position, will be updated

        // Parse input lines into map and moves
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith('#'))
                mapList.Add(lines[i]);
            else if (!string.IsNullOrWhiteSpace(lines[i]))
                moves.Add(lines[i]);

            if (lines[i].Contains('@'))
                robot = new Robot(lines[i].IndexOf('@'), i);
        }

        // Initialize the 2D map grid
        var map = new char[mapList.Count, mapList[0].Length];
        for (int y = 0; y < mapList.Count; y++)
            for (int x = 0; x < mapList[y].Length; x++)
                map[y, x] = mapList[y][x];

        // Process each move instruction
        foreach (var line in moves)
            foreach (var move in line)
                AttemptMove(directions.IndexOf(move));

        // Calculate final result: for each 'O', sum 100 * y + x
        long answer = 0;
        for (int y = 0; y < map.GetLength(0); y++)
            for (int x = 0; x < map.GetLength(1); x++)
                if (map[y, x] == 'O')
                    answer += 100 * y + x;

        Console.WriteLine(answer);

        // ----------------------------
        // Local Function Definitions
        // ----------------------------

        void AttemptMove(int direction)
        {
            int dY = robot.y + deltaMap[direction, 0];
            int dX = robot.x + deltaMap[direction, 1];

            // Move if next position is an empty tile
            if (map[dY, dX] == '.')
            {
                map[robot.y, robot.x] = '.';
                map[dY, dX] = '@';
                robot.y = dY;
                robot.x = dX;
            }

            // If stepping into an 'O', attempt to slide it forward
            else if (map[dY, dX] == 'O')
            {
                int nextY = dY, nextX = dX;
                int steps = 0;
                bool wallHit = false, foundEmpty = false;

                // Look ahead to see how far we can push the 'O'
                while (!wallHit && !foundEmpty)
                {
                    nextY += deltaMap[direction, 0];
                    nextX += deltaMap[direction, 1];
                    steps++;

                    if (map[nextY, nextX] == '.')
                        foundEmpty = true;
                    else if (map[nextY, nextX] == '#')
                        wallHit = true;
                }

                // If there is space, slide all blocks forward and move robot
                if (foundEmpty)
                {
                    for (int i = 0; i <= steps; i++)
                    {
                        int prevY = nextY - deltaMap[direction, 0];
                        int prevX = nextX - deltaMap[direction, 1];

                        map[nextY, nextX] = map[prevY, prevX];

                        // If robot was part of the shifted trail, update its position
                        if (map[prevY, prevX] == '@')
                        {
                            map[robot.y, robot.x] = '.';
                            robot.y = nextY;
                            robot.x = nextX;
                        }

                        nextY = prevY;
                        nextX = prevX;
                    }
                }
            }
        }
    }

    // Class representing the robot's position on the grid
    internal class Robot
    {
        public int x;
        public int y;

        public Robot(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
