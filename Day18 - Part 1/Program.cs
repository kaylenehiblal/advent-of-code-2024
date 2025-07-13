using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Load input containing memory block positions
        var lines = File.ReadAllLines("input.txt");

        // Movement directions: Up, Right, Down, Left
        var deltaMap = new int[4, 2] { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };

        var memoryMap = new bool[71, 71]; // Actual memory grid
        var nodes = new Node[73, 73];     // Padding + border for easy neighbor access
        var start = new Node(0, 0, isStart: true, isEnd: false);
        var end = new Node(0, 0, isStart: false, isEnd: true);

        // Mark blocked positions (i.e., memory used)
        for (int i = 0; i < 1024; i++)
        {
            var pos = lines[i].Split(',').Select(int.Parse).ToArray();
            memoryMap[pos[1], pos[0]] = true;
        }

        // Initialize valid nodes and identify start/end
        for (int y = 0; y < memoryMap.GetLength(0); y++)
        {
            for (int x = 0; x < memoryMap.GetLength(1); x++)
            {
                if (!memoryMap[y, x])
                {
                    var isStart = y == 0 && x == 0;
                    var isEnd = y == memoryMap.GetLength(0) - 1 && x == memoryMap.GetLength(1) - 1;

                    var node = new Node(x + 1, y + 1, isStart, isEnd);
                    nodes[y + 1, x + 1] = node;

                    if (isStart) start = node;
                    if (isEnd) end = node;
                }
            }
        }

        // Perform pathfinding (Dijkstra-like)
        RunSearch(start, end, nodes, deltaMap);

        // Backtrack from end to build path
        var path = new List<Node>();
        BuildPath(path, end);

        // Output the number of steps in the path
        Console.WriteLine(path.Count);
    }

    // -------------------------
    // Pathfinding using BFS with cost tracking
    // -------------------------
    static void RunSearch(Node start, Node end, Node[,] nodes, int[,] deltaMap)
    {
        start.minCostToStart = 0;
        var queue = new List<Node> { start };

        while (queue.Count > 0)
        {
            queue = queue.OrderBy(n => n.minCostToStart).ToList();
            var current = queue.First();
            queue.RemoveAt(0);

            // If end node reached, stop early
            if (current.end)
                return;

            for (int dir = 0; dir < 4; dir++)
            {
                int ny = current.y + deltaMap[dir, 0];
                int nx = current.x + deltaMap[dir, 1];
                var neighbor = nodes[ny, nx];

                if (neighbor == null || neighbor.visited)
                    continue;

                int newCost = current.minCostToStart.Value + 1;
                if (!neighbor.minCostToStart.HasValue || newCost < neighbor.minCostToStart)
                {
                    neighbor.minCostToStart = newCost;
                    neighbor.nearestToStart = current;

                    if (!queue.Contains(neighbor))
                        queue.Add(neighbor);
                }
            }

            current.visited = true;
        }
    }

    // -------------------------
    // Recursive path reconstruction
    // -------------------------
    static void BuildPath(List<Node> path, Node current)
    {
        if (current.nearestToStart == null)
            return;

        path.Add(current.nearestToStart);
        BuildPath(path, current.nearestToStart);
    }

    // -------------------------
    // Node Class
    // -------------------------
    internal class Node(int x, int y, bool isStart, bool isEnd)
    {
        public int x = x;
        public int y = y;

        public bool start = isStart;
        public bool end = isEnd;

        public int? minCostToStart = null;
        public bool visited = false;
        public Node nearestToStart = null;
    }
}
