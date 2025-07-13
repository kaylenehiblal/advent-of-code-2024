using System;
using System.Collections.Generic;
using System.IO;

var lines = File.ReadAllLines("input.txt");

var nboRobots = 2; // Number of directional robots

var numpad = new NumericKeypad(); // Numeric keypad instance
var robots = new List<DirectionalKeypad>();
for (var i = 0; i < nboRobots; i++)
    robots.Add(new DirectionalKeypad()); // Initialize directional keypads for robots

var answer = 0;
// Process each input code line to compute the total score
foreach (var line in lines)
    answer += typeCode(line);

Console.WriteLine(answer);

// Processes a single code string and calculates a numeric score
int typeCode(string code)
{
    var requiredSequence = string.Empty;

    // For each character in the code, find moves on the numeric keypad
    foreach (var character in code)
    {
        var numpadSequence = numpad.MoveTo(character);

        // For each move on the numeric keypad, recursively press buttons with all robots
        foreach (var move in numpadSequence)
            pressButtons(move, 0, ref requiredSequence);
    }

    // Score is the length of the required sequence multiplied by the numeric value of the code excluding last char
    return requiredSequence.Length * int.Parse(code[..^1]);
}

// Recursive function to simulate pressing buttons by multiple robots sequentially
void pressButtons(char button, int robotIndex, ref string result)
{
    var sequence = robots[robotIndex].MoveTo(button);

    if (robotIndex == nboRobots - 1)
    {
        // If last robot, append the sequence to the result
        result += sequence;
    }
    else
    {
        // Otherwise, continue with the next robot
        foreach (var move in sequence)
            pressButtons(move, robotIndex + 1, ref result);
    }
}

// Directional keypad used by the robots
class DirectionalKeypad : Keypad
{
    public DirectionalKeypad()
    {
        // Initialize position and buttons layout
        x = 2;
        y = 0;

        buttons = new char[2, 3]
        {
            { 'X', '^', 'A' },
            { '<', 'v', '>' }
        };
    }

    // Move to a specific button and return the sequence of moves plus an 'A' to press
    public string MoveTo(char button)
    {
        var requiredSequence = string.Empty;

        for (var dY = 0; dY < 2; dY++)
        {
            for (var dX = 0; dX < 3; dX++)
            {
                if (buttons[dY, dX] == button)
                {
                    // Determine order of moves for optimal path
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

                    requiredSequence += "A"; // Press button

                    x = dX;
                    y = dY;
                }
            }
        }

        return requiredSequence;
    }
}

// Numeric keypad class representing the number pad layout
class NumericKeypad : Keypad
{
    public NumericKeypad()
    {
        // Initialize position and buttons layout
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

    // Move to a specific button and return the sequence of moves plus an 'A' to press
    public string MoveTo(char button)
    {
        var requiredSequence = string.Empty;

        for (var dY = 0; dY < 4; dY++)
        {
            for (var dX = 0; dX < 3; dX++)
            {
                if (buttons[dY, dX] == button)
                {
                    // Determine move order depending on position to minimize moves
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

                    requiredSequence += "A"; // Press button

                    x = dX;
                    y = dY;
                }
            }
        }

        return requiredSequence;
    }
}

// Base class for keypad functionality with shared movement helpers
class Keypad
{
    public char[,] buttons;

    public int x;
    public int y;

    // Generate left/right movement sequence
    protected string MotionsX(int x, int dX)
    {
        if (dX > x)
            return string.Empty.PadLeft(Distance(x, dX), '>');
        else
            return string.Empty.PadLeft(Distance(x, dX), '<');
    }

    // Generate up/down movement sequence
    protected string MotionsY(int y, int dY)
    {
        if (dY > y)
            return string.Empty.PadLeft(Distance(y, dY), 'v');
        else
            return string.Empty.PadLeft(Distance(y, dY), '^');
    }

    // Calculate absolute distance between two positions
    protected int Distance(int a, int b) => Math.Abs(a - b);
}
