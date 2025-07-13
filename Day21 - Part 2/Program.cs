using System;
using System.Collections.Generic;
using System.IO;

var lines = File.ReadAllLines("input.txt");

var nboRobots = 25; // Number of directional robots

var numpad = new NumericKeypad(); // Numeric keypad instance
var robots = new List<DirectionalKeypad>();
for (var i = 0; i < nboRobots; i++)
    robots.Add(new DirectionalKeypad()); // Initialize all directional keypads (robots)

var cache = new long[5, 5, nboRobots]; // Cache to store previously computed move lengths

long answer = 0L;
// Process each input line, summing the calculated lengths
foreach (var line in lines)
    answer += typeCode(line);

Console.WriteLine(answer);

// Processes a single code string, calculating the total length of required move sequences
long typeCode(string code)
{
    long requiredSequenceLength = 0L;
    foreach (var character in code)
    {
        var numpadSequence = numpad.MoveTo(character);

        // For each move from numeric keypad, recursively press buttons across robots
        foreach (var move in numpadSequence)
            pressButtons(move, 0, ref requiredSequenceLength);
    }

    // Multiply total sequence length by the numeric value of the code (excluding last character)
    return requiredSequenceLength * int.Parse(code[..^1]);
}

// Recursive method to handle button presses by robots with caching of results to optimize
void pressButtons(char button, int robotIndex, ref long length)
{
    // Current position index on keypad mapped as y*3 + x -1
    var current = robots[robotIndex].y * 3 + robots[robotIndex].x - 1;
    // Target move mapping for cache lookup
    var target = button switch
    {
        '^' => 0,
        'A' => 1,
        '<' => 2,
        'v' => 3,
        _ => 4
    };

    // Use cached result if available to avoid recomputation
    if (cache[current, target, robotIndex] > 0)
    {
        length += cache[current, target, robotIndex];
        robots[robotIndex].MoveTo(button);
        return;
    }

    var lengthBefore = length;

    // Calculate move sequence length for current robot
    var sequence = robots[robotIndex].MoveTo(button);

    if (robotIndex == nboRobots - 1)
        length += sequence.Length; // If last robot, add length directly
    else
        foreach (var move in sequence)
            pressButtons(move, robotIndex + 1, ref length); // Otherwise continue recursion

    // Store computed length difference in cache for reuse
    cache[current, target, robotIndex] = length - lengthBefore;
}

// Directional keypad class used by robots
class DirectionalKeypad : Keypad
{
    public DirectionalKeypad()
    {
        x = 2;
        y = 0;

        buttons = new char[2, 3]
        {
            { 'X', '^', 'A' },
            { '<', 'v', '>' }
        };
    }

    // Returns the sequence of moves to reach a button and "A" to press it
    public string MoveTo(char button)
    {
        var requiredSequence = string.Empty;

        for (var dY = 0; dY < 2; dY++)
        {
            for (var dX = 0; dX < 3; dX++)
            {
                if (buttons[dY, dX] == button)
                {
                    if (dX == 0 && x > 0)
                    {
                        requiredSequence += MotionsY(y, dY);
                        requiredSequence += MotionsX(x, dX);
                    }
                    else if (x == 0 && dY == 0)
                    {
                        requiredSequence += MotionsX(x, dX);
                        requiredSequence += MotionsY(y, dY);
                    }
                    else
                    {
                        if (dX < x)
                        {
                            requiredSequence += MotionsX(x, dX);
                            requiredSequence += MotionsY(y, dY);
                        }
                        else
                        {
                            requiredSequence += MotionsY(y, dY);
                            requiredSequence += MotionsX(x, dX);
                        }
                    }
                    requiredSequence += "A";

                    x = dX;
                    y = dY;
                }
            }
        }

        return requiredSequence;
    }
}

// Numeric keypad class representing the numeric pad layout
class NumericKeypad : Keypad
{
    public NumericKeypad()
    {
        x = 2;
        y = 3;

        buttons = new char[4, 3]
        {
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { 'X', '0', 'A' }
        };
    }

    // Returns the sequence of moves to reach a button and "A" to press it
    public string MoveTo(char button)
    {
        var requiredSequence = string.Empty;

        for (var dY = 0; dY < 4; dY++)
        {
            for (var dX = 0; dX < 3; dX++)
            {
                if (buttons[dY, dX] == button)
                {
                    if (y == 3 && dY < 3 && dX == 0)
                    {
                        requiredSequence += MotionsY(y, dY);
                        requiredSequence += MotionsX(x, dX);
                    }
                    else if (dY == 3 && x == 0)
                    {
                        requiredSequence += MotionsX(x, dX);
                        requiredSequence += MotionsY(y, dY);
                    }
                    else
                    {
                        if (dX < x)
                        {
                            requiredSequence += MotionsX(x, dX);
                            requiredSequence += MotionsY(y, dY);
                        }
                        else
                        {
                            requiredSequence += MotionsY(y, dY);
                            requiredSequence += MotionsX(x, dX);
                        }
                    }
                    requiredSequence += "A";

                    x = dX;
                    y = dY;
                }
            }
        }

        return requiredSequence;
    }
}

// Base keypad class containing common movement helpers
class Keypad
{
    public char[,] buttons;

    public int x;
    public int y;

    // Generate left/right movement sequence using '<' or '>'
    protected string MotionsX(int x, int dX)
    {
        if (dX > x)
            return string.Empty.PadLeft(Distance(x, dX), '>');
        else
            return string.Empty.PadLeft(Distance(x, dX), '<');
    }

    // Generate up/down movement sequence using '^' or 'v'
    protected string MotionsY(int y, int dY)
    {
        if (dY > y)
            return string.Empty.PadLeft(Distance(y, dY), 'v');
        else
            return string.Empty.PadLeft(Distance(y, dY), '^');
    }

    // Compute absolute distance between two points
    protected static int Distance(int a, int b) => Math.Abs(a - b);
}
