using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines("input.txt");

        var answer = 0L;
        var availablePatterns = lines[0].Split(", ");

        for (int i = 2; i < lines.Length; i++)
            answer += SortTowels(lines[i], availablePatterns);

        Console.WriteLine(answer);
    }

    static long SortTowels(string design, string[] availablePatterns)
    {
        var towels = new List<string>[design.Length];
        for (int i = 0; i < design.Length; i++)
            towels[i] = new List<string>();

        foreach (var pattern in availablePatterns)
        {
            int lastIndex = 0;

            while (design[lastIndex..].Contains(pattern))
            {
                int index = design.IndexOf(pattern, lastIndex);
                towels[index].Add(pattern);
                lastIndex = index + 1;
            }
        }

        var possibilities = new long[design.Length];

        for (int i = towels.Length - 1; i >= 0; i--)
        {
            foreach (var towel in towels[i])
            {
                if (i + towel.Length < design.Length)
                    possibilities[i] += possibilities[i + towel.Length];
                else
                    possibilities[i]++;
            }
        }

        return possibilities[0];
    }
}
