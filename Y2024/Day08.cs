namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Resonant Collinearity", 2024, 08)]
public sealed class Day08 : IPuzzle
{
    private readonly IEnumerable<Antenna> _antennas;
    private readonly (int maxrow, int maxcol) _mapBounds;

    public Day08(TextReader reader)
    {
        var map = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();

        _antennas = ParseAntennas(map);
        _mapBounds = (maxrow: map.Length, maxcol: map[0].Length);
    }

    public PuzzleResult SolvePartOne()
    {
        return _antennas.SelectMany(ResonantFrequenciesGenerator)
            .Where(IsInBounds).Distinct().Count();
    }

    public PuzzleResult SolvePartTwo()
    {
        return _antennas.SelectMany(ResonantHarmonicsGenerator)
            .Where(IsInBounds).Distinct().Count();
    }

    private IEnumerable<Antenna> ParseAntennas(char[][] map)
    {
        var flat = map.SelectMany(x => x).ToArray();
        return Enumerable
            .Range(0, flat.Length)
            .Where(idx => flat[idx] is not '.')
            .Select(CreateAntenna);

        Antenna CreateAntenna(int idx)
        {
            char freq = flat[idx];
            var coord = new Coordinate(idx / map.Length, idx % map.Length);
            return new Antenna(freq, coord);
        }
    }

    private IEnumerable<Coordinate> ResonantFrequenciesGenerator(Antenna a1)
    {
        return GetMatchingAntennas(a1)
            .Select(a2 => GetAntinodePositionBetween(a1.Position, a2.Position, distance: 2));
    }

    private IEnumerable<Coordinate> ResonantHarmonicsGenerator(Antenna a1)
    {
        var maxDistance = Math.Max(_mapBounds.maxrow, _mapBounds.maxcol) + 1;

        return GetMatchingAntennas(a1)
            .SelectMany(
                a2 => Enumerable
                    .Range(1, maxDistance)
                    .Select(i => GetAntinodePositionBetween(a1.Position, a2.Position, distance: i))
                    .TakeWhile(IsInBounds)
            );
    }

    private IEnumerable<Antenna> GetMatchingAntennas(Antenna antenna)
    {
        return _antennas.Where(x => x.Frequency == antenna.Frequency).Except([antenna]);
    }

    private Coordinate GetAntinodePositionBetween(Coordinate c1, Coordinate c2, int distance)
    {
        var vector = (x: c2.Row - c1.Row, y: c2.Col - c1.Col);
        var antinode = (row: c1.Row + vector.x * distance, col: c1.Col + vector.y * distance);
        return new Coordinate(antinode.row, antinode.col);
    }

    private bool IsInBounds(Coordinate c)
    {
        return !(c.Row < 0
            || c.Col < 0
            || c.Row >= _mapBounds.maxrow
            || c.Col >= _mapBounds.maxcol);
    }

    private sealed record Antenna(char Frequency, Coordinate Position);

    private sealed record Coordinate(int Row, int Col);
}
