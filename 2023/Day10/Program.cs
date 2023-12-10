
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static System.Collections.Specialized.BitVector32;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2Solution();
    }

    private static async Task Problem2Solution()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        char[,] map = new char[lines[0].Length, lines.Length];
        char[,] largeMap = new char[lines[0].Length * 2, lines.Length * 2];
        Point start = new Point(-1, -1);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                map[j, i] = lines[i][j];

                if (map[j, i] == 'S')
                {
                    start.X = j;
                    start.Y = i;
                }
            }
        }

        // Find start directions.
        var startingDirs = new List<Direction>()
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
        }
        .Where(dir => CanGoDirection(start, dir))
        .Where(dir => GetNextDir(map, NextCord(start, dir), dir) != Direction.Impossible)
        .ToArray();


        Point pointA = start;
        Direction dirA = startingDirs[0];

        while (true)
        {
            largeMap[pointA.X * 2, pointA.Y * 2] = 'X';

            switch (dirA)
            {
                case Direction.North:
                    largeMap[pointA.X * 2, pointA.Y * 2 - 1] = 'X';
                    break;
                case Direction.East:
                    largeMap[pointA.X * 2 + 1, pointA.Y * 2] = 'X';
                    break;
                case Direction.West:
                    largeMap[pointA.X * 2 - 1, pointA.Y * 2] = 'X';
                    break;
                case Direction.South:
                    largeMap[pointA.X * 2, pointA.Y * 2 + 1] = 'X';
                    break;
            }

            pointA = NextCord(pointA, dirA);
            dirA = GetNextDir(map, pointA, dirA);

            if (dirA == Direction.Impossible)
            {
                break;
            }
        }

        var queue = new Queue<Point>();
        queue.Enqueue(new Point(start.X * 2 - 1, start.Y * 2 + 1)); // Need to customize this per solution
        var countedPoints = new HashSet<Point>();
        long areaCount = 0;

        while (queue.TryDequeue(out var point))
        {
            if (countedPoints.Contains(point))
            {
                continue;
            }

            if (point.X < 0 || point.X >= lines[0].Length * 2 || point.Y < 0 || point.Y >= lines.Length * 2)
            {
                continue;
            }

            if (largeMap[point.X, point.Y] == 'X')
            {
                continue;
            }

            areaCount++;
            countedPoints.Add(point);
            largeMap[point.X, point.Y] = 'I';
            queue.Enqueue(new Point(point.X + 1, point.Y));
            queue.Enqueue(new Point(point.X - 1, point.Y));
            queue.Enqueue(new Point(point.X, point.Y + 1));
            queue.Enqueue(new Point(point.X, point.Y - 1));
        }

        for (int y = 0; y < lines.Length * 2; y++)
        {
            for (int x = 0; x < lines[0].Length * 2; x++)
            {
                char c = largeMap[x, y];
                c = c == default ? '.' : c;
                Console.Write(c);
            }

            Console.WriteLine();
        }

        long count = 0;
        for (int y = 0; y < lines.Length * 2; y += 2)
        {
            for (int x = 0; x < lines[0].Length * 2; x += 2)
            {
                if (largeMap[x, y] == 'I')
                {
                    count++;
                }
            }
        }

        Console.WriteLine(count);

    }

    private static async Task Problem2Attempt1()
    {
        var lines = await File.ReadAllLinesAsync("./Sample3.aoc");

        char[,] map = new char[lines[0].Length, lines.Length];
        char[,] xInside = new char[lines[0].Length, lines.Length];
        char[,] yInside = new char[lines[0].Length, lines.Length];
        Point start = new Point(-1, -1);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                map[j, i] = lines[i][j];

                if (map[j, i] == 'S')
                {
                    start.X = j;
                    start.Y = i;
                }
            }
        }

        // Find start directions.
        var startingDirs = new List<Direction>()
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
        }
        .Where(dir => CanGoDirection(start, dir))
        .Where(dir => GetNextDir(map, NextCord(start, dir), dir) != Direction.Impossible)
        .ToArray();

        char startPipe = GetStartPipe(startingDirs);
        map[start.X, start.Y] = startPipe;

        for (int y = 0; y < lines.Length; y++)
        {
            List<List<int>> sections = new();
            sections.Add(new List<int>());
            bool inHorizontal = false;
            for (int x = 0; x < lines[0].Length; x++)
            {
                char c = map[x, y];
                if (inHorizontal)
                {
                    if (IsJoint(c))
                    {
                        inHorizontal = false;
                    }
                }
                else if (c == '|')
                {
                    sections.Add(new List<int>());
                }
                else if (IsJoint(c))
                {
                    sections.Add(new List<int>());
                    inHorizontal = true;
                }
                else if (c == '.')
                {
                    sections.Last().Add(x);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                xInside[x, y] = c;
            }

            if (sections.Count > 2 && sections.Count % 2 == 1)
            {
                for (int i = 1; i < sections.Count; i += 2)
                {
                    foreach (int x in sections[i])
                    {
                        xInside[x, y] = 'I';
                    }
                }
            }
            else if (sections.Count > 2)
            {
                for (int i = sections.Count / 2; i < sections.Count; i += 2)
                {
                    foreach (int x in sections[i])
                    {
                        xInside[x, y] = 'I';
                    }
                }

                for (int i = sections.Count / 2 - 1; i >= 0; i -= 2)
                {
                    foreach (int x in sections[i])
                    {
                        xInside[x, y] = 'I';
                    }
                }
            }

            
        }

        Console.WriteLine();
        int count = 0;
        for (int x = 0; x < lines[0].Length; x++)
        {
            List<List<int>> sections = new();
            sections.Add(new List<int>());
            bool inVertical = false;
            for (int y = 0; y < lines.Length; y++)
            {
                char c = map[x, y];
                if (inVertical)
                {
                    if (IsJoint(c))
                    {
                        inVertical = false;
                    }
                }
                else if (c == '-')
                {
                    sections.Add(new List<int>());
                }
                else if (IsJoint(c))
                {
                    sections.Add(new List<int>());
                    inVertical = true;
                }
                else if (c == '.')
                {
                    sections.Last().Add(y);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                yInside[x, y] = c;
            }

            if (sections.Count > 2 && sections.Count % 2 == 1)
            {
                for (int i = 1; i < sections.Count; i += 2)
                {
                    foreach (int y in sections[i])
                    {
                        yInside[x, y] = 'I';
                        if (xInside[x, y] == 'I')
                        {
                            count++;
                        }
                    }
                }
            }
            else if (sections.Count > 2)
            {
                for (int i = sections.Count / 2; i < sections.Count; i += 2)
                {
                    foreach (int y in sections[i])
                    {
                        yInside[x, y] = 'I';
                        if (xInside[x, y] == 'I')
                        {
                            count++;
                        }
                    }
                }

                for (int i = sections.Count / 2 - 1; i >= 0; i -= 2)
                {
                    foreach (int y in sections[i])
                    {
                        yInside[x, y] = 'I';
                        if (xInside[x, y] == 'I')
                        {
                            count++;
                        }
                    }
                }
            }
        }

        Console.WriteLine();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                char c = xInside[x, y];
                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                char c = yInside[x, y];
                Console.Write(c);
            }

            Console.WriteLine();
        }

        Console.WriteLine(count);

        static bool IsJoint(char c) => c switch
        {
            'F' => true,
            'J' => true,
            '7' => true,
            'L' => true,
            _ => false
        };
    }

    private static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        char[,] map = new char[lines.Length, lines.Length];
        Point start = new Point(-1, -1);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                map[j, i] = lines[i][j];

                if (map[j, i] == 'S')
                {
                    start.X = j;
                    start.Y = i;
                }
            }
        }

        // Find start directions.
        var startingDirs = new List<Direction>()
        {
            Direction.North,
            Direction.East,
            Direction.South,
            Direction.West,
        }
        .Where(dir => CanGoDirection(start, dir))
        .Where(dir => GetNextDir(map, NextCord(start, dir), dir) != Direction.Impossible)
        .ToArray();


        Point pointA = start;
        Point pointB = start;
        //Direction dirA = startingDirs[0];
        //Direction dirB = startingDirs[1];

        // Start direction is broken ): hack them in
        Direction dirA = Direction.North;
        Direction dirB = Direction.West;

        long count = 0;
        while (true)
        {
            count++;

            pointA = NextCord(pointA, dirA);
            dirA = GetNextDir(map, pointA, dirA);
            Console.WriteLine($"A: {pointA}, {dirA}");

            if (pointA == pointB)
            {
                break;
            }

            pointB = NextCord(pointB, dirB);
            dirB = GetNextDir(map, pointB, dirB);
            Console.WriteLine($"B: {pointB}, {dirB}");

            if (pointA == pointB)
            {
                break;
            }
        }

        Console.WriteLine(count);
    }

    public record struct Point
    {
        public Point(int x, int y)
        {
            X = x; Y = y;
        }

        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
    }

    public static Point NextCord(Point cur, Direction dir) => dir switch
    {
        Direction.North => new Point(cur.X, cur.Y - 1),
        Direction.East => new Point(cur.X + 1, cur.Y),
        Direction.South => new Point(cur.X, cur.Y + 1),
        Direction.West => new Point(cur.X - 1, cur.Y),
        Direction.Impossible => throw new InvalidOperationException()
    };

    public static  Direction GetNextDir(char[,] map, Point point, Direction curr)
    {
        char c = map[point.X, point.Y];
        var nextDir = NextDir(c, curr);

        if (nextDir == Direction.Impossible)
        {
            Console.WriteLine("Impossible detected");
            Console.WriteLine(point);
            Console.WriteLine($"Current direction: {curr}, Current symbol {c}");
            Console.WriteLine();
        }

        return nextDir;
    }

    public static Direction NextDir(char pipe, Direction curr) => (pipe, curr) switch
    {
        ('|', Direction.South) => curr,
        ('|', Direction.North) => curr,
        ('-', Direction.East) => curr,
        ('-', Direction.West) => curr,
        ('L', Direction.West) => Direction.North,
        ('L', Direction.South) => Direction.East,
        ('J', Direction.East) => Direction.North,
        ('J', Direction.South) => Direction.West,
        ('7', Direction.East) => Direction.South,
        ('7', Direction.North) => Direction.West,
        ('F', Direction.West) => Direction.South,
        ('F', Direction.North) => Direction.East,
        _ => Direction.Impossible
    };

    public static bool CanGoDirection(Point curr, Direction dir) => (dir, curr.X, curr.Y) switch
    {
        (Direction.West, 0, _) => false,
        (Direction.North, _, 0) => false,
        _ => true
    };

    public static char GetStartPipe(IEnumerable<Direction> dirs) => dirs.OrderBy(d => d).ToList() switch
    {
        [Direction.North, Direction.South] => '|',
        [Direction.North, Direction.East] => 'L',
        [Direction.North, Direction.West] => 'J',
        [Direction.South, Direction.East] => 'F',
        [Direction.South, Direction.West] => '7',
        [Direction.East, Direction.West] => '-',
    };

    public enum Direction
    {
        Impossible,
        North,
        South,
        East,
        West,
    }
}