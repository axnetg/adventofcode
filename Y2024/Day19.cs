using System.Text.RegularExpressions;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Linen Layout", 2024, 19)]
public sealed class Day19 : IPuzzle
{
    private readonly IEnumerable<string> _patterns;
    private readonly IEnumerable<string> _goalDesigns;

    public Day19(TextReader reader)
    {
        var lines = reader.ReadAllLines();

        _patterns = lines.First().Split(',', StringSplitOptions.TrimEntries).AsEnumerable();
        _goalDesigns = lines.Skip(1).Where(str => !string.IsNullOrEmpty(str));
    }

    public PuzzleResult SolvePartOne()
    {
        string pattern = string.Format(@"^(?:{0})+$", string.Join('|', _patterns));
        var regex = new Regex(pattern);

        return _goalDesigns.Count(regex.IsMatch);
    }

    public PuzzleResult SolvePartTwo()
    {
        Dictionary<string, long> memo = [];

        return _goalDesigns.Sum(design => CountCombinationsOf(design, memo));
    }

    private long CountCombinationsOf(string design, Dictionary<string, long> memo)
    {
        if (string.IsNullOrEmpty(design))
            return 1;

        if (memo.TryGetValue(design, out var combinations))
            return combinations;

        long count = 0;
        foreach (var pattern in _patterns)
        {
            if (design.StartsWith(pattern))
                count += CountCombinationsOf(design[pattern.Length..], memo);
        }

        memo.Add(design, count);
        return count;
    }
}
