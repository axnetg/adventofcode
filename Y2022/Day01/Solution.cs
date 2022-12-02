using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day01;

[Challenge("Calorie Counting", 2022, 1)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        /* How many total Calories is carrying the Elf with the most Calories? */
        return CaloriesPerElf(input).First();
    }

    public object PartTwo(string input)
    {
        /* How many Calories are the top three Elves carrying in total? */
        return CaloriesPerElf(input).Take(3).Sum();
    }

    private IEnumerable<int> CaloriesPerElf(string lines)
    {
        return from elf in lines.Split("\n\n")
               let calories = elf.Split("\n").Select(x => int.Parse(x)).Sum()
               orderby calories descending
               select calories;
    }
}
