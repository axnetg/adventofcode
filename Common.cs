namespace Axnetg.AdventOfCode;

public static class Common
{
    public static string[] ReadAllLines(this TextReader reader)
    {
        List<string> lines = [];

        while (reader.ReadLine() is string line)
            lines.Add(line);

        return lines.ToArray();
    }
}
