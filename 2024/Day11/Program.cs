using AocHelper;

public class Program
{
    public static int itrCount = 0;

    private static void Main(string[] args)
    {
        var lines = File.ReadAllLines("data.aoc");

        lines.First().ParseMany("", " ", out long[] startingValues, "");

        long totalResult = 0;
        var cache = new Dictionary<Stone, long>();
        foreach (var start in startingValues)
        {
            totalResult += GetStoneCount(new Stone(start, 75), cache);
        }

        Console.WriteLine($"Itr: {itrCount}");
        Console.WriteLine(totalResult);
        
    }

    public static long GetStoneCount(Stone stone, Dictionary<Stone, long> cache)
    {
        itrCount++;
        if (stone.Level == 0)
        {
            //Console.Write($"{stone.Value} ");
            return 1;
        }

        if (cache.TryGetValue(stone, out long found))
        {
            return found;
        }

        int nextLevel = stone.Level - 1;
        long stoneCount = 0;
        if (stone.Value == 0)
        {
            stoneCount = GetStoneCount(new Stone(1, nextLevel), cache);
        }
        else
        {
            var str = stone.Value.ToString().AsSpan();
            if (str.Length % 2 == 0)
            {
                int midPoint = str.Length / 2;
                long val1 = long.Parse(str.Slice(0, midPoint));
                long val2 = long.Parse(str.Slice(midPoint));

                stoneCount += GetStoneCount(new Stone(val1, nextLevel), cache);
                stoneCount += GetStoneCount(new Stone(val2, nextLevel), cache);
            }
            else
            {
                stoneCount = GetStoneCount(new Stone(stone.Value * 2024, nextLevel), cache);
            }
        }

        cache.Add(stone, stoneCount);

        return stoneCount;
    }

    public record struct Stone(long Value, int Level);
}