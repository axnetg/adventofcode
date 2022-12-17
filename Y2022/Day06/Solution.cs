using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day06;

[Challenge("Tuning Trouble", 2022, 6)]
public class Solution : ISolution
{
    public object PartOne(string input) => GetMarkerIndex(input, 4);

    public object PartTwo(string input) => GetMarkerIndex(input, 14);

    private int GetMarkerIndex(string data, int packetSize)
    {
        for (int i = 0; i < data.Length - packetSize; i++)
        {
            string chars = data.Substring(i, packetSize);
            if (chars.Distinct().Count() == packetSize)
            {
                return i + packetSize;
            }
        }

        throw new NotSupportedException();
    }
}
