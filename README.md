C# solutions to Advent of Code 2024 for MHA test.

(Days done 1, 2, 3, 4, 5, 6, 8, 9, 11, 13, 15, 18, 19, 20, 21, 22, 25 - Total 16.5 days done)

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 01 Solution

Each input line contains two integers separated by a space.

Input
- Input file: `input.txt`
- Sort the left and right columns independently. 


How it works
- For each corresponding pair, compute the absolute difference. Sum all these differences.
- Count how often each value in the right column appears. 
- For each number in the left column, multiply it by how many times it appears in the right column and sum all those values.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 02 Solution

Each line contains a space-separated list of integers representing a report.

Input
- Input file: `input.txt`
- Format: Space-separated list of integers

How it works
- A report is valid if it is monotonic (non-increasing or non-decreasing) and all adjacent differences are between 1 and 3.
- If a report is not valid, you may remove one number from the report. If the resulting report becomes valid, count it.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 03 Solution

The input is a single line of text containing operations.

Input
- Input file: `input.txt`
- Format: Find all mul(x,y) expressions.

How it works
- Multiply the values and sum them.
- Include do() and don't() toggles. Only evaluate mul(x,y) when active (initially active). 
- Toggling state is controlled by do() and don't().
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 04 Solution

Input
- Input file: `input.txt`
- Format: A 2D grid of characters is provided.

How it works
- Count how many times the word XMAS appears in any direction (8 total directions).
- For every A in the grid, check the diagonals (NW, NE, SW, SE). 
- If they form one of the valid MAS shapes (e.g., MMSS, MSSM, SSMM, SMMS), count them.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 05 Solution

Input
- Input file: `input.txt`
- Format: 
- Two sections separated by a blank line:
- Each line `X|Y` means if both pages X and Y are in the update, X must come before Y.
- Each line is a comma-separated list of page numbers representing a print job.


How it works
- Identify updates that already satisfy all ordering rules. For each valid update, take its middle element and sum them all.
- For updates that don't satisfy the rules, reorder the pages to comply (e.g. via swapping/fixing) and then also take the middle page. Sum these corrected midpoints.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 06 Solution

This solution processes a grid map representing walls and an initial position of a walker. It simulates walking along the grid, turning when walls are encountered, and detects loops reachable from the starting position. The answer is the count of such loop points.

Input
- Input file: `input.txt`
- Format: Grid of characters where `#` represents a wall and `^` marks the starting location.

How it works
- Parses the grid into a boolean wall map.
- Walks from the start position turning on walls.
- Uses a loop detection method to identify points that form loops.
- Counts these loop points.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 08 Solution

This program processes a grid with antennas identified by characters other than `.`. It calculates antinodes based on antenna positions and directional propagation.

Input
- Input file: `input.txt`
- Format: Grid of characters, `.` is empty space, other characters represent antennas.

How it works
- Reads the antenna positions.
- For each antenna pair of the same type, calculates antinodes in directions determined by their relative positions.
- Uses a helper function to mark antinodes by extending positions along those directions.
- Counts all unique antinode positions.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 09 Solution

This solution reads a disk map represented by a string of digits. It calculates a checksum based on values and their positions using two different strategies for even and odd indices.

Input
- Input file: `input.txt`
- Format: Single line string of digits representing disk blocks.

How it works
- Converts digits to integer map.
- For even positions, adds weighted sums.
- For odd positions, fetches last available value in the map.
- Outputs the checksum.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 11 Solution

This program simulates transformations on stones represented by numbers. Stones split into parts or multiply depending on their properties over multiple iterations.

Input
- Input file: `input.txt`
- Format: Single line of space-separated long integers representing stones.

How it works
- Uses a cache to avoid repeated calculations.
- For each stone, splits into two halves if length is even, or multiplies by 2024 if odd.
- Repeats this process for 75 iterations, counting how many stones exist after all transformations.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 13 Solution

Calculates prizes based on intersection points of two lines described in the input. Uses algebraic manipulation to find coefficients `a` and `b` and sums a weighted prize value.

Input
- Input file: `input.txt`
- Format: Multiple groups of 4 lines describing two lines and prize coordinates.

How it works
- Parses coordinates for two lines and prize point.
- Adjusts prize coordinates by adding a large offset.
- Solves linear equations to find integer coefficients.
- Sums weighted prizes where solutions are positive integers.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 15 Solution

