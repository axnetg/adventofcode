using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day02;

[Challenge("Rock Paper Scissors", 2022, 2)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        return input.Split("\n")
            .Sum(play =>
            {
                // A,X -> Rock (0)
                // B,Y -> Paper (1)
                // C,Z -> Scissors (2)

                int oponent = (int)(play[0]) - (int)'A';
                int self = (int)(play[2]) - (int)'X';
                return GetScore((Options)oponent, (Options)self);
            });
    }

    public object PartTwo(string input)
    {
        return input.Split("\n")
            .Sum(play =>
            {
                int oponent = (int)(play[0]) - (int)'A';

                // X -> losing = oponent - 1;   previous value in enum
                // Y -> draw = oponent;         same value in enum
                // Z -> win = oponent + 1;      next value in enum

                int self = ((int)(play[2]) - (int)'Y' + oponent + 3) % 3; // We add 3 so we can't get a negative modulo

                return GetScore((Options)oponent, (Options)self);
            });
    }

    private int GetScore(Options oponent, Options self)
    {
        int score = self switch
        {
            Options.Rock => 1,
            Options.Paper => 2,
            Options.Scissors => 3,
            _ => throw new NotSupportedException(),
        };

        if (oponent == self)  // draw
        {
            score += 3;
        }
        else if (GetWinningOption(oponent) == self)  // win
        {
            score += 6;
        }

        return score;
    }

    private Options GetWinningOption(Options option)
    {
        // The winning choice is the next one in the enum (using modulo so we can circle back to the start)
        return (Options) (((int)option + 1) % 3);
    }

    private enum Options
    {
        Rock,
        Paper,
        Scissors,
    }
}
