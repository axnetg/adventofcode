using System.Data;
using System.Diagnostics;
using System.Text;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Warehouse Woes", 2024, 15)]
public sealed class Day15 : IPuzzle
{
    private readonly List<string> _initialMap = [];
    private readonly List<Direction> _movements = [];

    public Day15(TextReader reader)
    {
        while (reader.ReadLine() is string line && !string.IsNullOrWhiteSpace(line))
        {
            _initialMap.Add(line);
        }

        while (reader.ReadLine() is string line)
        {
            _movements.AddRange(line.Select(ParseDirection));
        }
    }

    public PuzzleResult SolvePartOne()
    {
        char[][] map = _initialMap.Select(x => x.ToCharArray()).ToArray();

        StartRobotMovement(map);

        return GetCoordinatesOf(map, 'O').Sum(GpsScore);
    }

    public PuzzleResult SolvePartTwo()
    {
        char[][] map = _initialMap.Select(x => Widen(x).ToCharArray()).ToArray();

        StartRobotMovement(map);

        return GetCoordinatesOf(map, '[').Sum(GpsScore);

        static string Widen(string s)
            => new StringBuilder(s).Replace("#", "##").Replace("O", "[]").Replace(".", "..").Replace("@", "@.").ToString();
    }

    private void StartRobotMovement(char[][] map)
    {
        // [!] NOTE: mutates the original map
        var position = GetStartingPoint(map);
        foreach (var movement in _movements)
        {
            if (TryPush(map, position, movement, out var next))
                position = next;
        }
    }

    private Coordinate GetStartingPoint(char[][] map)
        => GetCoordinatesOf(map, '@').First();

    private IEnumerable<Coordinate> GetCoordinatesOf(char[][] map, char character)
        => from row in Enumerable.Range(0, map.Length)
           from col in Enumerable.Range(0, map[0].Length)
           where map[row][col] == character
           select new Coordinate(row, col);

    private int GpsScore(Coordinate coord)
        => (100 * coord.Row) + coord.Col;

    private Direction ParseDirection(char c)
        => c switch
        {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new UnreachableException(),
        };

    private bool TryPush(char[][] map, Coordinate current, Direction direction, out Coordinate next)
    {
        next = GetCoordinateInDirection(current, direction);

        // if next element is a wall, we can't push
        if (GetElementAt(map, next) is '#')
            return false;

        // special case for stacked boxes of size two in vertical direction
        if (direction is Direction.Up or Direction.Down && GetElementAt(map, next) is '[' or ']')
        {
            if (!TryPushStackedBoxes(map, current, direction))
                return false;

            SwapContentsOf(map, current, next);
            return true;
        }

        // if next element is an empty space, we can swap; otherwise, try pushing recursively to move all boxes in a line
        if (GetElementAt(map, next) is '.' || TryPush(map, next, direction, out _))
        {
            SwapContentsOf(map, current, next);
            return true;
        }

        return false;
    }

    private bool TryPushStackedBoxes(char[][] map, Coordinate current, Direction direction)
    {
        List<List<Coordinate>> rowsOfBoxCoords = [];
        List<Coordinate> currentRow = [current];

        // while there is no wall or not all elements are spaces -> keep scanning for boxes
        while (!(currentRow.Any(x => GetElementAt(map, GetCoordinateInDirection(x, direction)) is '#')
            || currentRow.All(x => GetElementAt(map, GetCoordinateInDirection(x, direction)) is '.')))
        {
            List<Coordinate> nextRow = [];
            foreach (var boxCoord in currentRow)
            {
                var c = GetCoordinateInDirection(boxCoord, direction);
                if (GetElementAt(map, c) is '[')
                {
                    nextRow.Add(c);
                    nextRow.Add(GetCoordinateInDirection(c, Direction.Right));
                }
                else if (GetElementAt(map, c) is ']')
                {
                    nextRow.Add(GetCoordinateInDirection(c, Direction.Left));
                    nextRow.Add(c);
                }
            }

            currentRow = nextRow.Distinct().ToList();
            rowsOfBoxCoords.Add(currentRow);
        }

        // if wall was found then we can't move any of the boxes
        if (currentRow.Any(x => GetElementAt(map, GetCoordinateInDirection(x, direction)) is '#'))
            return false;

        // iterate row by row in reverse (furthest rows first) swapping positions with empty spaces
        foreach (var row in Enumerable.Reverse(rowsOfBoxCoords))
            foreach (var boxPos in row)
                SwapContentsOf(map, boxPos, GetCoordinateInDirection(boxPos, direction));

        return true;
    }

    private static Coordinate GetCoordinateInDirection(Coordinate coord, Direction direction)
    {
        return direction switch
        {
            Direction.Up => coord with { Row = coord.Row - 1 },
            Direction.Down => coord with { Row = coord.Row + 1 },
            Direction.Left => coord with { Col = coord.Col - 1 },
            Direction.Right => coord with { Col = coord.Col + 1 },
            _ => throw new UnreachableException(),
        };
    }

    private char GetElementAt(char[][] map, Coordinate coord)
        => map[coord.Row][coord.Col];

    private void SwapContentsOf(char[][] map, Coordinate c1, Coordinate c2)
    {
        ((var y1, var x1), (var y2, var x2)) = (c1, c2);
        (map[y1][x1], map[y2][x2]) = (map[y2][x2], map[y1][x1]);
    }

    private enum Direction { Up, Down, Left, Right }

    private sealed record Coordinate(int Row, int Col);
}
