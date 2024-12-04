using System.CommandLine;
using Axnetg.AdventOfCode;

var yearOption = new Option<int>(aliases: ["--year", "-y"], description: "The year of the puzzle");
var dayOption = new Option<int>(aliases: ["--day", "-d"], description: "The day of the puzzle");

var rootCommand = new RootCommand("Advent of Code Resolver") { yearOption, dayOption };

rootCommand.SetHandler(ctx =>
{
    var year = ctx.BindingContext.ParseResult.GetValueForOption(yearOption);
    var day = ctx.BindingContext.ParseResult.GetValueForOption(dayOption);
    PuzzleExecutor.Execute(year, day);

    return Task.CompletedTask;
});

return await rootCommand.InvokeAsync(args);
