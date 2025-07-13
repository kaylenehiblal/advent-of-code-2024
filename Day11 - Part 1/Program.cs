using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read the input line and parse it into a list of stones (long integers)
        var lines = File.ReadAllLines("input.txt");
        var stones = lines[0].Split(' ').Select(long.Parse).ToList();

        // Cache to store computed results of changeStones for efficiency
        var cache = new Dictionary<long, List<long>>();

        // Perform 25 iterations of stone transformations
        for (int iteration = 0; iteration < 25; iteration++)
        {
            var newStones = new List<long>();

            // For each stone, get the transformed stones and accumulate them
            foreach (var stone in stones)
            {
                newStones.AddRange(ChangeStones(stone));
            }

            stones = newStones;
        }

        // Output the total count of stones after all transformations
        Console.WriteLine(stones.Count);

        // ----------------------
        // Local helper function
        // ----------------------

        // Transforms a stone based on specific rules with caching for repeated values
        List<long> ChangeStones(long engraving)
        {
            // Return cached result if available
            if (cache.TryGetValue(engraving, out var cachedResult))
                return cachedResult;

            var result = new List<long>();
            string engravingStr = engraving.ToString();

            if (engraving == 0)
            {
                // If engraving is zero, transform it to 1
                result.Add(1);
            }
            else if (engravingStr.Length % 2 == 0)
            {
                // If length of engraving is even, split into two halves and parse them
                int halfLength = engravingStr.Length / 2;
                result.Add(long.Parse(engravingStr.Substring(0, halfLength)));
                result.Add(long.Parse(engravingStr.Substring(halfLength)));
            }
            else
            {
                // If length is odd, multiply engraving by 2024
                result.Add(engraving * 2024);
            }

            // Cache the computed result for future calls
            cache[engraving] = result;
            return result;
        }
    }
}
