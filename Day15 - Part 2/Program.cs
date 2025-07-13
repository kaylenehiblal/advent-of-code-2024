using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Load input file
        var lines = File.ReadAllLines("input.txt");

        // Direction symbols and their (dy, dx) movements: ^ > v <
        var directions = "^>v<";
        var deltaMap = new int[4, 2] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };

        var warehouse = new List<string>();
        var moves = new List<string>();
        var robot = new Obstacle(0, 0, ObstacleType.Robot); // default position

        // Parse map layout and movement instructions
        for (int y = 0; y < lines.Length; y++)
        {
            if (lines[y].StartsWith('#'))
                warehouse.Add(lines[y]);
            else if (!string.IsNullOrWhiteSpace(lines[y]))
                moves.Add(lines[y]);

            if (lines[y].Contains('@'))
                robot = new Obstacle(lines[y].IndexOf('@') * 2, y, ObstacleType.Robot);
        }

        // Initialize grid of obstacles (expanded x-dimension for mid-points)
        var obstacles = new Obstacle?[warehouse.Count, warehouse[0].Length * 2];
        for (int y = 0; y < warehouse.Count; y++)
        {
            for (int x = 0; x < warehouse[0].Length; x++)
            {
                char cell = warehouse[y][x];
                if (cell == '#')
                    obstacles[y, x * 2] = new Obstacle(x * 2, y, ObstacleType.Wall);
                else if (cell == 'O')
                    obstacles[y, x * 2] = new Obstacle(x * 2, y, ObstacleType.Box);
            }
        }

        // Execute all movement instructions
        foreach (var line in moves)
            foreach (var move in line)
                AttemptMove(directions.IndexOf(move));

        // Calculate final score based on box positions
        long answer = 0;
        for (int y = 0; y < obstacles.GetLength(0); y++)
        {
            for (int x = 0; x < obstacles.GetLength(1); x++)
            {
                if (obstacles[y, x]?.type == ObstacleType.Box)
                    answer += 100 * y + x;
            }
        }

        Console.WriteLine(answer);

        // ----------------------------
        // Function to attempt robot move
        // ----------------------------
        void AttemptMove(int direction)
        {
            int dY = robot.y + deltaMap[direction, 0];
            int dX = robot.x + deltaMap[direction, 1];

            // Try to identify an obstacle in the intended direction
            Obstacle? obstruction = null;
            if (direction != 3 && obstacles[dY, dX] != null)
                obstruction = obstacles[dY, dX];
            else if (direction != 1 && obstacles[dY, dX - 1] != null)
                obstruction = obstacles[dY, dX - 1];

            // Blocked by wall
            if (obstruction?.type == ObstacleType.Wall)
                return;

            // Try moving any boxes in the way
            if (obstruction != null)
            {
                if (obstruction.TryMove(obstacles, deltaMap, direction, false))
                    obstruction.TryMove(obstacles, deltaMap, direction, true);
                else
                    return;
            }

            // Move robot if unblocked
            robot.y = dY;
            robot.x = dX;
        }
    }

    // -----------------------------------
    // Obstacle class: represents any object on the grid
    // -----------------------------------
    internal class Obstacle(int x, int y, ObstacleType type)
    {
        public int x = x;
        public int y = y;
        public ObstacleType type = type;

        public bool TryMove(Obstacle?[,] obstacles, int[,] deltaMap, int direction, bool doMove)
        {
            int dX = x + deltaMap[direction, 1];
            int dY = y + deltaMap[direction, 0];

            var boxes = new List<Obstacle>();

            // Check primary tile
            if (direction % 2 == 0 && obstacles[dY, dX] != null)
            {
                if (obstacles[dY, dX]?.type == ObstacleType.Wall)
                    return false;
                boxes.Add(obstacles[dY, dX]);
            }

            // Check side tiles for composite movement logic
            if (direction != 1 && obstacles[dY, dX - 1] != null)
            {
                if (obstacles[dY, dX - 1]?.type == ObstacleType.Wall)
                    return false;
                boxes.Add(obstacles[dY, dX - 1]);
            }

            if (direction != 3 && obstacles[dY, dX + 1] != null)
            {
                if (obstacles[dY, dX + 1]?.type == ObstacleType.Wall)
                    return false;
                boxes.Add(obstacles[dY, dX + 1]);
            }

            // If no blocking boxes, we can move
            if (boxes.Count == 0)
            {
                if (doMove)
                    Move(obstacles, deltaMap, direction);
                return true;
            }
            else
            {
                // Try to recursively move all boxes first
                bool canMove = true;
                foreach (var box in boxes)
                    canMove &= box.TryMove(obstacles, deltaMap, direction, false);

                if (canMove && doMove)
                {
                    foreach (var box in boxes)
                        box.TryMove(obstacles, deltaMap, direction, true);
                    Move(obstacles, deltaMap, direction);
                }

                return canMove;
            }
        }

        // Moves the obstacle to a new position in the direction specified
        private void Move(Obstacle?[,] obstacles, int[,] deltaMap, int direction)
        {
            obstacles[y, x] = null;

            x += deltaMap[direction, 1];
            y += deltaMap[direction, 0];

            obstacles[y, x] = this;
        }
    }

    // -----------------------------------
    // Enum to define type of each obstacle
    // -----------------------------------
    public enum ObstacleType
    {
        Box,
        Robot,
        Wall
    }
}
