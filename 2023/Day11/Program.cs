await Problem1();
await Problem2();

static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    var map = new char[lines[0].Length, lines.Length];

    var yGalaxies = new HashSet<int>();
    var xGalaxies = new HashSet<int>();
    var galaxies = new List<(int X, int Y)>();

    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[0].Length; x++)
        {
            char c = lines[x][y];
            map[x, y] = c;

            if (c == '#')
            {
                yGalaxies.Add(y);
                xGalaxies.Add(x);
                galaxies.Add((x, y));
            }
        }
    }

    int count = 0;
    for (int i = 0; i < galaxies.Count; i++)
    {
        var a = galaxies[i];
        for (int j = i + 1; j < galaxies.Count; j++)
        {
            var b = galaxies[j];

            int yStart = Math.Min(a.Y, b.Y);
            int yEnd = Math.Max(a.Y, b.Y);
            int xStart = Math.Min(a.X, b.X);
            int xEnd = Math.Max(a.X, b.X);

            int dist = 0;
            for (int y = yStart; y < yEnd; y++)
            {
                dist++;
                if (!yGalaxies.Contains(y))
                {
                    dist++;
                }
            }

            for (int x = xStart; x < xEnd; x++)
            {
                dist++;
                if (!xGalaxies.Contains(x))
                {
                    dist++;
                }
            }

            count += dist;
        }
    }

    Console.WriteLine(count);
}

static async Task Problem2()
{
    long emptyDist = 1000000;
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    var map = new char[lines[0].Length, lines.Length];

    var yGalaxies = new HashSet<int>();
    var xGalaxies = new HashSet<int>();
    var galaxies = new List<(int X, int Y)>();

    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[0].Length; x++)
        {
            char c = lines[x][y];
            map[x, y] = c;

            if (c == '#')
            {
                yGalaxies.Add(y);
                xGalaxies.Add(x);
                galaxies.Add((x, y));
            }
        }
    }

    long count = 0;
    for (int i = 0; i < galaxies.Count; i++)
    {
        var a = galaxies[i];
        for (int j = i + 1; j < galaxies.Count; j++)
        {
            var b = galaxies[j];

            int yStart = Math.Min(a.Y, b.Y);
            int yEnd = Math.Max(a.Y, b.Y);
            int xStart = Math.Min(a.X, b.X);
            int xEnd = Math.Max(a.X, b.X);

            long dist = 0;
            for (int y = yStart; y < yEnd; y++)
            {
                if (!yGalaxies.Contains(y))
                {
                    dist += emptyDist;
                }
                else
                {
                    dist++;
                }
            }

            for (int x = xStart; x < xEnd; x++)
            {
                if (!xGalaxies.Contains(x))
                {
                    dist += emptyDist;
                }
                else
                {
                    dist++;
                }
            }

            count += dist;
        }
    }

    Console.WriteLine(count);
}
