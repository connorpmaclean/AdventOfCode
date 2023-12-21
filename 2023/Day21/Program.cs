using AocHelper;
using System.ComponentModel;

await Problem2();


static async Task Problem2()
{
    var lines = await File.ReadAllLinesAsync("Sample.aoc");

    var map = lines.GetMap<char>(out int xLength, out int yLength);

    //int expXLength = xLength * 3;
    //int expYLength = yLength * 3;
    //var expMap = new char[expXLength, expYLength];
    //for (int x = 0; x < expXLength; x++)
    //{
    //    for (int y = 0; y < expYLength; y++)
    //    {
    //        expMap[x, y] = map[x % xLength, y % yLength];
    //    }
    //}

    //map = expMap;
    //xLength = expXLength;
    //yLength = expYLength;

    Point start = new Point(0, 0);
    for (int x = 0; x < xLength; x++)
    {
        for (int y = 0; y < yLength; y++)
        {
            if (map[x, y] == 'S')
            {
                start = new Point(x, y);
            }
        }
    }



    var result = SolveMap(map, new (Point, long)[] { (start, 0) }, 6);
    Console.WriteLine(result.even);

    //var initial = current;
    //long sum = count;

    //// Down
    //Console.WriteLine();
    //for (int i = 1; i < 10; i++)
    //{
    //    var nextStarts = current.Where(kvp => kvp.Key.Y == yLength - 1 && kvp.Value >= 0).Select(kvp => (new Point(kvp.Key.X, 0), kvp.Value + 1)).ToList();
    //    current = SolveMap(map, nextStarts, xLength, yLength);
    //    count = current.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();

    //    sum += 8 * i * count;
    //    Console.WriteLine($"{i + 1}: Count: {count}, min start {(nextStarts.Any() ? nextStarts.Min(x => x.Item2) : "")}, max start {(nextStarts.Any() ? nextStarts.Max(x => x.Item2) : "")}");
    //}

    //Console.WriteLine(sum);
    //current = initial;

    // Up
    //Console.WriteLine();
    Console.WriteLine();
    //for (int i = 0; i < 10; i++)
    //{
    //    var nextStarts = current.Where(kvp => kvp.Key.X == xLength - 1 && kvp.Value >= 0).Select(kvp => (new Point(0, kvp.Key.Y), kvp.Value + 1)).ToList();
    //    current = SolveMap(map, nextStarts, xLength, yLength);

    //    count = current.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();

    //    sum += 8 * (i - 1) * count;
    //    Console.WriteLine($"{i + 1}: Count: {count}, min start {(nextStarts.Any() ? nextStarts.Min(x => x.Item2) : "")}, max start {(nextStarts.Any() ? nextStarts.Max(x => x.Item2) : "")}");
    //}

    //Console.WriteLine(sum);

    //// Right
    //Console.WriteLine();
    //for (int i = 0; i < 10; i++)
    //{
    //    var nextStarts = current.Where(kvp => kvp.Key.X == 0).Select(kvp => (new Point(xLength -1, kvp.Key.Y), kvp.Value + 1)).ToList();
    //    current = SolveMap(map, nextStarts, xLength, yLength);
    //    count = current.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();
    //    Console.WriteLine(count);
    //}



}

//static void SolveInDir(char[,] map, Dictionary<Point, long> current, int xLength, int yLength, ref long sum)
//{


//    Console.WriteLine();
//    var nextStarts = current.Where(kvp => kvp.Key.Y == yLength - 1 && kvp.Value >= 0).Select(kvp => (new Point(kvp.Key.X, 0), kvp.Value + 1)).ToList();
//    current = SolveMap(map, nextStarts, xLength, yLength);
//    long count = current.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();

//    sum += i * count;
//    Console.WriteLine($"{i + 1}: Count: {count}, min start {(nextStarts.Any() ? nextStarts.Min(x => x.Item2) : "")}, max start {(nextStarts.Any() ? nextStarts.Max(x => x.Item2) : "")}");
//}

static (long even, long odd) SolveMap(char[,] map, IList<(Point, long)> starts, long maxDist)
{
    PriorityQueue<Point, long> queue = new();
    Dictionary<Point, long> visited = new Dictionary<Point, long>();

    foreach (var item in starts)
    {
        queue.Enqueue(item.Item1, item.Item2);
    }

    while (queue.TryDequeue(out var point, out var dist))
    {
        if (visited.ContainsKey(point))
        {
            continue;
        }

        if (point.X < 0 || point.Y < 0 || point.X >= map.GetLength(0) || point.Y >= map.GetLength(0))
        {
            visited.Add(point, -1);
            continue;
        }

        if (map[point.X, point.Y] == '#')
        {
            visited.Add(point, -1);
            continue;
        }

        if (dist > maxDist)
        {
            visited.Add(point, -1);
            continue;
        }

        visited.Add(point, dist);
        long nextDist = dist + 1;

        queue.Enqueue((new Point(point.X + 1, point.Y)), nextDist);
        queue.Enqueue((new Point(point.X - 1, point.Y)), nextDist);
        queue.Enqueue((new Point(point.X, point.Y + 1)), nextDist);
        queue.Enqueue((new Point(point.X, point.Y - 1)), nextDist);
    }

    long even = visited.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();
    long odd = visited.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 1).Count();

    return (even, odd);
}


static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("Data.aoc");

    var map = lines.GetMap<char>(out int xLength, out int yLength);

    Point start = new Point(0, 0);
    for (int x = 0; x < xLength; x++)
    {
        for (int y = 0; y < yLength; y++)
        {
            if (map[x, y] == 'S')
            {
                start = new Point(x, y);
            }
        }
    }



    Queue<(Point, int)> queue = new();
    Dictionary<Point, int> visited = new Dictionary<Point, int>();

    queue.Enqueue((start, 0));

    while (queue.TryDequeue(out var next))
    {
        (Point point, int dist) = next;
        if (visited.ContainsKey(point))
        {
            continue;
        }

        if (point.X < 0 || point.Y < 0 || point.X >= xLength || point.Y >= yLength)
        {
            visited.Add(point, -1);
            continue;
        }

        if (map[point.X, point.Y] == '#')
        {
            visited.Add(point, -1);
            continue;
        }

        if (dist > 64)
        {
            visited.Add(point, -1);
            continue;
        }

        visited.Add(point, dist);
        int nextDist = dist + 1;

        queue.Enqueue((new Point(point.X + 1, point.Y), nextDist));
        queue.Enqueue((new Point(point.X - 1, point.Y), nextDist));
        queue.Enqueue((new Point(point.X, point.Y + 1), nextDist));
        queue.Enqueue((new Point(point.X, point.Y - 1), nextDist));
    }

    long count = visited.Where(kvp => kvp.Value >= 0 && kvp.Value % 2 == 0).Count();
    Console.WriteLine(count);
}
