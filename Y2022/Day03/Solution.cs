using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day03;

[Challenge("Rucksack Reorganization", 2022, 3)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        /* Find the item type that appears in both compartments (half a line) of each rucksack */
        var sharedItems = input.Split("\n")
            .Select(line => line.Chunk(line.Length / 2))
            .Select(x => IntersectAll(x).First());

        return sharedItems.Sum(x => GetPriority(x));
    }

    public object PartTwo(string input)
    {
        /* Find the item type that is common between all three Elves' ruckasks (each 3 lines) for each group */
        var badges = input.Split("\n")
            .Chunk(3)
            .Select(x => IntersectAll(x).First());

        return badges.Sum(x => GetPriority(x));
    }

    private IEnumerable<char> IntersectAll(IEnumerable<IEnumerable<char>> values)
    {
        return values.Aggregate((s1, s2) => s1.Intersect(s2).ToArray());
    }

    private int GetPriority(char c)
    {
        if (char.IsLower(c)) return (int)c - (int)'a' + 1;
        else return (int)c - (int)'A' + 1 + 26;
    }
}
