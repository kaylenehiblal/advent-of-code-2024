using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Read all input lines from the specified input file
        string[] lines = File.ReadAllLines("input.txt");

        // Variable to store the final answer
        int answer = 0;

        // Create an array of two integer lists to store the two columns separately
        var lists = new List<int>[2];
        for (int i = 0; i < 2; i++)
            lists[i] = new List<int>();

        // Process each line in the input
        foreach (var line in lines)
        {
            // Split each line using triple space as the delimiter to get the two numbers
            var numbers = line.Split("   ");

            // Parse and store each number into its respective column list
            for (int i = 0; i < 2; i++)
                lists[i].Add(int.Parse(numbers[i]));
        }

        // Sort both lists independently
        for (int i = 0; i < 2; i++)
            lists[i].Sort();

        // Calculate the sum of absolute differences between the sorted pairs
        for (int i = 0; i < lists[0].Count; i++)
            answer += Math.Abs(lists[0][i] - lists[1][i]);

        // Output the final answer
        Console.WriteLine(answer);
    }
}
