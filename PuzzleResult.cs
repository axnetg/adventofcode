namespace Axnetg.AdventOfCode;

public readonly struct PuzzleResult
{
    private readonly string _value;

    private PuzzleResult(string value)
    {
        _value = value;
    }

    // Implicit conversion from int to PuzzleResult
    public static implicit operator PuzzleResult(int value)
    {
        return new PuzzleResult(value.ToString());
    }

    // Implicit conversion from long to PuzzleResult
    public static implicit operator PuzzleResult(long value)
    {
        return new PuzzleResult(value.ToString());
    }

    // Implicit conversion from string to PuzzleResult
    public static implicit operator PuzzleResult(string value)
    {
        return new PuzzleResult(value);
    }

    // Override ToString to provide the textual representation
    public override string ToString()
    {
        return _value;
    }
}
