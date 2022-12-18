using AdventOfCode.Core;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day07;

[Challenge("No Space Left On Device", 2022, 7)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var dirs = GetDirectories(input);
        return dirs.Values.Where(s => s <= 100000).Sum();
    }

    public object PartTwo(string input)
    {
        var dirs = GetDirectories(input);

        var unusedSpace = 70000000 - dirs.Values.Max();
        return dirs.Values.Where(s => unusedSpace + s >= 30000000).Min();
    }

    public Dictionary<string, long> GetDirectories(string input)
    {
        var path = new Stack<string>();
        var dirs = new Dictionary<string, long>();
        foreach (var line in input.Split("\n"))
        {
            if (line.StartsWith("$ cd .."))
            {
                path.Pop();
            }
            else if (line.StartsWith("$ cd"))
            {
                var dirName = line.Split(" ")[2];
                path.Push(Path.Combine(string.Join("", path), dirName));
            }
            else if (Regex.IsMatch(line, @"^\d"))   // starts with digit
            {
                var size = long.Parse(line.Split(" ")[0]);
                foreach (var currentDir in path)    // traverse all dirs and add the size of the current file
                {
                    dirs[currentDir] = dirs.GetValueOrDefault(currentDir) + size;
                }
            }
        }

        return dirs;
    }
}
