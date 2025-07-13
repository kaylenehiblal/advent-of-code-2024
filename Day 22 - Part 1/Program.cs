using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Read and parse the input file into a long array of secret numbers.
        var secrets = File.ReadAllLines("input.txt")
                          .Select(long.Parse)
                          .ToArray();

        long answer = 0;

        // For each secret, apply the pseudo() function 2000 times
        for (int i = 0; i < secrets.Length; i++)
        {
            for (int j = 0; j < 2000; j++)
                secrets[i] = Pseudo(secrets[i]);

            answer += secrets[i];
        }

        // Output the final answer
        Console.WriteLine(answer);
    }

    // Applies a three-step pseudorandom transformation to a secret value.
    static long Pseudo(long secret)
    {
        // Step 1: XOR with secret * 64, then prune
        secret = Mix(secret, secret * 64);
        secret = Prune(secret);

        // Step 2: XOR with secret / 32, then prune
        secret = Mix(secret, secret / 32);
        secret = Prune(secret);

        // Step 3: XOR with secret * 2048, then prune
        secret = Mix(secret, secret * 2048);
        secret = Prune(secret);

        return secret;
    }

    // Mixes two long values using XOR.
    static long Mix(long secret, long value) => secret ^ value;

    // Prunes the secret to stay within 24-bit range (modulo 2^24).
    static long Prune(long secret) => secret % 16777216;
}
