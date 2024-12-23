namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Garden Groups", 2024, 12)]
public sealed class Day12 : IPuzzle
{
    private readonly char[][] _map;

    public Day12(TextReader reader)
    {
        _map = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        return GetAllPlantRegions().Sum(GetRegionScore);
    }

    public PuzzleResult SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private int GetRegionScore(IEnumerable<Plant> region)
    {
        var area = region.Count();
        var perimeter = region.Sum(p => 4 - GetAdyacentPlantsOfType(p.Type, p.Position).Count());
        return area * perimeter;
    }

    private int GetRegionDiscountedScore(IEnumerable<Plant> region)
    {
        var area = region.Count();
        var sides = 0;
        return area * sides;
    }

    private IEnumerable<IEnumerable<Plant>> GetAllPlantRegions()
    {
        HashSet<Coordinate> visitedPositions = [];

        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                var currentPosition = new Coordinate(Row: i, Col: j);
                if (visitedPositions.Contains(currentPosition)) continue;

                Plant plant = new(GetPlantTypeAtCoordinate(currentPosition), currentPosition);
                HashSet<Plant> region = [];

                TraverseGardenRecursive(region, plant);

                region.ToList().ForEach(r => visitedPositions.Add(r.Position));
                yield return region;
            }
        }
    }

    private void TraverseGardenRecursive(HashSet<Plant> region, Plant plant)
    {
        region.Add(plant);

        foreach (Plant adyancentPlant in GetAdyacentPlantsOfType(plant.Type, plant.Position))
        {
            if (region.Contains(adyancentPlant)) continue;
            TraverseGardenRecursive(region, adyancentPlant);
        }
    }

    private IEnumerable<Plant> GetAdyacentPlantsOfType(char type, Coordinate coord)
    {
        var up = coord with { Row = coord.Row - 1 };
        var down = coord with { Row = coord.Row + 1 };
        var left = coord with { Col = coord.Col - 1 };
        var right = coord with { Col = coord.Col + 1 };

        Coordinate[] directions = [up, down, left, right];
        return directions
            .Select(c => (type: GetPlantTypeAtCoordinate(c), pos: c))
            .Where(x => x.type == type)
            .Select(x => new Plant(x.type, x.pos));
    }

    private char GetPlantTypeAtCoordinate(Coordinate coord)
    {
        var row = coord.Row;
        var col = coord.Col;
        if (row < 0 || row >= _map.Length) return '.';
        if (col < 0 || col >= _map[row].Length) return '.';

        return _map[row][col];
    }

    private sealed record Plant(char Type, Coordinate Position);

    private sealed record Coordinate(int Row, int Col);
}
