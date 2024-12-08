namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Red-Nosed Reports", 2024, 02)]
public sealed class Day02 : IPuzzle
{
    private const int MinStep = 1;
    private const int MaxStep = 3;

    private readonly IEnumerable<Report> _reports;

    public Day02(TextReader reader)
    {
        _reports = reader.ReadAllLines().Select(x => new Report(x.Split(' ').Select(int.Parse).ToArray()));
    }

    public PuzzleResult SolvePartOne()
    {
        return _reports.Count(IsSafe);
    }

    public PuzzleResult SolvePartTwo()
    {
        return _reports.Count(IsSafeWithSkip);
    }

    private bool IsSafe(Report report)
    {
        return IsSafeLevel(report.Levels);
    }

    private bool IsSafeWithSkip(Report report)
    {
        return Enumerable
            .Range(0, report.Levels.Length)
            .Select(i => report.Levels.Splice(i, 1).ToArray())
            .Any(IsSafeLevel);
    }

    private bool IsSafeLevel(int[] levels)
    {
        if (levels.Length <= 1) return true;

        bool isSafe = true;

        int idx = 1;
        int sign = int.Sign(levels[0] - levels[1]);

        while (isSafe && idx < levels.Length)
        {
            int a = levels[idx - 1];
            int b = levels[idx];

            // The levels are either all increasing or all decreasing
            int diff = a - b;
            if (int.Sign(diff) != sign) isSafe = false;

            // Any two adjacent levels differ by at least one and at most three
            int step = Math.Abs(diff);
            if (step < MinStep || step > MaxStep) isSafe = false;

            idx++;
        }

        return isSafe;
    }

    private sealed record Report(int[] Levels);
}
