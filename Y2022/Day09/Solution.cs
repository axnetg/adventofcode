using AdventOfCode.Core;

namespace AdventOfCode.Y2022.Day09;

[Challenge("Rope Bridge", 2022, 9)]
public class Solution : ISolution
{
    public object PartOne(string input)
    {
        var headMovements = GetHeadMovements(input);

        return GetKnotMovements(headMovements).Distinct().Count();
    }

    public object PartTwo(string input)
    {
        var movements = GetHeadMovements(input);

        for (int i = 0; i < 9; i++)
        {
            movements = GetKnotMovements(movements);
        }

        return movements.Distinct().Count();
    }

    private IEnumerable<Point> GetHeadMovements(string input)
    {
        Point head = new Point(0, 0);

        List<Point> points = new() { head };

        foreach (var line in input.Split("\n"))
        {
            char direction = line[0];
            int steps = int.Parse(line.Split(" ")[1]);

            for (int i = 0; i < steps; i++)
            {
                head = MovePoint(head, direction);

                points.Add(head);
            }
        }

        return points;
    }

    private IEnumerable<Point> GetKnotMovements(IEnumerable<Point> followingKnotMovements)
    {
        Point knot = new Point(0, 0);

        List<Point> points = new() { knot };

        foreach (Point next in followingKnotMovements.Skip(1))
        {
            if (!AreAdjacent(knot, next))
            {
                // The knot always follows the next knot (even diagonally) by one step.
                int newX = knot.X + next.X.CompareTo(knot.X);
                int newY = knot.Y + next.Y.CompareTo(knot.Y);

                knot = new Point(newX, newY);

                points.Add(knot);
            }
        }

        return points;
    }

    private Point MovePoint(Point p, char direction)
    {
        return direction switch
        {
            'U' => new Point(p.X - 1, p.Y),
            'D' => new Point(p.X + 1, p.Y),
            'L' => new Point(p.X, p.Y - 1),
            'R' => new Point(p.X, p.Y + 1),
            _ => throw new NotSupportedException(),
        };
    }

    private bool AreAdjacent(Point p1, Point p2)
    {
        return Math.Abs(p1.X - p2.X) <= 1 && Math.Abs(p1.Y - p2.Y) <= 1;
    }

    private record Point(int X, int Y);
}
