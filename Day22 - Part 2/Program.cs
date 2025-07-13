using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read and parse each line as a long using a custom parser
        var secrets = File.ReadAllLines("input.txt")
                          .Select(ParseLong)
                          .ToArray();

        // Dictionary to store sequence values and their aggregated prices
        var bananas = new Dictionary<int, int>();

        // Process each secret individually
        for (int i = 0; i < secrets.Length; i++)
        {
            var changes = new int[2000];
            var occurrences = new List<int>();

            for (int j = 0; j < 2000; j++)
            {
                // Apply the pseudo-random transformation
                var newSecret = Pseudo(secrets[i]);
                changes[j] = Price(newSecret) - Price(secrets[i]);
                secrets[i] = newSecret;

                // Once we have 4 values, form a sequence and track it
                if (j >= 3)
                {
                    // Encode a 4-digit sequence in base 18
                    var sequence = changes[j - 3] * 5832 + changes[j - 2] * 324 + changes[j - 1] * 18 + changes[j];

                    // Track unique sequences per secret
                    if (!occurrences.Contains(sequence))
                    {
                        if (bananas.ContainsKey(sequence))
                            bananas[sequence] += Price(secrets[i]);
                        else
                            bananas.Add(sequence, Price(secrets[i]));

                        occurrences.Add(sequence);
                    }
                }
            }
        }

        // Output the maximum aggregated value across all sequences
        Console.WriteLine(bananas.Values.Max());
    }

    // Custom parser to convert string to long without using long.Parse.
    static long ParseLong(string s)
    {
        long result = 0;
        for (int i = 0; i < s.Length; i++)
            result = result * 10 + (s[i] - '0');
        return result;
    }

    // Returns the price value of a secret (last digit).
    static int Price(long secret) => (int)(secret % 10);

    // Applies a 3-step pseudo-random transformation on a secret.
    static long Pseudo(long secret)
    {
        secret = Mix(secret, secret << 6);
        secret = Prune(secret);

        secret = Mix(secret, secret >> 5);
        secret = Prune(secret);

        secret = Mix(secret, secret << 11);
        secret = Prune(secret);

        return secret;
    }

    // Bitwise XOR mix of secret and value.
    static long Mix(long secret, long value) => secret ^ value;

    // Prunes the secret to 24-bit space.
    static long Prune(long secret) => secret & 0xFFFFFF;
}
