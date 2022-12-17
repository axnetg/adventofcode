using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day05;

[Challenge("Supply Stacks", 2022, 5)]
public class Solution : ISolution
{
    public object PartOne(string input) => Rearrange(input, CrateMover9000);

    public object PartTwo(string input) => Rearrange(input, CrateMover9001);

    private string Rearrange(string input, Action<List<Stack<char>>, IEnumerable<Instruction>> action)
    {
        var firstSection = input.Split("\n\n")[0];
        var secondSection = input.Split("\n\n")[1];

        var crates = GetCrates(firstSection);
        var instructions = GetInstructions(secondSection);

        action.Invoke(crates, instructions);

        return string.Join(string.Empty, crates.Select(c => c.Peek()));
    }

    private void CrateMover9000(List<Stack<char>> crates, IEnumerable<Instruction> instructions)
    {
        foreach (var instr in instructions)
        {
            for (int i = 0; i < instr.Count; i++)
            {
                char c = crates[instr.From - 1].Pop();
                crates[instr.To - 1].Push(c);
            }
        }
    }

    private void CrateMover9001(List<Stack<char>> crates, IEnumerable<Instruction> instructions)
    {
        foreach (var instr in instructions)
        {
            Stack<char> aux = new();
            for (int i = 0; i < instr.Count; i++)
            {
                char c = crates[instr.From - 1].Pop();
                aux.Push(c);
            }

            while (aux.Count > 0)
            {
                char c = aux.Pop();
                crates[instr.To - 1].Push(c);
            }
        }
    }

    private List<Stack<char>> GetCrates(string section)
    {
        var startingCrates = section.Split("\n").Reverse();
        var lineLength = startingCrates.First().Length;

        List<Stack<char>> crates = Enumerable.Range(0, lineLength / 4 + 1).Select(_ => new Stack<char>()).ToList();

        foreach (var line in startingCrates.Skip(1)) // First line indicates the crane numbers
        {
            for (int i = 1; i < lineLength; i += 4) // We can find the crane every 4 chars (starting at 1)
            {
                char c = line[i];
                if (!char.IsWhiteSpace(c))
                {
                    crates[i / 4].Push(c);
                }
            }
        }

        return crates;
    }

    private IEnumerable<Instruction> GetInstructions(string section)
    {
        foreach (var line in section.Split("\n"))
        {
            // ['move', x, 'from', y, 'to', 'z']
            var split = line.Split(" ");

            yield return new Instruction(int.Parse(split[1]), int.Parse(split[3]), int.Parse(split[5]));
        }
    }

    private record Instruction(int Count, int From, int To);
}
