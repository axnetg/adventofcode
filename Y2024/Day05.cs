namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Print Queue", 2024, 05)]
public sealed class Day05 : IPuzzle
{
    private readonly HashSet<(int, int)> _rules = [];
    private readonly List<List<int>> _pages = [];

    public Day05(TextReader reader)
    {
        while (reader.ReadLine() is string line && !string.IsNullOrWhiteSpace(line))
        {
            var rule = line.Split('|');
            _rules.Add((int.Parse(rule[0]), int.Parse(rule[1])));
        }

        while (reader.ReadLine() is string line)
        {
            var page = line.Split(',');
            _pages.Add(page.Select(int.Parse).ToList());
        }
    }

    public PuzzleResult SolvePartOne()
    {
        return _pages.Where(IsValidPage).Sum(MiddleItem);
    }

    public PuzzleResult SolvePartTwo()
    {
        return _pages.Where(p => !IsValidPage(p)).Select(ReOrder).Sum(MiddleItem);
    }

    private bool IsValidPage(List<int> pageNumbers)
    {
        var isValid = true;
        var list = pageNumbers;

        while (list is [int actual, .. List<int> rest] && isValid)
        {
            isValid = rest.All(x => IsCorrectPosition(actual, x));
            list = rest;
        }

        return isValid;
    }

    private bool IsCorrectPosition(int before, int after)
    {
        return !_rules.Contains((after, before));
    }

    private List<int> ReOrder(List<int> pageNumbers)
    {
        List<int> orderedPages = [];

        while (orderedPages.Count != pageNumbers.Count)
        {
            var rest = pageNumbers.Except(orderedPages);
            var page = rest.First(actual => rest.All(x => IsCorrectPosition(actual, x)));
            orderedPages.Add(page);
        }

        return orderedPages;
    }

    private int MiddleItem(List<int> list)
    {
        return list[list.Count / 2];
    }
}
