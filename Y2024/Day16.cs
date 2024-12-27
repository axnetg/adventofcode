using System.Collections.Immutable;
using System.Diagnostics;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Reindeer Maze", 2024, 16)]
public sealed class Day16 : IPuzzle
{
    private readonly char[][] _maze;

    public Day16(TextReader reader)
    {
        _maze = reader.ReadAllLines().Select(x => x.ToCharArray()).ToArray();
    }

    public PuzzleResult SolvePartOne()
    {
        var result = CalculateShortestPath();
        return result.Distance;
    }

    public PuzzleResult SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private NodeState CalculateShortestPath()
    {
        var distances = new Dictionary<Node, NodeState>();
        var startNode = GetStartingNode();

        // priority queue for tracking shortest distance from the start node to each other node
        var queue = new PriorityQueue<Node, int>();

        // initialize the start node at a distance of 0
        distances[startNode] = new NodeState(Distance: 0, Path: [startNode.Position]);

        // add the start node to the queue for processing
        queue.Enqueue(startNode, 0);

        // as long as we have a node to process, keep looping
        while (queue.Count > 0)
        {
            // remove the node with the current smallest distance from the start node
            Node current = queue.Dequeue();

            // if this is the end node, then we're finished!
            if (GetElementAt(current.Position) is 'E')
                return distances[current];

            // process each of the neighboring nodes
            foreach ((Node neighborNode, int neighborDistance) in GetNeighbors(current))
            {
                // get the current shortest distance to the connected node
                int distance = distances.TryGetValue(neighborNode, out var nodeState)
                    ? nodeState.Distance : int.MaxValue;

                // calculate the new cumulative distance to the edge
                int newDistance = distances[current].Distance + neighborDistance;

                // if the new distance is shorter, then it represents a new shortest-path
                if (newDistance < distance)
                {
                    var path = distances[current].Path;

                    // update the shortest distance to the connection
                    distances[neighborNode] = new NodeState(
                        Distance: newDistance, Path: path.Add(neighborNode.Position));

                    // if the node is already in the queue, first remove it
                    queue.Remove(neighborNode, out _, out _);

                    // now add the node with the new distance
                    queue.Enqueue(neighborNode, newDistance);
                }
            }
        }

        throw new InvalidOperationException("Maze has no solution!");
    }

    private Node GetStartingNode()
    {
        var positions = from row in Enumerable.Range(0, _maze.Length)
                        from col in Enumerable.Range(0, _maze[0].Length)
                        where _maze[row][col] == 'S'
                        select new Coordinate(row, col);

        return new Node(positions.First(), Direction.East);
    }

    private IEnumerable<(Node node, int distance)> GetNeighbors(Node current)
    {
        Node forward = current with { Position = GoForward(current.Position, current.Direction) };
        if (GetElementAt(forward.Position) is not '#') yield return (forward, 1);

        Node left = current with { Direction = TurnLeft() };
        yield return (left, 1000);

        Node right = current with { Direction = TurnRight() };
        yield return (right, 1000);

        Direction TurnLeft() => (Direction)(((int)current.Direction + 4 - 1) % 4);
        Direction TurnRight() => (Direction)(((int)current.Direction + 4 + 1) % 4);
    }

    private char GetElementAt(Coordinate coord)
        => _maze[coord.Row][coord.Col];

    private static Coordinate GoForward(Coordinate coord, Direction direction)
    {
        return direction switch
        {
            Direction.North => coord with { Row = coord.Row - 1 },
            Direction.East => coord with { Col = coord.Col + 1 },
            Direction.South => coord with { Row = coord.Row + 1 },
            Direction.West => coord with { Col = coord.Col - 1 },
            _ => throw new UnreachableException(),
        };
    }

    private enum Direction { North, East, South, West }

    private sealed record Node(Coordinate Position, Direction Direction);

    private sealed record NodeState(int Distance, ImmutableList<Coordinate> Path);

    private sealed record Coordinate(int Row, int Col);
}
