using System.Diagnostics;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Hoof It", 2024, 10)]
public sealed class Day10 : IPuzzle
{
    private readonly int[][] _map;

    public Day10(TextReader reader)
    {
        _map = reader.ReadAllLines().Select(x => x.Select(c => (int)c - '0').ToArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        return GetTrailheads()
            .Select(GetVisitedEndOfTrailPositions)
            .Sum(coords => coords.Distinct().Count());
    }

    public PuzzleResult SolvePartTwo()
    {
        return GetTrailheads()
            .Select(GetVisitedEndOfTrailPositions)
            .Sum(coords => coords.Count());
    }

    private IEnumerable<Step> GetTrailheads()
    {
        var flat = _map.SelectMany(x => x).ToArray();
        return Enumerable
            .Range(0, flat.Length)
            .Where(idx => flat[idx] is 0)
            .Select(ParseStep);

        Step ParseStep(int idx)
        {
            var coord = new Coordinate(idx / _map.Length, idx % _map.Length);
            return new Step(Height: 0, Position: coord);
        }
    }

    private IEnumerable<Coordinate> GetVisitedEndOfTrailPositions(Step trailHeadStep)
    {
        Debug.Assert(trailHeadStep.Height == 0);

        var visitedEndOfTrailPositions = new List<Coordinate>();

        TraverseHikeTrailRecursive(visitedEndOfTrailPositions, trailHeadStep);

        return visitedEndOfTrailPositions;
    }

    private void TraverseHikeTrailRecursive(List<Coordinate> visitedEndOfTrailPositions, Step currentStep)
    {
        if (currentStep.Height == 9)
            visitedEndOfTrailPositions.Add(currentStep.Position);

        foreach (Step nextStep in GetNextSteps(currentStep))
        {
            TraverseHikeTrailRecursive(visitedEndOfTrailPositions, nextStep);
        }
    }

    private IEnumerable<Step> GetNextSteps(Step currentStep)
    {
        if (currentStep.Height == 9) return [];

        var coord = currentStep.Position;
        int nextHeight = currentStep.Height + 1;

        var up = coord with { Row = coord.Row - 1 };
        var down = coord with { Row = coord.Row + 1 };
        var left = coord with { Col = coord.Col - 1 };
        var right = coord with { Col = coord.Col + 1 };

        Coordinate[] directions = [up, down, left, right];
        return directions
            .Where(c => GetHeightAtPosition(c.Row, c.Col) == nextHeight)
            .Select(c => new Step(Height: nextHeight, Position: c));
    }

    private int GetHeightAtPosition(int row, int col)
    {
        if (row < 0 || row >= _map.Length) return -1;
        if (col < 0 || col >= _map[row].Length) return -1;

        return _map[row][col];
    }

    private sealed record Step(int Height, Coordinate Position);

    private sealed record Coordinate(int Row, int Col);
}
