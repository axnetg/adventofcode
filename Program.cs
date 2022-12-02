using System.CommandLine;
using AdventOfCode.Core;

namespace AdventOfCode;

public class Program
{
    public static void Main(string[] args)
    {
        var yearOption = new Option<int?>(
            aliases: new[] { "--year", "-y" },
            description: "The challenge year to solve.");

        var dayOption = new Option<int?>(
            aliases: new[] { "--day", "-d" },
            description: "The day of the year to solve.");

        RootCommand rootCommand = new(description: "AdventOfCode solution solver application.")
        {
            yearOption,
            dayOption,
        };

        rootCommand.SetHandler(HandlerAction, yearOption, dayOption);
        rootCommand.Invoke(args);
    }

    private static void HandlerAction(int? year, int? day)
    {
        if (day.HasValue && !year.HasValue)
        {
            Console.Error.WriteLine("Please specify a year.");
            Environment.Exit(1);
        }

        Solver solver = new Solver(year, day);
        solver.Run();
    }
}
