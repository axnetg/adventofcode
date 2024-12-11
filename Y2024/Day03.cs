using System.Text.RegularExpressions;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Mull It Over", 2024, 03)]
public sealed partial class Day03 : IPuzzle
{
    private readonly string _commandText;

    public Day03(TextReader reader)
    {
        _commandText = reader.ReadToEnd().Replace("\n", "");
    }

    public PuzzleResult SolvePartOne()
    {
        return GetOperationsFromCommand(_commandText).Sum(x => x.Result);
    }

    public PuzzleResult SolvePartTwo()
    {
        var cleanCommand = DoDontSplitRegex().Replace(_commandText, string.Empty);
        cleanCommand = DontUntilEndRegex().Replace(cleanCommand, string.Empty);

        return GetOperationsFromCommand(cleanCommand).Sum(x => x.Result);
    }

    private IEnumerable<Mul> GetOperationsFromCommand(string command)
    {
        Regex regex = MulOperationRegex();
        return regex.Matches(command).Select(ParseMulText);

        static Mul ParseMulText(Match m)
        {
            int left = int.Parse(m.Groups[1].Value);
            int right = int.Parse(m.Groups[2].Value);
            return new Mul(left, right);
        }
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulOperationRegex();

    [GeneratedRegex(@"don't\(\).*?do\(\)")]
    private static partial Regex DoDontSplitRegex();

    [GeneratedRegex(@"don't\(\).*$")]
    private static partial Regex DontUntilEndRegex();

    private sealed record Mul(int Left, int Right)
    {
        public int Result => Left * Right;
    };
}
