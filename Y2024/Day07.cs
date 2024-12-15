using System.Text.RegularExpressions;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Bridge Repair", 2024, 07)]
public sealed class Day07 : IPuzzle
{
    private readonly List<Equation> _equations = [];

    public Day07(TextReader reader)
    {
        Regex regex = new(@"(?<goal>\d+): (?<operands>.*$)");

        foreach (var line in reader.ReadAllLines())
        {
            Match match = regex.Match(line);
            if (match.Success)
            {
                var goal = long.Parse(match.Groups["goal"].Value);
                var operands = match.Groups["operands"].Value.Split(' ').Select(int.Parse).ToList();
                _equations.Add(new Equation(goal, operands));
            }
        }
    }

    public PuzzleResult SolvePartOne()
    {
        List<Operation> operators = [Add, Multiply];

        return _equations
            .Where(eq => GetCombinatorialResults(eq, operators).Any(result => eq.Goal == result))
            .Sum(eq => eq.Goal);
    }

    public PuzzleResult SolvePartTwo()
    {
        List<Operation> operators = [Add, Multiply, Concat];

        return _equations
            .Where(eq => GetCombinatorialResults(eq, operators).Any(result => eq.Goal == result))
            .Sum(eq => eq.Goal);
    }

    private IEnumerable<long> GetCombinatorialResults(Equation equation, List<Operation> operations)
    {
        var operands = equation.Operands;

        IEnumerable<long> combinatorial = [operands.FirstOrDefault()];

        foreach (var operand in operands.Skip(1))
        {
            combinatorial = combinatorial.SelectMany(item => operations.Select(f => f.Invoke(item, operand)));
        }

        return combinatorial;
    }

    private static long Add(long a, long b) => a + b;

    private static long Multiply(long a, long b) => a * b;

    private static long Concat(long a, long b) => long.Parse(string.Format("{0}{1}", a, b));

    private delegate long Operation(long a, long b);

    private sealed record Equation(long Goal, List<int> Operands);
}
