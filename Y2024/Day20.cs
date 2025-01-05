namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Race Condition", 2024, 20)]
public sealed class Day20 : IPuzzle
{
    private readonly char[][] _map;

    public Day20(TextReader reader)
    {
        _map = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        var raceTrackCoords = GetRaceTrackCoordinates();
        var cheats = GetPossibleCheats(raceTrackCoords);

        var scores = Enumerable.Reverse(raceTrackCoords)
            .Select((coord, idx) => new { coord, idx })
            .ToDictionary(x => x.coord, x => x.idx);

        const int time = 100;
        return cheats.Where(c => GetSavedTime(c) >= time).Count();

        int GetSavedTime(Cheat c) => scores[c.From] - scores[c.To] - 2;
    }

    public PuzzleResult SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private List<Coordinate> GetRaceTrackCoordinates()
    {
        // up, down, left, or right
        IEnumerable<(int X, int Y)> directions = [(0, -1), (0, 1), (-1, 0), (1, 0)];

        var previous = (Coordinate?)null;
        var actual = GetStartingPosition();

        List<Coordinate> raceTrackCoords = [actual];

        while (GetElementAt(actual) is not 'E')
        {
            var next = directions
                .Select(dir => (row: actual.Row + dir.X, col: actual.Col + dir.Y))
                .Select(x => new Coordinate(x.row, x.col))
                .First(coord => GetElementAt(coord) is not '#' && coord != previous);

            previous = actual;
            actual = next;

            raceTrackCoords.Add(actual);
        }

        return raceTrackCoords;
    }

    private List<Cheat> GetPossibleCheats(List<Coordinate> raceTrackCoords)
    {
        // up, down, left, or right
        IEnumerable<(int X, int Y)> directions = [(0, -1), (0, 1), (-1, 0), (1, 0)];

        List<Cheat> cheatList = [];
        HashSet<Coordinate> visited = [];

        foreach (var actual in raceTrackCoords)
        {
            visited.Add(actual);

            var walls = directions
                .Select(dir => (dir, row: actual.Row + dir.X, col: actual.Col + dir.Y))
                .Select(x => (x.dir, wall: new Coordinate(x.row, x.col)))
                .Where(x => GetElementAt(x.wall) is '#' && !IsOuterWall(x.wall));

            foreach ((var direction, var wall) in walls)
            {
                int row = actual.Row + 2 * direction.X;
                int col = actual.Col + 2 * direction.Y;
                var dest = new Coordinate(row, col);

                if (GetElementAt(dest) is not '#' && !visited.Contains(dest))
                {
                    cheatList.Add(new Cheat(From: actual, To: dest));
                }
            }
        }

        return cheatList;
    }

    private Coordinate GetStartingPosition()
    {
        var positions = from row in Enumerable.Range(0, _map.Length)
                        from col in Enumerable.Range(0, _map[0].Length)
                        where _map[row][col] == 'S'
                        select new Coordinate(row, col);

        return positions.First();
    }

    private bool IsOuterWall(Coordinate coord)
    {
        return coord.Row is 0
            || coord.Col is 0
            || coord.Row == _map.Length - 1
            || coord.Col == _map[0].Length - 1;
    }

    private char GetElementAt(Coordinate coord)
        => _map[coord.Row][coord.Col];

    private sealed record Coordinate(int Row, int Col);

    private sealed record Cheat(Coordinate From, Coordinate To);
}
