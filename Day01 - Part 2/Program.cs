using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified input file path
        string[] lines = File.ReadAllLines("input.txt");

        // Variable to hold the final calculated answer
        int answer = 0;

        // Create an array of two integer lists to store left and right columns separately
        var lists = new List<int>[2];
        for (int i = 0; i < 2; i++)
            lists[i] = new List<int>();

        // Process each line of input
        foreach (var line in lines)
        {
            // Split the line using triple spaces to extract both column values
            var numbers = line.Split("   ");

            // Parse and add the numbers to the corresponding column list
            for (int i = 0; i < 2; i++)
                lists[i].Add(int.Parse(numbers[i]));
        }

        // Loop through each value in the left column
        for (int i = 0; i < lists[0].Count; i++)
        {
            // Count how many times the current left-column number appears in the right column
            int occurrence = 0;
            for (int j = 0; j < lists[1].Count; j++)
            {
                if (lists[0][i] == lists[1][j])
                    occurrence++;
            }

            // Multiply the left-column number by its count of appearances and add to the answer
            answer += lists[0][i] * occurrence;
        }

        // Output the final answer
        Console.WriteLine(answer);
    }
}
