namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Plutonian Pebbles", 2024, 11)]
public sealed class Day11 : IPuzzle
{
    private readonly IEnumerable<Stone> _stones;
    private readonly Dictionary<(long, int), long> _memo = [];

    public Day11(TextReader reader)
    {
        _stones = reader.ReadToEnd().Split(' ').Select(x => new Stone(long.Parse(x)));
    }

    public PuzzleResult SolvePartOne()
    {
        const int times = 25;

        return _stones.Sum(s => GetStoneScore(s.Mark, times));
    }

    public PuzzleResult SolvePartTwo()
    {
        const int times = 75;

        return _stones.Sum(s => GetStoneScore(s.Mark, times));
    }

    private long GetStoneScore(long mark, int blinks)
    {
        // Return memoized score if already computed
        if (_memo.TryGetValue(key: (mark, blinks), out long memoizedScore))
        {
            return memoizedScore;
        }

        // Base case
        if (blinks == 0) return 1;
        blinks--;

        // If the stone is engraved with the number 0,
        // it is replaced by a stone engraved with the number 1.
        if (mark is 0)
        {
            var result = GetStoneScore(1, blinks);
            _memo.TryAdd(key: (1, blinks), value: result);
            return result;
        }

        // If the stone is engraved with a number that has an even number of digits,
        // it is replaced by two stones.
        if (Math.Floor(Math.Log10(mark) + 1) % 2 == 0)
        {
            var (left, right) = SplitNumberInHalf(mark);
            var leftScore = GetStoneScore(left, blinks);
            var rightScore = GetStoneScore(right, blinks);
            _memo.TryAdd(key: (left, blinks), value: leftScore);
            _memo.TryAdd(key: (right, blinks), value: rightScore);
            return leftScore + rightScore;
        }

        // If none of the other rules apply, the stone is replaced by a new stone;
        // the old stone's number multiplied by 2024 is engraved.
        var score = GetStoneScore(mark * 2024, blinks);
        _memo.TryAdd(key: (mark * 2024, blinks), value: score);
        return score;
    }

    private static (long left, long right) SplitNumberInHalf(long number)
    {
        var str = number.ToString();
        var halfLen = str.Length / 2;

        var left = long.Parse(str[..halfLen]);
        var right = long.Parse(str[^halfLen..]);

        return (left, right);
    }

    private sealed record Stone(long Mark);
}
