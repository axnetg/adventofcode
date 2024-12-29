namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("RAM Run", 2024, 18)]
public sealed class Day18 : IPuzzle
{
    private readonly IEnumerable<Position> _walls;
    private readonly (int X, int Y) _bounds = (70, 70);

    public Day18(TextReader reader)
    {
        _walls = reader.ReadAllLines().Select(x => x.Split(','))
            .Select(x => new Position(int.Parse(x[0]), int.Parse(x[1])));
    }

    public PuzzleResult SolvePartOne()
    {
        const int count = 1024;
        var walls = _walls.Take(count).ToHashSet();

        return CalculateShortestPath(walls, startNode: new (0, 0), endNode: new (70, 70));
    }

    public PuzzleResult SolvePartTwo()
    {
        var walls = _walls.ToList();
        var startNode = new Position(0, 0);
        var endNode = new Position(70, 70);

        var minimum = 0;
        var maximum = walls.Count;
        Position? solution = null;

        while (solution is null)
        {
            var length = minimum + ((maximum - minimum) / 2);
            var candidate1 = walls.GetRange(0, length).ToHashSet();
            var candidate2 = walls.GetRange(0, length + 1).ToHashSet();

            var result1 = CalculateShortestPath(candidate1, startNode, endNode);
            var result2 = CalculateShortestPath(candidate2, startNode, endNode);
            switch (new[] { result1, result2 })
            {
                case [> 0, -1]:
                    solution = walls[length];
                    break;
                case [> 0, > 0]:
                    minimum = length;
                    break;
                case [-1, -1]:
                    maximum = length;
                    break;
            }
        }

        return $"{solution.X},{solution.Y}";
    }

    private int CalculateShortestPath(HashSet<Position> walls, Position startNode, Position endNode)
    {
        var distances = new Dictionary<Position, int>();

        // priority queue for tracking shortest distance from the start node to each other node
        var queue = new PriorityQueue<Position, int>();

        // initialize the start node at a distance of 0
        distances[startNode] = 0;

        // add the start node to the queue for processing
        queue.Enqueue(startNode, 0);

        // as long as we have a node to process, keep looping
        while (queue.Count > 0)
        {
            // remove the node with the current smallest distance from the start node
            Position current = queue.Dequeue();

            // if this is the end node, then we're finished!
            if (current == endNode)
                return distances[current];

            // process each of the neighboring nodes
            foreach (Position neighborNode in GetNeighbors(current).Except(walls))
            {
                // get the current shortest distance to the connected node
                int distance = distances.TryGetValue(neighborNode, out var value)
                    ? value : int.MaxValue;

                // calculate the new cumulative distance to the edge
                int newDistance = distances[current] + 1;

                // if the new distance is shorter, then it represents a new shortest-path
                if (newDistance < distance)
                {
                    // update the shortest distance to the connection
                    distances[neighborNode] = newDistance;

                    // if the node is already in the queue, first remove it
                    queue.Remove(neighborNode, out _, out _);

                    // now add the node with the new distance
                    queue.Enqueue(neighborNode, newDistance);
                }
            }
        }

        return -1; // throw new InvalidOperationException("Maze has no solution!");
    }

    private IEnumerable<Position> GetNeighbors(Position coord)
    {
        var up = coord with { Y = coord.Y - 1 };
        var down = coord with { Y = coord.Y + 1 };
        var left = coord with { X = coord.X - 1 };
        var right = coord with { X = coord.X + 1 };

        return new[] { right, down, left, up }.Where(IsInBounds);

        bool IsInBounds(Position coord)
            => !(coord.Y < 0 || coord.X < 0 || coord.Y > _bounds.Y || coord.X > _bounds.X);
    }

    private sealed record Position(int X, int Y);
}
