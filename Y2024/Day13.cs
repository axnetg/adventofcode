using System.Text.RegularExpressions;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Claw Contraption", 2024, 13)]
public sealed partial class Day13 : IPuzzle
{
    private readonly IEnumerable<Equation> _equations;

    public Day13(TextReader reader)
    {
        _equations = reader.ReadAllLines().Chunk(4).Select(l => ParseEquation(string.Join(' ', l)));
    }

    public PuzzleResult SolvePartOne()
    {
        return _equations.Sum(CountTokens);
    }

    public PuzzleResult SolvePartTwo()
    {
        const long offset = 10_000_000_000_000;

        return _equations
            .Select(x => x with { Px = x.Px + offset, Py = x.Py + offset })
            .Sum(CountTokens);
    }

    private Equation ParseEquation(string str)
    {
        if (RegexEquationFormat().Match(str) is { Success: true } match)
            return new Equation(Parse("ax"), Parse("ay"), Parse("bx"), Parse("by"), Parse("px"), Parse("py"));

        throw new ArgumentException($"Wrong equation format: {str}");

        long Parse(string groupname) => long.Parse(match.Groups[groupname].ValueSpan);
    }

    private long CountTokens(Equation equation)
    {
        var result = SolveEquation(equation);

        long sx = result.A * equation.Ax + result.B * equation.Bx;
        long sy = result.A * equation.Ay + result.B * equation.By;

        if (sx != equation.Px || sy != equation.Py)
            return 0;

        return 3 * result.A + 1 * result.B;
    }

    private (long A, long B) SolveEquation(Equation equation)
    {
        (long ax, long ay, long bx, long by, long px, long py) = equation;

        var a = (px * by - py * bx) / (ax * by - ay * bx);
        var b = (ax * py - ay * px) / (ax * by - ay * bx);

        return (A: a, B: b);
    }

    public sealed record Equation(long Ax, long Ay, long Bx, long By, long Px, long Py);

    [GeneratedRegex(@"Button A: X\+(?<ax>\d+), Y\+(?<ay>\d+) Button B: X\+(?<bx>\d+), Y\+(?<by>\d+) Prize: X=(?<px>\d+), Y=(?<py>\d+)")]
    private static partial Regex RegexEquationFormat();
}
