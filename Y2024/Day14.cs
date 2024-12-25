using System.Text;
using System.Text.RegularExpressions;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Restroom Redoubt", 2024, 14)]
public sealed partial class Day14 : IPuzzle
{
    private readonly Vector _mapRoom = new(101, 103);
    private readonly IEnumerable<Robot> _robots;

    public Day14(TextReader reader)
    {
        _robots = reader.ReadAllLines().Select(ParseRobot);
    }

    public PuzzleResult SolvePartOne()
    {
        const int seconds = 100;
        var positions = _robots.Select(r => GetPositionAfterSeconds(r, seconds));
        return GetScore(positions);
    }

    public PuzzleResult SolvePartTwo()
    {
        int max = _mapRoom.X * _mapRoom.Y;

        int seconds = Enumerable
            .Range(0, max)
            .Select(seconds => (seconds, positions: _robots.Select(r => GetPositionAfterSeconds(r, seconds))))
            .Select(x => (x.seconds, score: GetScore(x.positions)))
            .MinBy(x => x.score)
            .seconds;

        return seconds + "\n" + GetStringRepresentation(seconds);
    }

    private Robot ParseRobot(string str)
    {
        if (RegexRobotFormat().Match(str) is { Success: true } match)
            return new Robot(
                Position: new Vector(Parse("px"), Parse("py")),
                Velocity: new Vector(Parse("vx"), Parse("vy")));

        throw new ArgumentException($"Wrong robot format: {str}");

        int Parse(string groupname) => int.Parse(match.Groups[groupname].ValueSpan);
    }

    private Vector GetPositionAfterSeconds(Robot robot, int seconds)
    {
        var distance = (X: robot.Velocity.X * seconds, Y: robot.Velocity.Y * seconds);

        var positionX = Modulo(robot.Position.X + distance.X, _mapRoom.X);
        var positionY = Modulo(robot.Position.Y + distance.Y, _mapRoom.Y);

        return new Vector(positionX, positionY);
    }

    private int GetScore(IEnumerable<Vector> positions)
    {
        return positions
            .GroupBy(GetQuadrant)
            .Where(x => x.Key != 0)
            .Aggregate(1, (acc, group) => acc * group.Count());
    }

    private int GetQuadrant(Vector position)
    {
        int middleX = _mapRoom.X / 2;
        int middleY = _mapRoom.Y / 2;

        if (position.X < middleX && position.Y < middleY)
            return 1;
        if (position.X > middleX && position.Y < middleY)
            return 2;
        if (position.X < middleX && position.Y > middleY)
            return 3;
        if (position.X > middleX && position.Y > middleY)
            return 4;

        return 0;
    }

    private string GetStringRepresentation(int seconds)
    {
        var positions = _robots
            .Select(r => GetPositionAfterSeconds(r, seconds))
            .Select(p => (p.X, p.Y))
            .ToHashSet();

        StringBuilder sb = new();
        for (int y = 0; y < _mapRoom.Y; y++)
        {
            for (int x = 0; x < _mapRoom.X; x++)
            {
                char c = positions.Contains((x, y)) ? '#' : '.';
                sb.Append(c);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    private static int Modulo(int x, int m) => ((x % m) + m) % m;

    private sealed record Robot(Vector Position, Vector Velocity);

    private sealed record Vector(int X, int Y);

    [GeneratedRegex(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)")]
    private static partial Regex RegexRobotFormat();
}
