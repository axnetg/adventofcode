namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Code Chronicle", 2024, 25)]
public sealed class Day25 : IPuzzle
{
    private readonly IEnumerable<Block> _blocks;

    public Day25(TextReader reader)
    {
        _blocks = reader.ReadToEnd()
            .ReplaceLineEndings("\n")
            .Split("\n\n")
            .Select(x => new Block(x));
    }

    public PuzzleResult SolvePartOne()
    {
        return GetAllCombinations().Count(x => AreComplementaryBlocks(x.First, x.Second));
    }

    public PuzzleResult SolvePartTwo()
    {
        return "N/A";
    }

    private IEnumerable<(Block First, Block Second)> GetAllCombinations()
    {
        var list = _blocks.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                yield return (list[i], list[j]);
            }
        }
    }

    private static bool AreComplementaryBlocks(Block b1, Block b2)
    {
        // We define complementary blocks as those that, for each position of the grid,
        // never overlap (this is, both must never be '#' at the same time).

        return b1.Lines.Zip(b2.Lines).All(pair => pair is not ('#', '#'));
    }

    private sealed record Block(string Lines);
}
