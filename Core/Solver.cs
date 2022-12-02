using System.Reflection;

namespace AdventOfCode.Core;

public class Solver
{
    public Solver(int? year, int? day)
    {
        Year = year;
        Day = day;
    }

    public int? Year { get; }

    public int? Day { get; }

    public void Run()
    {
        Console.WriteLine(Constants.WelcomeText);

        var challenges = from type in Assembly.GetExecutingAssembly().GetTypes()
                         where typeof(ISolution).IsAssignableFrom(type) && Attribute.IsDefined(type, typeof(ChallengeAttribute))
                         let attr = type.GetCustomAttribute<ChallengeAttribute>()!
                         where !Year.HasValue || Year.Value == attr.Year && (!Day.HasValue || Day.Value == attr.Day)
                         orderby attr.Year, attr.Day
                         let solution = Activator.CreateInstance(type) as ISolution
                         select new Challenge(solution!, attr.Name, attr.Year, attr.Day);

        foreach (var challenge in challenges)
        {
            SolveChallenge(challenge);
        }

        if (!challenges.Any())
        {
            Console.WriteLine("No challenges found! :(");
        }
    }

    private static void SolveChallenge(Challenge challenge)
    {
        try
        {
            var path = Path.Combine($"Y{challenge.Year}", $"Day{challenge.Day:00}", "input.in");
            var input = File.ReadAllText(path).TrimEnd('\n');

            var answers = GetAnswers(challenge.Solution, input);

            PrintAnswers(challenge, answers);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }

    private static void PrintAnswers(Challenge challenge, IEnumerable<object> answers)
    {
        Console.WriteLine("====================");
        Console.WriteLine($" > {challenge.Year} Day {challenge.Day:00}");
        Console.WriteLine($" > {challenge.Name}");

        foreach (var answer in answers.Where(a => a != null))
        {
            Console.WriteLine($" > {answer}");
        }

        Console.WriteLine();
    }

    private static IEnumerable<object> GetAnswers(ISolution solution, string input)
    {
        yield return solution.PartOne(input);
        yield return solution.PartTwo(input);
    }

    private record Challenge(ISolution Solution, string Name, int Year, int Day);
}
