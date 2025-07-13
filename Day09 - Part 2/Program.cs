using System;
using System.IO;

class Program
{
    static void Main()
    {
        // Read the disk map string from the input file (first line only)
        string diskMap = File.ReadAllLines("input.txt")[0];

        // Calculate the total length of all blocks by summing digits in diskMap
        int totalLength = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            totalLength += diskMap[i] - '0';
        }

        // Initialize an array representing the disk blocks
        // The last block is set to -1 to mark an end or invalid block
        int[] blocks = new int[totalLength + 1];
        blocks[^1] = -1;

        // Fill blocks array: even index positions get the file index (i / 2), odd positions get -1
        int writePos = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            int count = diskMap[i] - '0';
            for (int j = 0; j < count; j++)
            {
                blocks[writePos++] = (i % 2 == 0) ? i / 2 : -1;
            }
        }

        int currentFile = diskMap.Length / 2;  // Start scanning from highest file index
        int searchStart = 0;                    // Starting point for searching empty blocks

        // Loop backward through blocks to try and rearrange files to earlier positions
        for (int i = blocks.Length - 1; i >= 0; i--)
        {
            // Detect block that starts a sequence for the current file, preceded by a different file
            if (i > 0 && blocks[i] == currentFile && blocks[i - 1] != currentFile)
            {
                // Determine length of contiguous blocks belonging to currentFile (up to 9 max)
                int fileLength = 1;
                while (fileLength < 9 && i + fileLength < blocks.Length && blocks[i + fileLength] == currentFile)
                {
                    fileLength++;
                }

                int firstEmptyIndex = -1;

                // Try to find an empty slot before current position where this file can fit
                for (int j = searchStart; j <= blocks.Length - fileLength && j < i; j++)
                {
                    if (blocks[j] == -1)
                    {
                        if (firstEmptyIndex == -1)
                            firstEmptyIndex = j;

                        bool fits = true;
                        for (int k = 1; k < fileLength; k++)
                        {
                            if (blocks[j + k] != -1)
                            {
                                fits = false;
                                break;
                            }
                        }

                        // If found a fitting empty block, move the file there and clear old blocks
                        if (fits)
                        {
                            for (int k = 0; k < fileLength; k++)
                            {
                                blocks[j + k] = currentFile;
                                blocks[i + k] = -1;
                            }
                            break;
                        }
                    }
                }

                // Update search start index for next iteration to optimize search
                if (firstEmptyIndex > -1)
                    searchStart = firstEmptyIndex;

                // Stop if no more progress can be made or currentFile is zero
                if (searchStart == i || currentFile == 0)
                    break;

                currentFile--;
            }
        }

        // Compute checksum by summing file index * block index for all allocated blocks
        long checksum = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] > -1)
                checksum += (long)blocks[i] * i;
        }

        // Output the checksum result
        Console.WriteLine(checksum);
    }
}
