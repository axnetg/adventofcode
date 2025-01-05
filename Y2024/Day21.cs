using System.Collections.ObjectModel;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Keypad Conundrum", 2024, 21)]
public sealed class Day21 : IPuzzle
{
    private readonly Dictionary<MemoizationKey, long> _memo = [];
    private readonly string[] _inputCodes;

    public Day21(TextReader reader)
    {
        _inputCodes = reader.ReadAllLines();
    }

    public PuzzleResult SolvePartOne()
    {
        const int robots = 2;

        return _inputCodes.Sum(code => GetCodeComplexity(code, robots));
    }

    public PuzzleResult SolvePartTwo()
    {
        const int robots = 25;

        return _inputCodes.Sum(code => GetCodeComplexity(code, robots));
    }

    private long GetCodeComplexity(string code, int depth)
    {
        long length = GetShortestSequenceLength(code, depth);
        int codeNumeric = int.Parse(code.TrimEnd('A'));

        return length * codeNumeric;
    }

    private long GetShortestSequenceLength(string code, int depth)
    {
        long len = 0;
        for (int i = 0; i < code.Length; i++)
        {
            // recursively compute sequence length between the previous key (or 'A' if first) and the actual key
            len += GetShortestSequenceLength(i is 0 ? 'A' : code[i - 1], code[i], depth, NumericKeypad.Instance);
        }

        return len;
    }

    private long GetShortestSequenceLength(char from, char to, int depth, KeypadBase kp)
    {
        // return value if sequence was already computed
        if (_memo.TryGetValue(new(from, to, depth), out var value))
            return value;

        char[] movs = kp.GetMovements(from, to);

        // base case: number of movements
        if (depth is 0)
            return movs.Length;

        long len = 0;
        for (int i = 0; i < movs.Length; i++)
        {
            // recursively compute sequence length between previous key ('A' if first) and the actual key for the new keypad
            len += GetShortestSequenceLength(i is 0 ? 'A' : movs[i - 1], movs[i], depth - 1, DirectionalKeypad.Instance);
        }

        _memo.Add(new(from, to, depth), len);
        return len;
    }

    private abstract class KeypadBase
    {
        private readonly ReadOnlyDictionary<char, Position> _keypad;

        protected KeypadBase(ReadOnlyDictionary<char, Position> keypad)
        {
            _keypad = keypad;
        }

        public char[] GetMovements(char from, char to)
        {
            var p1 = _keypad[from];
            var p2 = _keypad[to];
            var gap = _keypad[' '];

            var vector = (X: p2.X - p1.X, Y: p2.Y - p1.Y);

            var horizontalMovs = Enumerable.Repeat(vector.X < 0 ? '<' : '>', Math.Abs(vector.X));
            var verticalMovs = Enumerable.Repeat(vector.Y < 0 ? '^' : 'v', Math.Abs(vector.Y));

            if (gap.X == p1.X + vector.X && gap.Y == p1.Y)
            {
                // if moving horizontally we end in the gap, then do vertical movement first
                return [.. verticalMovs, .. horizontalMovs, 'A'];
            }
            else if (gap.X == p1.X && gap.Y == p1.Y + vector.Y)
            {
                // if moving vertically we end in the gap, then do horizontal movement first
                return [.. horizontalMovs, .. verticalMovs, 'A'];
            }
            else if (vector.X < 0)
            {
                // if going left (and not into the gap), then do horizontal movement first
                return [.. horizontalMovs, .. verticalMovs, 'A'];
            }
            else
            {
                // otherwise, do vertical movement first
                return [.. verticalMovs, .. horizontalMovs, 'A'];
            }
        }
    }

    private class NumericKeypad : KeypadBase
    {
        private static readonly NumericKeypad _singleton = new();

        public static NumericKeypad Instance => _singleton;

        private static Dictionary<char, Position> Keypad => new()
        {
            ['7'] = new(0, 0),
            ['8'] = new(1, 0),
            ['9'] = new(2, 0),
            ['4'] = new(0, 1),
            ['5'] = new(1, 1),
            ['6'] = new(2, 1),
            ['1'] = new(0, 2),
            ['2'] = new(1, 2),
            ['3'] = new(2, 2),
            [' '] = new(0, 3),
            ['0'] = new(1, 3),
            ['A'] = new(2, 3),
        };

        private NumericKeypad() : base(Keypad.AsReadOnly()) { }
    }

    private class DirectionalKeypad : KeypadBase
    {
        private static readonly DirectionalKeypad _singleton = new();

        public static DirectionalKeypad Instance => _singleton;

        private static Dictionary<char, Position> Keypad => new()
        {
            [' '] = new(0, 0),
            ['^'] = new(1, 0),
            ['A'] = new(2, 0),
            ['<'] = new(0, 1),
            ['v'] = new(1, 1),
            ['>'] = new(2, 1),
        };

        private DirectionalKeypad() : base(Keypad.AsReadOnly()) { }
    }

    private sealed record Position(int X, int Y);

    private sealed record MemoizationKey(char From, char To, int Depth);
}
