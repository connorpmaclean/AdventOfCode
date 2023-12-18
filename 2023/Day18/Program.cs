
internal class Program
{
    private static async Task Main(string[] args)
    {
        checked
        {

            await Problem2();
        }
    }

    private static async Task Problem2()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        Point current = new Point(0, 0);
        var list = new List<Point>();
        long perim = 0;
        foreach (var line in lines)
        {
            var comp = line.Split(' ');

            string hex = comp[2];
            string distHex = hex.Substring(2, 5);
            char dir = hex[^2];

            long dist = Convert.ToInt64(distHex, 16);

            Console.WriteLine($"{dir} {dist}");
            current = dir switch
            {
                '0' => new Point(current.x + dist, current.y),
                '2' => new Point(current.x - dist, current.y),
                '1' => new Point(current.x, current.y + dist),
                '3' => new Point(current.x, current.y - dist),
            };

            list.Add(current);
            perim += dist;
        }

        // Shoelace
        long counter = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var p1 = list[i];
            var p2 = list[(i + 1) % list.Count];

            counter += p1.x * p2.y;
            counter -= p1.y * p2.x;
        }

        var doubleResult = (counter + perim);
        var result = doubleResult / 2;
        Console.WriteLine(result + 1);
            
    }

    private static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var covered = new HashSet<Point>();

        Point current = new Point(0, 0);
        covered.Add(current);
        foreach (var line in lines)
        {
            var comp = line.Split(' ');

            string dir = comp[0];
            int dist = int.Parse(comp[1]);

            Func<Point, Point> nextPoint = dir switch
            {
                "R" => (p) => new Point(p.x + 1, p.y),
                "L" => (p) => new Point(p.x - 1, p.y),
                "U" => (p) => new Point(p.x, p.y - 1),
                "D" => (p) => new Point(p.x, p.y + 1),
            };

            for (int i = 0; i < dist; i++)
            {
                current = nextPoint(current);
                covered.Add(current);
            }
        }

        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (covered.Contains(new Point(x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }

        var queue = new Queue<Point>();
        queue.Enqueue(new Point(1, 1));
        while (queue.TryDequeue(out var point))
        {
            if (covered.Contains(point))
            {
                continue;
            }

            covered.Add(point);

            queue.Enqueue(new Point(point.x + 1, point.y));
            queue.Enqueue(new Point(point.x - 1, point.y));
            queue.Enqueue(new Point(point.x, point.y + 1));
            queue.Enqueue(new Point(point.x, point.y - 1));
        }

        Console.WriteLine(covered.Count);
    }

    public record struct Point(long x, long y)
    {

    }
}