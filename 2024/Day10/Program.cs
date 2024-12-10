using AocHelper;
using System.Net;

internal class Program
{
    private static void Main(string[] args)
    {
        P2();
    }

    private static void P2()
    {
        var map = File.ReadAllLines("./data.aoc").GetMap<int>(out int xLength, out int yLength);

        int sum = 0;
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (map[x, y] == 0)
                {
                    var result = SearchP2(map, xLength, yLength, x, y, 0);
                    sum += result;
                }
            }
        }

        Console.WriteLine(sum);
    }

    private static int SearchP2(int[,] map, int xLength, int yLength, int x, int y, int expectedLevel)
    {
        if (x < 0
            || x >= xLength
            || y < 0
            || y >= yLength
            || expectedLevel != map[x, y])
        {
            return 0;
        }

        if (expectedLevel == 9)
        {
            return 1;
        }

        var north = SearchP2(map, xLength, yLength, x, y - 1, expectedLevel + 1);
        var result = north;

        var east = SearchP2(map, xLength, yLength, x + 1, y, expectedLevel + 1);
        result += east;

        var south = SearchP2(map, xLength, yLength, x, y + 1, expectedLevel + 1);
        result += south;

        var west = SearchP2(map, xLength, yLength, x - 1, y, expectedLevel + 1);
        result += west;

        return result;
    }

    private static void P1()
    {
        var map = File.ReadAllLines("./data.aoc").GetMap<int>(out int xLength, out int yLength);

        int sum = 0;
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (map[x, y] == 0)
                {
                    var result = SearchP1(map, xLength, yLength, x, y, 0);
                    sum += result.Count;
                }
            }
        }

        Console.WriteLine(sum);
    }

    private static HashSet<Point>? SearchP1(int[,] map, int xLength, int yLength, int x, int y, int expectedLevel)
    {
        if (x < 0
            || x >= xLength
            || y < 0
            || y >= yLength
            || expectedLevel != map[x, y])
        {
            return null;
        }

        if (expectedLevel == 9)
        {
            return new HashSet<Point>() { new Point(x, y) };
        }

        var north = SearchP1(map, xLength, yLength, x, y - 1, expectedLevel + 1);
        var result = north;

        var east = SearchP1(map, xLength, yLength, x + 1, y, expectedLevel + 1);
        result = MergeResultsP1(result, east);

        var south = SearchP1(map, xLength, yLength, x, y + 1, expectedLevel + 1);
        result = MergeResultsP1(result, south);

        var west = SearchP1(map, xLength, yLength, x - 1, y, expectedLevel + 1);
        result = MergeResultsP1(result, west);

        return result;
    }

    private static HashSet<Point>? MergeResultsP1(HashSet<Point>? current, HashSet<Point>? next)
    {
        if (next == null)
        {
            return current;
        }

        if (current == null)
        {
            return next;
        }

        return new HashSet<Point>(current.Concat(next));
    }
}