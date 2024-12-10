using AocHelper;
internal class Program
{
    private static void Main(string[] args)
    {
        P2();
    }

    private static void P2()
    {
        var lines = File.ReadAllLines("./data.aoc");

        long sum = 0;
        foreach (string line in lines)
        {
            var rem = line.Parse<long>(out long target, ": ").ParseMany<long>("", " ", out long[] values1);
            ReadOnlyMemory<long> values = values1;

            bool result = CanFormP2(target, values.Span[0], values.Slice(1));
            if (result)
            {
                sum += target;
            }
        }

        Console.WriteLine(sum);
    }

    private static bool CanFormP2(long target, long current, ReadOnlyMemory<long> remaining)
    {
        if (remaining.Length == 0)
        {
            return current == target;
        }

        if (current > target)
        {
            return false;
        }

        long next = remaining.Span[0];
        remaining = remaining.Slice(1);
        return CanFormP2(target, current + next, remaining)
         || CanFormP2(target, current * next, remaining)
         || CanFormP2(target, long.Parse(current.ToString() + next.ToString()), remaining);
    }

    private static void P1()
    {
        var lines = File.ReadAllLines("./data.aoc");

        long sum = 0;
        foreach (string line in lines)
        {
            var rem = line.Parse<long>(out long target, ": ").ParseMany<long>("", " ", out long[] values1);
            ReadOnlyMemory<long> values = values1;

            //var first = new Checkpoint(values.Span[0], values.Slice(1));
            bool result = CanFormP1(target, values.Span[0], values.Slice(1));
            if (result)
            {
                sum += target;
            }
        }

        Console.WriteLine(sum);
    }

    private static bool CanFormP1(long target, long current, ReadOnlyMemory<long> remaining)
    {
        if (remaining.Length == 0)
        {
            return current == target;
        }

        long next = remaining.Span[0];
        remaining = remaining.Slice(1);
        return CanFormP1(target, current + next, remaining)
         || CanFormP1(target, current * next, remaining);
    }

    public record Checkpoint(long Current, ReadOnlyMemory<long> Remaining);
}