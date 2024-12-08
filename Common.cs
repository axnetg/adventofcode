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

    public static IEnumerable<T> Splice<T>(this IEnumerable<T> source, int start)
    {
        return source.Where((_, idx) => idx < start);
    }

    public static IEnumerable<T> Splice<T>(this IEnumerable<T> source, int start, int deleteCount)
    {
        int end = start + deleteCount;
        return source.Where((_, idx) => idx < start || idx >= end);
    }
}
