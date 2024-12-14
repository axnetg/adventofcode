using System.Diagnostics;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Guard Gallivant", 2024, 06)]
public sealed class Day06 : IPuzzle
{
    private readonly char[][] _map;

    public Day06(TextReader reader)
    {
        _map = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        return GetTravel(GetObstacleCoordinates()).Path.DistinctBy(x => x.Coordinate).Count();
    }

    public PuzzleResult SolvePartTwo()
    {
        var obstacles = GetObstacleCoordinates();
        var originalPath = GetTravel(obstacles).Path;
        var possibleObstacles = originalPath.Select(x => x.Coordinate).Distinct().Except([GetStartingPosition()]);

        return possibleObstacles
            .Select(newObstacle => GetTravel([.. obstacles, newObstacle]))
            .Count(t => t.IsStuckInLoop);
    }

    private TravelResult GetTravel(HashSet<Coordinate> obstacles)
    {
        HashSet<PathRecord> visitedPositions = [];

        Coordinate start = GetStartingPosition();
        Direction direction = Direction.Up;

        PathRecord path = new(start, direction);

        while (!IsOutOfBounds(path.Coordinate) && visitedPositions.Add(path))
        {
            var forward = GoForward(path.Coordinate, path.Direction);
            while (obstacles.Contains(forward))
            {
                direction = TurnRight(direction);
                forward = GoForward(path.Coordinate, direction);
            }

            path = new PathRecord(forward, direction);
        }

        return new TravelResult(
            Path: visitedPositions,
            IsStuckInLoop: visitedPositions.Contains(path)
        );
    }

    private HashSet<Coordinate> GetObstacleCoordinates()
    {
        var flat = _map.SelectMany(x => x).ToArray();
        return Enumerable
            .Range(0, flat.Length)
            .Where(idx => flat[idx] is '#')
            .Select(idx => new Coordinate(idx / _map.Length, idx % _map.Length))
            .ToHashSet();
    }

    private Coordinate GetStartingPosition()
    {
        var flat = _map.SelectMany(x => x).ToArray();
        var idx = Enumerable.Range(0, flat.Length).First(idx => flat[idx] is '^');
        return new Coordinate(idx / _map.Length, idx % _map.Length);
    }

    private bool IsOutOfBounds(Coordinate coord)
    {
        return coord.Row < 0
            || coord.Col < 0
            || coord.Row >= _map.Length
            || coord.Col >= _map.Length;
    }

    private Direction TurnRight(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new UnreachableException(),
        };
    }

    private Coordinate GoForward(Coordinate coord, Direction direction)
    {
        return direction switch
        {
            Direction.Up => coord with { Row = coord.Row - 1 },
            Direction.Right => coord with { Col = coord.Col + 1 },
            Direction.Down => coord with { Row = coord.Row + 1 },
            Direction.Left => coord with { Col = coord.Col - 1 },
            _ => throw new UnreachableException(),
        };
    }

    private sealed record Coordinate(int Row, int Col);

    private sealed record PathRecord(Coordinate Coordinate, Direction Direction);

    private sealed record TravelResult(HashSet<PathRecord> Path, bool IsStuckInLoop);

    private enum Direction { Left, Right, Up, Down }
}
