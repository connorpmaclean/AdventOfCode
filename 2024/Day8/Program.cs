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
        var map = lines.GetMap<char>(out int xLength, out int yLength);

        var list = new List<(char name, Point point)>();
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                char c = map[x, y];
                if (c != '.')
                {
                    list.Add((c, new Point(x, y)));
                }
            }
        }

        var lookup = list.ToLookup(x => x.name, x => x.point);
        var antiNodes = new HashSet<Point>();

        foreach (var group in lookup)
        {
            var pointList = group.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                for (int j = i + 1; j < pointList.Count; j++)
                {
                    AddAntiNodesP2(antiNodes, pointList[i], pointList[j], xLength, yLength);
                }
            }
        }

        Console.WriteLine(antiNodes.Count);
    }

    public static void AddAntiNodesP2(HashSet<Point> antiNodes, Point p1, Point p2, int xLength, int yLength)
    {
        long xDist = Math.Abs(p1.X - p2.X);
        long yDist = Math.Abs(p1.Y - p2.Y);

        Func<long, long> x1Func, y1Func, x2Func, y2Func;
        if (p1.X < p2.X)
        {
            x1Func = x => x - xDist;
            x2Func = x => x + xDist;
        }
        else
        {
            x1Func = x => x + xDist;
            x2Func = x => x - xDist;
        }

        if (p1.Y < p2.Y)
        {
            y1Func = y => y - yDist;
            y2Func = y => y + yDist;
        }
        else
        {
            y1Func = y => y + yDist;
            y2Func = y => y - yDist;
        }

        long x1 = p1.X;
        long y1 = p1.Y;
        long x2 = p2.X;
        long y2 = p2.Y;

        while (x1 >= 0 && x1 < xLength && y1 >= 0 && y1 < yLength)
        {
            antiNodes.Add(new Point(x1, y1));
            x1 = x1Func(x1);
            y1 = y1Func(y1);
        }

        while (x2 >= 0 && x2 < xLength && y2 >= 0 && y2 < yLength)
        {
            antiNodes.Add(new Point(x2, y2));
            x2 = x2Func(x2);
            y2 = y2Func(y2);
        }
    }

    private static void P1()
    {
        var lines = File.ReadAllLines("./data.aoc");
        var map = lines.GetMap<char>(out int xLength, out int yLength);

        var list = new List<(char name, Point point)>();
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                char c = map[x, y];
                if (c != '.')
                {
                    list.Add((c, new Point(x, y)));
                }
            }
        }

        var lookup = list.ToLookup(x => x.name, x => x.point);
        var antiNodes = new HashSet<Point>();

        foreach (var group in lookup)
        {
            var pointList = group.ToList();
            for (int i = 0; i < pointList.Count; i++)
            {
                for (int j = i + 1; j < pointList.Count; j++)
                {
                    AddAntiNodesP1(antiNodes, pointList[i], pointList[j], xLength, yLength);
                }
            }
        }

        Console.WriteLine(antiNodes.Count);
    }

    public static void AddAntiNodesP1(HashSet<Point> antiNodes, Point p1, Point p2, int xLength, int yLength)
    {
        long xDist = Math.Abs(p1.X - p2.X);
        long yDist = Math.Abs(p1.Y - p2.Y);

        long x1, y1, x2, y2;
        if (p1.X < p2.X)
        {
            x1 = p1.X - xDist;
            x2 = p2.X + xDist;
        }
        else
        {
            x1 = p1.X + xDist;
            x2 = p2.X - xDist;
        }

        if (p1.Y < p2.Y)
        {
            y1 = p1.Y - yDist;
            y2 = p2.Y + yDist;
        }
        else
        {
            y1 = p1.Y + yDist;
            y2 = p2.Y - yDist;
        }

        if (x1 >= 0 && x1 < xLength && y1 >= 0 && y1 < yLength)
        {
            antiNodes.Add(new Point(x1, y1));
        }

        if (x2 >= 0 && x2 < xLength && y2 >= 0 && y2 < yLength)
        {
            antiNodes.Add(new Point(x2, y2));
        }
    }
}