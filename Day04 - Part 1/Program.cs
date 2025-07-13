using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified file
        string[] lines = File.ReadAllLines("input.txt");

        // Variable to store the count of found "XMAS" words
        int answer = 0;

        // Loop through every character in every line of the grid
        for (int line = 0; line < lines.Length; line++)
        {
            for (int character = 0; character < lines[line].Length; character++)
            {
                // If the current character is 'X', it's a potential starting point
                if (lines[line][character] == 'X')
                {
                    // Try all 8 directions from the current (line, character) position
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            // Skip the (0, 0) direction (no movement)
                            if (dx != 0 || dy != 0)
                                Search(line, character, dx, dy);
                        }
                    }
                }
            }
        }

        // Output the total number of "XMAS" sequences found
        Console.WriteLine(answer);

        // Local function to search in a specific direction from (y, x)
        void Search(int y, int x, int dX, int dY)
        {
            string str = string.Empty;

            // Move in the specified direction for 4 steps (looking for "XMAS")
            for (int step = 1; step <= 4; step++, y += dX, x += dY)
            {
                // Check grid boundaries before accessing the character
                if (y >= 0 && y < lines.Length && x >= 0 && x < lines[y].Length)
                    str += lines[y][x];
            }

            // If the collected string is exactly "XMAS", increment the count
            if (str == "XMAS")
                answer++;
        }
    }
}
