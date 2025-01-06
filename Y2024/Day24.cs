using System.Diagnostics;

namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Crossed Wires", 2024, 24)]
public sealed class Day24 : IPuzzle
{
    private readonly List<Wire> _wires = [];
    private readonly List<Instruction> _instructions = [];

    public Day24(TextReader reader)
    {
        while (reader.ReadLine() is string line && !string.IsNullOrWhiteSpace(line))
        {
            var pair = line.Split(':', StringSplitOptions.TrimEntries);
            _wires.Add(new Wire(pair[0], pair[1] is "1"));
        }

        while (reader.ReadLine() is string line)
        {
            var groups = line.Split(' ', StringSplitOptions.TrimEntries);
            _instructions.Add(new Instruction(groups[0], groups[2], groups[1], groups[4]));
        }
    }

    public PuzzleResult SolvePartOne()
    {
        var result = RunSimulation().Where(x => x.Name.StartsWith('z')).OrderBy(x => x.Name);

        return result.Select(ToBinary).Sum();
    }

    public PuzzleResult SolvePartTwo()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<Wire> RunSimulation()
    {
        Dictionary<string, bool> values = _wires.ToDictionary(x => x.Name, x => x.Value);

        Queue<Instruction> pending = new();
        _instructions.ForEach(pending.Enqueue);

        while (pending.Count > 0)
        {
            var instruction = pending.Dequeue();

            // if we can't solve the operation: that means we don't have the values for both operands
            // so enquque to the end of the list and continue to next instruction
            if (!values.ContainsKey(instruction.A) || !values.ContainsKey(instruction.B))
            {
                pending.Enqueue(instruction);
                continue;
            }

            bool result = SimulateLogicGate(values[instruction.A], values[instruction.B], instruction.Gate);
            values[instruction.Out] = result;
        }

        return values.Select(x => new Wire(x.Key, x.Value));
    }

    private static bool SimulateLogicGate(bool a, bool b, string gate) => gate switch
    {
        "AND" => a & b,
        "OR"  => a | b,
        "XOR" => a ^ b,
        _ => throw new UnreachableException(),
    };

    private static long ToBinary(Wire wire, int idx)
    {
        if (!wire.Value) return 0;
        return (long)Math.Pow(2, idx);
    }

    private sealed record Wire(string Name, bool Value);

    private sealed record Instruction(string A, string B, string Gate, string Out);
}
