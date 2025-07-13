using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read the disk map string from the input file (only the first line)
        string diskMap = File.ReadAllLines("input.txt")[0];

        // Convert each character digit into an integer and store in an array
        int[] intMap = new int[diskMap.Length];
        for (int i = 0; i < diskMap.Length; i++)
        {
            intMap[i] = diskMap[i] - '0';
        }

        int rightBoundary = diskMap.Length - 1; // Rightmost index for scanning
        long checksum = 0;                      // Accumulate result here
        int index = 0;                          // Tracks the incremental index for checksum calculation

        // Iterate over each position in the disk map
        for (int i = 0; i < diskMap.Length; i++)
        {
            if (i % 2 == 0)
            {
                // For even indices: add (i/2 * index) repeated intMap[i] times
                for (int j = 0; j < intMap[i]; j++)
                {
                    checksum += (i / 2) * index;
                    index++;
                }
            }
            else
            {
                // For odd indices: perform different calculation involving takeLast
                for (int j = 0; j < intMap[i]; j++)
                {
                    int x = TakeLast(i);
                    if (x >= 0)
                    {
                        checksum += x * index;
                        index++;
                    }
                    else
                    {
                        // No more valid values found; exit outer loop early
                        i = diskMap.Length;
                        break;
                    }
                }
            }
        }

        // Output the final computed checksum
        Console.WriteLine(checksum);

        // ----------------------
        // Local helper function
        // ----------------------

        // Searches backwards from the right boundary towards the left boundary,
        // decrementing values in intMap for valid entries and returning their half-index.
        int TakeLast(int leftBoundary)
        {
            for (int i = rightBoundary; i > leftBoundary; i -= 2)
            {
                if (intMap[i] > 0)
                {
                    if (intMap[i] == 1)
                    {
                        rightBoundary = i; // Update boundary to avoid rescanning exhausted indices
                    }
                    intMap[i]--;
                    return i / 2;
                }
            }
            return -1; // Indicate no valid index found
        }
    }
}
