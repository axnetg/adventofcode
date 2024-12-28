namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Chronospatial Computer", 2024, 17)]
public sealed class Day17 : IPuzzle
{
    private readonly Memory _initialState;
    private readonly List<int> _program;

    public Day17(TextReader reader)
    {
        var lines = reader.ReadAllLines();

        var a = long.Parse(lines[0].Substring("Register A:".Length).Trim());
        var b = long.Parse(lines[1].Substring("Register B:".Length).Trim());
        var c = long.Parse(lines[2].Substring("Register C:".Length).Trim());
        _initialState = new Memory(a, b, c);

        var program = lines[4].Substring("Program:".Length).Trim();
        _program = program.Split(',').Select(int.Parse).ToList();
    }

    public PuzzleResult SolvePartOne()
    {
        var memory = new Memory(_initialState.RegA, _initialState.RegB, _initialState.RegC);
        ExecuteProgram(memory);

        return string.Join(',', memory.Output);
    }

    public PuzzleResult SolvePartTwo()
    {
        if (!TryFindStartingValueRecursive(0, _program.Count - 1, out var solution))
            throw new InvalidOperationException("No solution found.");

        return solution;
    }

    private bool TryFindStartingValueRecursive(long value, int position, out long solution)
    {
        solution = value;
        if (position < 0) return true;

        var goal = string.Join(',', _program[position..]);
        var candidate = value << 3;

        for (int i = 0; i < 8; i++)
        {
            if (RunProgram(candidate + i) != goal)
                continue;

            if (TryFindStartingValueRecursive(candidate + i, position - 1, out solution))
                return true;
        }

        return false;

        string RunProgram(long a)
        {
            var memory = new Memory(a, _initialState.RegB, _initialState.RegC);
            ExecuteProgram(memory);

            return string.Join(',', memory.Output);
        }
    }

    private void ExecuteProgram(Memory memory)
    {
        var pointer = 0;

        while (_program.Count > pointer)
        {
            (var opcode, var operand) = (_program[pointer++], _program[pointer++]);

            switch (opcode)
            {
                case 0:
                    Adv(memory, operand);
                    break;
                case 1:
                    Bxl(memory, operand);
                    break;
                case 2:
                    Bst(memory, operand);
                    break;
                case 3:
                    Jnz(memory, operand, ref pointer);
                    break;
                case 4:
                    Bxc(memory);
                    break;
                case 5:
                    Out(memory, operand);
                    break;
                case 6:
                    Bdv(memory, operand);
                    break;
                case 7:
                    Cdv(memory, operand);
                    break;
            }
        }
    }

    private void Adv(Memory memory, int operand)
    {
        var numer = memory.RegA;
        var denom = Math.Pow(2, ComboOperand(memory, operand));
        memory.RegA = (long)(numer / denom);
    }

    private void Bxl(Memory memory, int operand)
    {
        var value = memory.RegB ^ operand;
        memory.RegB = value;
    }

    private void Bst(Memory memory, int operand)
    {
        var value = ComboOperand(memory, operand) % 8;
        memory.RegB = value;
    }

    private void Jnz(Memory memory, int operand, ref int pointer)
    {
        if (memory.RegA is not 0)
            pointer = operand;
    }

    private void Bxc(Memory memory)
    {
        var value = memory.RegB ^ memory.RegC;
        memory.RegB = value;
    }

    private void Out(Memory memory, int operand)
    {
        var value = ComboOperand(memory, operand) % 8;
        memory.Output.Add(value);
    }

    private void Bdv(Memory memory, int operand)
    {
        var numer = memory.RegA;
        var denom = Math.Pow(2, ComboOperand(memory, operand));
        memory.RegB = (long)(numer / denom);
    }

    private void Cdv(Memory memory, int operand)
    {
        var numer = memory.RegA;
        var denom = Math.Pow(2, ComboOperand(memory, operand));
        memory.RegC = (long)(numer / denom);
    }

    private long ComboOperand(Memory memory, int operand)
    {
        return operand switch
        {
            >= 0 and < 4 => operand,
            4 => memory.RegA,
            5 => memory.RegB,
            6 => memory.RegC,
            _ => throw new ArgumentException($"Invalid operand [{operand}]", nameof(operand)),
        };
    }

    private sealed class Memory(long RegA, long RegB, long RegC)
    {
        public long RegA { get; set; } = RegA;
        public long RegB { get; set; } = RegB;
        public long RegC { get; set; } = RegC;
        public List<long> Output { get; } = [];
    }
}
