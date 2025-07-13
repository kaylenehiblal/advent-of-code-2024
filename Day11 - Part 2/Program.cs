using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read input line and parse initial stones into a dictionary
        // Key = engraving value, Value = multiplier (count of how many times it appears)
        var lines = File.ReadAllLines("input.txt");
        var stones = new Dictionary<long, long>();
        foreach (var stone in lines[0].Split(' ').Select(long.Parse))
        {
            stones[stone] = 1;
        }

        // Cache for storing results of changeStones to avoid repeated calculations
        var cache = new Dictionary<long, List<long>>();

        // Perform 75 iterations of stone transformations with multiplier tracking
        for (int iteration = 0; iteration < 75; iteration++)
        {
            var nextStones = new Dictionary<long, long>();

            foreach (var stone in stones.Keys)
            {
                long multiplier = stones[stone];

                // For each transformed stone, update its multiplier in the next dictionary
                foreach (var newStone in ChangeStones(stone))
                {
                    if (!nextStones.TryAdd(newStone, multiplier))
                    {
                        nextStones[newStone] += multiplier;
                    }
                }
            }

            stones = nextStones;
        }

        // Calculate the sum of all multipliers for the final stones collection
        long answer = 0;
        foreach (var multiplier in stones.Values)
        {
            answer += multiplier;
        }

        // Output the total count of stones after all transformations
        Console.Write(answer);

        // ----------------------
        // Local helper function
        // ----------------------

        // Returns transformed stones from a given engraving with caching
        List<long> ChangeStones(long engraving)
        {
            if (cache.TryGetValue(engraving, out var cachedResult))
                return cachedResult;

            var result = new List<long>();
            string engravingStr = engraving.ToString();

            if (engraving == 0)
            {
                result.Add(1);
            }
            else if (engravingStr.Length % 2 == 0)
            {
                int halfLen = engravingStr.Length / 2;
                result.Add(long.Parse(engravingStr.Substring(0, halfLen)));
                result.Add(long.Parse(engravingStr.Substring(halfLen)));
            }
            else
            {
                result.Add(engraving * 2024);
            }

            cache[engraving] = result;
            return result;
        }
    }
}
