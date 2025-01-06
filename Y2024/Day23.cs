namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("LAN Party", 2024, 23)]
public sealed class Day23 : IPuzzle
{
    private readonly Dictionary<Computer, HashSet<Computer>> _network = [];

    public Day23(TextReader reader)
    {
        var connections = reader.ReadAllLines()
            .Select(line => line.Split('-'))
            .Select(pair => (nodeA: new Computer(pair[0]), nodeB: new Computer(pair[1])));

        foreach (var (nodeA, nodeB) in connections)
        {
            GetOrCreateConnections(nodeA).Add(nodeB);
            GetOrCreateConnections(nodeB).Add(nodeA);
        }

        HashSet<Computer> GetOrCreateConnections(Computer key)
        {
            if (_network.TryGetValue(key, out var value))
                return value;

            var connections = new HashSet<Computer>();
            _network.Add(key, connections);
            return connections;
        }
    }

    public PuzzleResult SolvePartOne()
    {
        return FindTriangles().Count(t => t.Any(computer => computer.Name.StartsWith('t')));
    }

    public PuzzleResult SolvePartTwo()
    {
        var largest = _network.Keys.Select(FindClique).MaxBy(clique => clique.Count)!;

        return string.Join(',', largest.Select(computer => computer.Name).Order());
    }

    private IEnumerable<HashSet<Computer>> FindTriangles()
    {
        return from nodeA in _network.Keys
               from nodeB in _network[nodeA]
               from nodeC in _network[nodeB]
               where nodeA < nodeB && nodeB < nodeC
               where _network[nodeA].Contains(nodeC)
               select new HashSet<Computer>() { nodeA, nodeB, nodeC };
    }

    private HashSet<Computer> FindClique(Computer computer)
    {
        HashSet<Computer> clique = [computer];
        foreach (var neighbor in _network[computer])
        {
            if (clique.All(node => _network[neighbor].Contains(node)))
            {
                clique.Add(neighbor);
            }
        }

        return clique;
    }

    private sealed record Computer(string Name)
    {
        public static bool operator >(Computer a, Computer b) => a.Name.CompareTo(b.Name) > 0;
        public static bool operator <(Computer a, Computer b) => a.Name.CompareTo(b.Name) < 0;
    }
}
