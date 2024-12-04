using System.Diagnostics;
using System.Reflection;

namespace Axnetg.AdventOfCode;

public static class PuzzleExecutor
{
    public static void Execute(int year, int day)
    {
        try
        {
            var puzzle = GetPuzzleForYearAndDay(year, day);

            using var reader = GetInputReader(puzzle);

            var puzzleInstance = GetInstanceOfPuzzle(puzzle.PuzzleType, reader);

            // ---- Puzzle solver start ----
            Console.WriteLine("========================================");
            Console.WriteLine(" > {0} Day {1}", puzzle.Year, puzzle.Day.ToString("00"));
            Console.WriteLine(" > {0}", puzzle.Title);

            // Time measurements
            var timestamp = Stopwatch.GetTimestamp();

            var resultPartOne = CatchError(puzzleInstance.SolvePartOne);
            Console.WriteLine(" > Part 1: {0}", resultPartOne);

            var resultPartTwo = CatchError(puzzleInstance.SolvePartTwo);
            Console.WriteLine(" > Part 2: {0}", resultPartTwo);
            // ---- Puzzle solver end ----

            Console.WriteLine(" > Time: {0} ms", Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds);
            Console.WriteLine();
        }
        catch (InvalidOperationException ex)
        {
            Console.Error.WriteLine("[!] Invalid operation: {0}", ex.Message);
            return;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("[!] General error occurred: {0}", ex);
            return;
        }
    }

    private static PuzzleMetadata GetPuzzleForYearAndDay(int year, int day)
    {
        var puzzle = GetAllPuzzleTypes().FirstOrDefault(x => x.Year == year && x.Day == day);
        if (puzzle == null)
            throw new InvalidOperationException($"No puzzle found for year = {year} and day = {day}.");

        return puzzle;
    }

    private static IEnumerable<PuzzleMetadata> GetAllPuzzleTypes()
    {
        return from type in Assembly.GetEntryAssembly()!.GetTypes()
               where typeof(IPuzzle).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract
               let attribute = type.GetCustomAttribute<PuzzleAttribute>()
               where attribute != null
               orderby attribute.Year, attribute.Day
               select new PuzzleMetadata(type, attribute.Title, attribute.Year, attribute.Day);
    }

    private static TextReader GetInputReader(PuzzleMetadata puzzle)
    {
        var inputFilePath = $"Inputs/{puzzle.Year}{puzzle.Day:00}.in";
        if (!File.Exists(inputFilePath))
            throw new InvalidOperationException($"Input file [{inputFilePath}] not found.");

        return new StreamReader(inputFilePath);
    }

    private static IPuzzle GetInstanceOfPuzzle(Type type, TextReader reader)
    {
        var constructor = type.GetConstructor([typeof(TextReader)]);
        if (constructor == null)
            throw new InvalidOperationException($"Type [{type.FullName}] must have a constructor with a parameter of type TextReader");

        if (constructor.Invoke([reader]) is not IPuzzle instance)
            throw new InvalidOperationException($"Could not create instance of type [{type.FullName}].");

        return instance;
    }

    private static PuzzleResult CatchError(Func<PuzzleResult> func)
    {
        try
        {
            return func.Invoke();
        }
        catch (NotImplementedException)
        {
            return "[NOT IMPLEMENTED]";
        }
        catch (Exception ex)
        {
            return $"{ex.Message}\n{ex.StackTrace}";
        }
    }

    private sealed record PuzzleMetadata(Type PuzzleType, string Title, int Year, int Day);
}
