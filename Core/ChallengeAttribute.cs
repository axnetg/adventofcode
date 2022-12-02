namespace AdventOfCode.Core;

[AttributeUsage(AttributeTargets.Class)]
public class ChallengeAttribute : Attribute
{
    public ChallengeAttribute(string name, int year, int day)
    {
        Name = name;
        Year = year;
        Day = day;
    }

    public string Name { get; }
    public int Year { get; }
    public int Day { get; }
}
