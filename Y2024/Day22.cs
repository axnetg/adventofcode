namespace Axnetg.AdventOfCode.Y2024;

[Puzzle("Monkey Market", 2024, 22)]
public sealed class Day22 : IPuzzle
{
    private readonly IEnumerable<int> _secretNumbers;

    public Day22(TextReader reader)
    {
        _secretNumbers = reader.ReadAllLines().Select(int.Parse);
    }

    public PuzzleResult SolvePartOne()
    {
        const int count = 2000;

        return _secretNumbers.Sum(seed => GenerateRandomEnumerable(seed).Take(count).Last());
    }

    public PuzzleResult SolvePartTwo()
    {
        const int count = 2000;

        var offers = _secretNumbers.SelectMany(seed =>
        {
            var numbers = GenerateRandomEnumerable(seed).Take(count);
            var diffs = GetPriceDiffs(seed % 10, numbers.Select(n => (int)(n % 10)));
            return GetBuyOffers(diffs);
        });

        return GetBestOffer(offers);
    }

    private IEnumerable<PriceDiff> GetPriceDiffs(int startingPrice, IEnumerable<int> prices)
    {
        int previous = startingPrice;
        foreach (var price in prices)
        {
            yield return new PriceDiff(price, price - previous);
            previous = price;
        }
    }

    private IEnumerable<BuyOffer> GetBuyOffers(IEnumerable<PriceDiff> diffs)
    {
        var enumerator = diffs.GetEnumerator();

        if (!enumerator.MoveNext()) yield break;
        var a = enumerator.Current.Diff;

        if (!enumerator.MoveNext()) yield break;
        var b = enumerator.Current.Diff;

        if (!enumerator.MoveNext()) yield break;
        var c = enumerator.Current.Diff;

        // avoid duplicate patterns, as there could be more than one pattern
        // with different prices but we only care about the first one found!
        HashSet<DiffPattern> patternSet = [];

        while (enumerator.MoveNext())
        {
            var (price, d) = enumerator.Current;
            var pattern = new DiffPattern(a, b, c, d);

            if (!patternSet.Contains(pattern)) yield return new BuyOffer(pattern, price);
            patternSet.Add(pattern);

            (a, b, c) = (b, c, d);
        }
    }

    private int GetBestOffer(IEnumerable<BuyOffer> offers)
    {
        Dictionary<DiffPattern, int> prices = [];

        foreach (var offer in offers)
        {
            int total = prices.TryGetValue(offer.Pattern, out int value) ? value : 0;
            prices[offer.Pattern] = total + offer.Price;
        }

        return prices.Values.Max();
    }

    private static IEnumerable<long> GenerateRandomEnumerable(int seed)
    {
        var r = new RandomNumberGenerator(seed);
        while (true)
            yield return r.Next();
    }

    private sealed record PriceDiff(int Price, int Diff);

    private sealed record BuyOffer(DiffPattern Pattern, int Price);

    private sealed record DiffPattern(int A, int B, int C, int D);

    private sealed class RandomNumberGenerator(long seed)
    {
        private const long Modulo = 16777216;

        private long _current = seed;

        public long Next()
        {
            var n0 = _current;
            var n1 = Prune(Mix(n0 * 64, n0));
            var n2 = Prune(Mix(n1 / 32, n1));
            var n3 = Prune(Mix(n2 * 2048, n2));

            _current = n3;
            return _current;
        }

        private static long Mix(long a, long b) => a ^ b;

        private static long Prune(long a) => a % Modulo;
    }
}
