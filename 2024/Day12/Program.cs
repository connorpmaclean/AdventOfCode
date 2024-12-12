using AocHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        P2();
    }

    private static void P2()
    {
        var map = File.ReadAllLines("./data.aoc").GetMap<char>(out int xLength, out int yLength);

        var visited = new HashSet<Point>();
        long price = 0;
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (visited.Contains(new Point(x, y)))
                {
                    continue;
                }

                char c = map[x, y];
                ResultP1? result = ScanP2(visited, map, xLength, yLength, x, y, c);
                long value = result.Area * result.Perimeter;
                //Console.WriteLine($"{c}: {result.Area} x {result.Perimeter} = {value}");
                price += value;
            }
        }

        Console.WriteLine(price);
    }

    public static ResultP1? ScanP2(HashSet<Point> visited, char[,] map, int xLength, int yLength, int x, int y, char target)
    {
        if (x < 0 || x >= xLength || y < 0 || y >= yLength)
        {
            return null;
        }

        if (map[x, y] != target)
        {
            return null;
        }

        if (visited.Contains(new Point(x, y)))
        {
            return new ResultP1(0, 0);
        }

        // Need to do this before visiting others.
        visited.Add(new Point(x, y));

        var n = ScanP1(visited, map, xLength, yLength, x, y - 1, target);
        var e = ScanP1(visited, map, xLength, yLength, x + 1, y, target);
        var s = ScanP1(visited, map, xLength, yLength, x, y + 1, target);
        var w = ScanP1(visited, map, xLength, yLength, x - 1, y, target);

        var cardinalDirs = new List<ResultP1?>()
        {
            n, s, e, w
        };

        int localArea = 1;
        int totalArea = cardinalDirs.Sum(d => d?.Area ?? 0) + localArea;

        return new ResultP2(totalPerim, totalArea);
    }

    //public int GetCornerCount(char[,] map, int xLength, int yLength, int x, int y, char target)
    //{
    //    var n = IsMatchingChar(map, xLength, yLength, x, y - 1, target);
    //    var ne = IsMatchingChar(map, xLength, yLength, x + 1, y - 1, target);
    //    var e = IsMatchingChar(map, xLength, yLength, x + 1, y, target);
    //    var se = IsMatchingChar(map, xLength, yLength, x + 1, y + 1, target);
    //    var s = IsMatchingChar(map, xLength, yLength, x, y + 1, target);
    //    var sw = IsMatchingChar(map, xLength, yLength, x - 1, y + 1, target);
    //    var w = IsMatchingChar(map, xLength, yLength, x - 1, y, target);
    //    var nw = IsMatchingChar(map, xLength, yLength, x - 1, y - 1, target);

    //    var cardinals = new bool[] { n, e, s, w };
    //    var diagonals = new bool[] { ne, se, sw, nw};
    //    var cardinalCount = cardinals.Count(b => b);
    //    var diagonalCount = diagonalCount.Count(b => b);

    //    // Interior
    //    if (cardinalCount == 4)
    //    {
    //        return 4 - diagonalCount;
    //    }

    //    // Inner corner
    //    if (count == 7)
    //    {
    //        return 1;
    //    }
    //}

    //public bool IsMatchingChar(char[,] map, int xLength, int yLength, int x, int y, char target)
    //{
    //    if (x < 0 || x >= xLength || y < 0 || y >= yLength)
    //    {
    //        return false;
    //    }

    //    return map[x, y] == target;
    //}

    //public static ResultP1? ScanP2(HashSet<Point> visited, char[,] map, int xLength, int yLength, int x, int y, char target)
    //{
    //    if (x < 0 || x >= xLength || y < 0 || y >= yLength)
    //    {
    //        return null;
    //    }

    //    if (map[x, y] != target)
    //    {
    //        return null;
    //    }

    //    if (visited.Contains(new Point(x, y)))
    //    {
    //        return new ResultP1(0, 0);
    //    }

    //    // Need to do this before visiting others.
    //    visited.Add(new Point(x, y));

    //    var n = ScanP1(visited, map, xLength, yLength, x, y - 1, target);
    //    var ne = ScanP1(visited, map, xLength, yLength, x + 1, y - 1, target);
    //    var e = ScanP1(visited, map, xLength, yLength, x + 1, y, target);
    //    var se = ScanP1(visited, map, xLength, yLength, x + 1, y + 1, target);
    //    var s = ScanP1(visited, map, xLength, yLength, x, y + 1, target);
    //    var sw = ScanP1(visited, map, xLength, yLength, x - 1, y + 1, target);
    //    var w = ScanP1(visited, map, xLength, yLength, x - 1, y, target);
    //    var nw = ScanP1(visited, map, xLength, yLength, x - 1, y - 1, target);

    //    int isCorner = (n, ne, e, se, s, sw, w, nw) switch
    //    {
    //        // Interior
    //        (not null, _, not null, _, not null, _, not null, _) => 0,

    //        // Straights
    //        (null, _, not null, _, null, _, not null, _) => 0,
    //        (not null, _, null, _, not null, _, null, _) => 0,

    //        // Outer
    //        (not null, not null, not null, _, _, _, _, _) => 1,
    //        (_, _, not null, not null, not null, _, _, _) => 1,
    //        (_, _, _, _, not null, not null, not null, _) => 1,
    //        (not null, _, _, _, _, _, not null, not null) => 1,

    //        // L shapes
    //        (not null, _, not null, _, _, _, _, _) => 2,
    //        (_, _, not null, _, not null, _, _, _) => 2,
    //        (_, _, _, _, not null, _, not null, _) => 2,
    //        (not null, _, _, _, _, _, not null, _) => 2,
    //    };

    //var cardinalDirs = new List<ResultP1?>()
    //    {
    //        n, s, e, w
    //    };

    //    int localArea = 1;
    //    int totalArea = areaDirs.Sum(d => d?.Area ?? 0) + localArea;

    //    return new ResultP2(totalPerim, totalArea);
    //}

    private static void P1()
    {
        var map = File.ReadAllLines("./data.aoc").GetMap<char>(out int xLength, out int yLength);

        var visited = new HashSet<Point>();
        long price = 0;
        for (int y = 0; y < yLength; y++)
        {
            for (int x = 0; x < xLength; x++)
            {
                if (visited.Contains(new Point(x, y)))
                {
                    continue;
                }

                char c = map[x, y];
                ResultP1? result = ScanP1(visited, map, xLength, yLength, x, y, c);
                long value = result.Area * result.Perimeter;
                //Console.WriteLine($"{c}: {result.Area} x {result.Perimeter} = {value}");
                price += value;
            }
        }

        Console.WriteLine(price);
    }

    public static ResultP1? ScanP1(HashSet<Point> visited, char[,] map, int xLength, int yLength, int x, int y, char target)
    {
        if (x < 0 || x >= xLength || y < 0 || y >= yLength)
        {
            return null;
        }

        if (map[x, y] != target)
        {
            return null;
        }

        if (visited.Contains(new Point(x, y)))
        {
            return new ResultP1(0, 0);
        }

        // Need to do this before visiting others.
        visited.Add(new Point(x, y));

        var dirs = new List<ResultP1?>()
        {
            ScanP1(visited, map, xLength, yLength, x + 1, y, target),
            ScanP1(visited, map, xLength, yLength, x - 1, y, target),
            ScanP1(visited, map, xLength, yLength, x, y + 1, target),
            ScanP1(visited, map, xLength, yLength, x, y - 1, target),
        };

        int localPerim = 4 - dirs.Where(d => d != null).Count();
        int localArea = 1;

        int totalPerim = dirs.Sum(d => d?.Perimeter ?? 0) + localPerim;
        int totalArea = dirs.Sum(d => d?.Area ?? 0) + localArea;
        return new ResultP1(totalPerim, totalArea);
    }

    public record ResultP1(int Perimeter, int Area)
    {
        
    }

    public record ResultP2(int Corners, int Area)
    {

    }
}