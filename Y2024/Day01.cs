namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Historian Hysteria", 2024, 01)]
public sealed class Day01 : IPuzzle
{
    private readonly IEnumerable<int> _leftList;
    private readonly IEnumerable<int> _rightList;

    public Day01(TextReader reader)
    {
        var lists = from line in reader.ReadAllLines()
                    let pair = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    select new { Left = pair[0], Right = pair[1] };

        _leftList = lists.Select(x => int.Parse(x.Left));
        _rightList = lists.Select(x => int.Parse(x.Right));
    }

    public PuzzleResult SolvePartOne()
    {
        var leftList = _leftList.OrderBy(x => x);
        var rightList = _rightList.OrderBy(x => x);

        var count = leftList.Zip(rightList).Sum(x => Math.Abs(x.First - x.Second));
        return count;
    }

    public PuzzleResult SolvePartTwo()
    {
        var rightGroupedBy = _rightList.GroupBy(x => x);

        //var x = _leftList.GroupBy(x => x).Select(x => (x.Key, x.Count()));

        var count = 0;
        foreach (var left in _leftList)
        {
            var timesOnRight = rightGroupedBy.FirstOrDefault(x => x.Key == left)?.Count() ?? 0;
            count += left * timesOnRight;
        }

        return count;
    }
}
