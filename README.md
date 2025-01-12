# Axnetg's Advent of Code

This repository contains my implementations of the challenges from [Advent of Code](https://adventofcode.com/), an annual series of programming puzzles designed to test problem-solving skills and creativity.

I like to use LINQ to simplify looping logic and records to make the code more concise. While my solutions might not always be the cleanest or most polished, my focus is on correctness and brevity over maintainability, reflecting my current C# skills.

Each challenge solution is implemented in its own class, with one method for part one and another for part two of the puzzle.

## About the Project

This is a console application written in C#. Its sole purpose is to print the solutions to Advent of Code challenges.

## Stars Earned in 2024

I earned **45 stars** in Advent of Code 2024. Each star represents completing one part of a challenge (up to two stars per day).

## Usage

Run the application from the command line, specifying the year and day of the challenge you want to solve.

```
Description:
  Advent of Code Resolver

Usage:
  Axnetg.AdventOfCode [options]

Options:
  -y, --year <year>  The year of the puzzle
  -d, --day <day>    The day of the puzzle
  --version          Show version information
  -?, -h, --help     Show help and usage information
```

To solve the challenge for December 5, 2024:

```bash
dotnet run -- -y 2024 -d 5
```

## Puzzle Inputs

Puzzle input files should be placed under the top-level `Inputs/` folder.
The input file for each challenge must follow the naming convention:
`{year}{day:00}.in`

Example input for December 5, 2024: `Inputs/202405.in`

## Acknowledgments

- [Advent of Code](https://adventofcode.com/) by Eric Wastl for the fun and challenging puzzles.
- [Zoran Horvat's Advent of Code 2024 Repository](https://github.com/zoran-horvat/advent-of-code-2024), which provided inspiration for some of my solutions.