Simulates robot movement in a warehouse grid with walls and movable boxes. Moves boxes if possible when robot pushes. Calculates a score based on box positions.

Input
- Input file: `input.txt`
- Format: Grid lines starting with `#` for warehouse, lines with moves (`^`, `>`, `v`, `<`).

How it works
- Parses warehouse layout into obstacles.
- Reads robot starting position.
- Processes moves, pushing boxes if possible.
- Calculates final score based on positions of boxes.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 18 Solution

Determines the maximum number of memory bytes that can be read while still having a path from start to end in a grid of occupied and free spaces.

Input
- Input file: `input.txt`
- Format: Lines of comma-separated integers representing occupied byte positions.

How it works
- Uses binary search over the number of bytes read.
- For each test, marks occupied spaces.
- Builds graph nodes for free spaces.
- Uses Dijkstra-like search to check if end node is reachable from start node.
- Prints the last line of input representing the maximal readable bytes.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 19 Solution

Determines how many designs can be formed by sequentially matching given patterns from a list of towels.

Input
- Input file: `input.txt`
- Format:
  - Line 1: comma-separated available patterns
  - Lines 3 onward: towel designs to check

How it works
- Splits patterns and iterates over towel designs.
- For each design, builds a dynamic programming solution counting the number of ways to fully cover it with patterns.
- Sums total number of designs that can be formed.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 20 Solution

Calculates the number of special pairs of reachable points on a racetrack grid map that meet specific distance and time savings criteria.

Input
- Input file: `input.txt`
- Format: Grid of characters representing walls (#), start (S), end (E), and open spaces.

How it works
- Parses the grid and identifies start and end points.
- Performs a directional search from start to end, marking step counts in a 2D array.
- Scans pairs of reachable points within a Manhattan distance of 20.
- For each pair, computes if they meet the "saved time" criteria to increment the answer.

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 21 Solution

Calculates the total length of movement sequences required for multiple directional robots to type codes on a numeric keypad. 
The solution uses caching to optimize repeated calculations for many robots.

Input
- Input file: `input.txt`
- Format: Each line contains a code string composed of keypad characters.

How it works
- Initializes a numeric keypad and a list of directional robot keypads.
- For each code line, computes the movement sequence required to type the code using all robots recursively.
- Caches movement lengths for robot positions and moves to avoid redundant calculations.
- Sums the total length of all required sequences multiplied by a numeric factor derived from the code.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 22 Solution

This solution processes a list of secret numbers by applying a deterministic pseudo-random function. 
It then tracks 4-step change sequences and aggregates scores (called "prices") for each unique sequence. The result is the highest score among all tracked sequences.

Input
- Input file: `input.txt`
- Format: Each line contains a single integer representing a secret value.

How it works
- Custom Parsing:
Converts each line to a long using a manual parser for performance.
- Transformations:
Applies a 3-step pseudo-random transformation using bitwise operations and pruning to a 24-bit space.
- Sequence Tracking:
Tracks deltas (differences in the last digit) after each transformation.
After collecting 4 deltas, encodes them as a base-18 number to form a unique sequence ID.
Each sequence contributes the price of the current secret (last digit) to a dictionary if it hasn’t been recorded yet for the current secret.
- Result:
The maximum aggregated value across all sequences is printed as the final answer.
-------------------------------------------------------------------------------------------------------------------------------------------------------------------
Day 25 Solution - Part 1 Only (Cannot access due to not completing *all* prior days)

Determines how many keys can fit into corresponding locks based on block-style ASCII art input. Each lock and key consists of a 5-column grid of # (filled) and . (empty) characters, spanning 7 lines. A key "fits" into a lock if, for each column, the combined height of filled cells does not exceed a total height limit of 7.

Input
- Input File: `input.txt`
- Format: Every 8 lines represent a single key or lock. Lines 1–7: a 5-character-wide ASCII grid using # and .. Line 8: a blank line or separator. If the first line of the block contains no . characters, it's treated as a lock; otherwise, it's treated as a key.

How it works
- Parsing:
Reads each 8-line block and counts the number of # characters per column (i.e. height profile).
Categorizes blocks into locks or keys based on the first line.
- Compatibility Check:
A key is compatible with a lock if, for all 5 columns, key[i] + lock[i] <= 7.
- Counting:
Iterates through all key-lock combinations and counts the number of valid pairings.
