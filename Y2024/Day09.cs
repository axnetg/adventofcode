namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Disk Fragmenter", 2024, 09)]
public sealed class Day09 : IPuzzle
{
    private readonly IEnumerable<int> _diskmap;

    public Day09(TextReader reader)
    {
        _diskmap = reader.ReadToEnd().Trim().SelectMany(GenerateBlock);
    }

    public PuzzleResult SolvePartOne()
    {
        var array = _diskmap.ToArray();

        int left = 0;
        int right = array.Length - 1;

        while (true)
        {
            while (array[left] != -1) left++;
            while (array[right] == -1) right--;

            if (left >= right) break;

            (array[left], array[right]) = (array[right], array[left]);
            left++;
            right--;
        }

        //return string.Join(' ', array);
        return array
            .TakeWhile(x => x != -1)
            .Select((x, idx) => (long)(idx * x))
            .Sum();
    }

    public PuzzleResult SolvePartTwo()
    {
        var array = _diskmap.ToArray();

        int right = array.Length - 1;

        while (true)
        {
            int left = Array.IndexOf(array, -1);
            if (left >= right) break;

            while (array[right] == -1) right--;

            int id = array[right];
            int size = 1;

            // calculates size of contiguous block files and resets right index
            while (array[--right] == id) size++;
            right += size;

            while (true)
            {
                while (left < right && array[left] != -1) left++;
                if (left >= right)
                {
                    // no contiguous space of memory with enough size found on all disk -- don't do anything
                    right -= size;
                    size = 0;
                    break;
                }

                int gap = 1;
                while (left < right && array[++left] == -1) gap++;
                if (gap >= size)
                {
                    // space of memory big enough found -- reset left index to start moving blocks
                    left -= gap;
                    break;
                }
            }

            while (size > 0)
            {
                // moves blocks `size` times between space blocks (left) and file blocks (right)
                (array[left], array[right]) = (array[right], array[left]);
                left++;
                right--;
                size--;
            }
        }

        return array
            .Select((x, idx) => (position: idx, value: x))
            .Where(x => x.value != -1)
            .Sum(x => (long)(x.position * x.value));
    }

    private IEnumerable<int> GenerateBlock(char c, int idx)
    {
        var count = (int)c - '0';
        return idx % 2 == 0
            ? Enumerable.Range(0, count).Select(_ => idx / 2)
            : Enumerable.Range(0, count).Select(_ => -1);
    }
}
