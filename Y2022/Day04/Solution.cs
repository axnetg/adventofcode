using AdventOfCode.Core;
using System.Diagnostics;

namespace AdventOfCode.Y2022.Day04;

[Challenge("Camp Cleanup", 2022, 4)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        /* In how many pairs does one range fully contain the other? */
        return GetCleaningAssignments(input)
            .Count(c => c[0].All(elem => c[1].Contains(elem)) || c[1].All(elem => c[0].Contains(elem)));
    }

    public object PartTwo(string input)
    {
        /* In how many pairs do the ranges overlap? */
        return GetCleaningAssignments(input)
            .Count(c => c[0].Intersect(c[1]).Any());
    }

    private IEnumerable<int[][]> GetCleaningAssignments(string input)
    {
        return input.Split("\n")
            .Select(pair => pair.Split(","))
            .Select(cleanup => cleanup.Select(c =>
                {
                    var roomRanges = c.Split("-").Select(r => int.Parse(r)).ToArray();
                    return SequenceRange(roomRanges[0], roomRanges[1]).ToArray();
                })
                .ToArray()
            );
    }

    private IEnumerable<int> SequenceRange(int start, int end)
    {
        Debug.Assert(start <= end);
        return Enumerable.Range(start, end - start + 1);
    }
}
