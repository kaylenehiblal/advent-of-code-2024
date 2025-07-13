using System;
using System.IO;
using System.Collections.Generic;

var lines = File.ReadAllLines("input.txt");

// Lists to hold the height profiles of locks and keys
var locks = new List<int[]>();
var keys = new List<int[]>();

var answer = 0;

// Process every 8-line block in the input (7 lines per lock/key, 8th line as a separator)
for (var i = 0; i < lines.Length; i += 8)
{
    var heights = new int[5]; // Tracks the number of '#' per column in a 5-wide grid

    // Count '#' characters per column across 7 lines
    for (var j = i; j < i + 7; j++)
    {
        for (var x = 0; x < 5; x++)
        {
            if (lines[j][x] == '#')
                heights[x]++;
        }
    }

    // If the top line of the block contains no '.', treat it as a lock; otherwise, as a key
    if (!lines[i].Contains('.'))
        locks.Add(heights);
    else
        keys.Add(heights);
}

// Compare each key to each lock to see if they can fit together
foreach (var l0ck in locks)
{
    foreach (var key in keys)
    {
        var fits = true;

        // Check each of the 5 columns: sum of key and lock column heights must not exceed 7
        for (var x = 0; x < 5; x++)
        {
            if (key[x] + l0ck[x] > 7)
            {
                fits = false;
                break;
            }
        }

        if (fits)
            answer++;
    }
}

// Output the number of valid key-lock pairings
Console.WriteLine(answer);
