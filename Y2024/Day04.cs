using System.Diagnostics;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Ceres Search", 2024, 04)]
public sealed class Day04 : IPuzzle
{
    private readonly char[][] _wordSearch;

    public Day04(TextReader reader)
    {
        _wordSearch = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        int sum = 0;

        for (int row = 0; row < _wordSearch.Length; row++)
        {
            for (int col = 0; col < _wordSearch[row].Length; col++)
            {
                char letter = _wordSearch[row][col];
                if (letter == 'X')
                {
                    sum += CountMatchesAtPosition(row, col);
                }
            }
        }

        return sum;
    }

    public PuzzleResult SolvePartTwo()
    {
        int sum = 0;

        for (int row = 0; row < _wordSearch.Length; row++)
        {
            for (int col = 0; col < _wordSearch[row].Length; col++)
            {
                if (IsMasInCrossShape(row, col))
                    sum++;
            }
        }

        return sum;
    }

    private int CountMatchesAtPosition(int row, int col)
    {
        return Enum.GetValues<Direction>()
            .Select(direction => GetWordAtDirection(row, col, direction))
            .Count(word => word == "XMAS");
    }

    private string GetWordAtDirection(int row, int col, Direction direction)
    {
        var chars = Enumerable.Range(0, 4)
            .Select(distance => GetLetterAtDirection(row, col, direction, distance));

        return new string(chars.ToArray());
    }

    private bool IsMasInCrossShape(int row, int col)
    {
        var center = GetCharAtPosition(row, col);
        if (center != 'A') return false;

        var nwLetter = GetLetterAtDirection(row, col, Direction.NorthWest);
        var neLetter = GetLetterAtDirection(row, col, Direction.NorthEast);
        var swLetter = GetLetterAtDirection(row, col, Direction.SouthWest);
        var seLetter = GetLetterAtDirection(row, col, Direction.SouthEast);

        return (nwLetter, seLetter) is ('M', 'S') or ('S', 'M')
            && (neLetter, swLetter) is ('M', 'S') or ('S', 'M');
    }

    private char GetLetterAtDirection(int row, int col, Direction direction, int distance = 1)
    {
        var pos = direction switch
        {
            Direction.NorthWest =>  (row: row - distance, col: col - distance),
            Direction.North =>      (row: row - distance, col: col),
            Direction.NorthEast =>  (row: row - distance, col: col + distance),
            Direction.East =>       (row: row, col: col + distance),
            Direction.SouthEast =>  (row: row + distance, col: col + distance),
            Direction.South =>      (row: row + distance, col: col),
            Direction.SouthWest =>  (row: row + distance, col: col - distance),
            Direction.West =>       (row: row, col: col - distance),
            _ => throw new UnreachableException(),
        };

        return GetCharAtPosition(pos.row, pos.col);
    }

    private char GetCharAtPosition(int row, int col)
    {
        if (row < 0 || row >= _wordSearch.Length) return '.';
        if (col < 0 || col >= _wordSearch[row].Length) return '.';

        return _wordSearch[row][col];
    }

    private enum Direction
    {
        NorthWest,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
    }
}
