
internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2();
    }

    private static async Task Problem2()
    {
        string[] lines = await File.ReadAllLinesAsync("Data.aoc");

        int yLength = lines.Length;
        int xLength = lines[0].Length;
        var map = new char[lines[0].Length, lines.Length];

        for (int x = 0; x < lines[0].Length; x++)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                map[x, y] = lines[y][x];
            }
        }

        int best = 0;
        for (int x = 0; x < xLength; x++)
        {
            var result = new HashSet<Point>();
            FollowBeam(map, Direction.South, x, 0, result, xLength, yLength, new HashSet<(Direction, Point)>());
            best = Math.Max(best, result.Count);

            result = new HashSet<Point>();
            FollowBeam(map, Direction.North, x, yLength - 1, result, xLength, yLength, new HashSet<(Direction, Point)>());
            best = Math.Max(best, result.Count);
        }

        for (int y = 0; y < yLength; y++)
        {
            var result = new HashSet<Point>();
            FollowBeam(map, Direction.East, 0, y, result, xLength, yLength, new HashSet<(Direction, Point)>());
            best = Math.Max(best, result.Count);

            result = new HashSet<Point>();
            FollowBeam(map, Direction.West, xLength - 1, y, result, xLength, yLength, new HashSet<(Direction, Point)>());
            best = Math.Max(best, result.Count);
        }

        Console.WriteLine(best);
    }

    private static async Task Problem1()
    {
        string[] lines = await File.ReadAllLinesAsync("Data.aoc");

        int yLength = lines.Length;
        int xLength = lines[0].Length;
        var map = new char[lines[0].Length, lines.Length];

        for (int x = 0; x < lines[0].Length; x++)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                map[x, y] = lines[y][x];
            }
        }

        var result = new HashSet<Point>();
        FollowBeam(map, Direction.East, 0, 0, result, xLength, yLength, new HashSet<(Direction, Point)>());
        Console.WriteLine(result.Count);
    }

    public static void FollowBeam(char[,] map, Direction dir, int x, int y, HashSet<Point> covered, int xLength, int yLength, HashSet<(Direction, Point)> seen)
    {
        while (true)
        {
            if (x < 0 || x >= xLength || y < 0 || y >= yLength)
            {
                return;
            }

            Point p = new Point(x, y);
            if (seen.Contains((dir, p)))
            {
                return;
            }
            else
            {
                seen.Add((dir, p));
            }

            covered.Add(new Point(x, y));

            char c = map[x, y];

            if (c == '/')
            {
                dir = dir switch
                {
                    Direction.East => Direction.North,
                    Direction.West => Direction.South,
                    Direction.South => Direction.West,
                    Direction.North => Direction.East,
                };
            }
            else if (c == '\\')
            {
                dir = dir switch
                {
                    Direction.East => Direction.South,
                    Direction.West => Direction.North,
                    Direction.South => Direction.East,
                    Direction.North => Direction.West,
                };
            }
            else if (c == '-')
            {
                if (dir == Direction.North || dir == Direction.South)
                {
                    FollowBeam(map, Direction.East, x + 1, y, covered, xLength, yLength, seen);
                    dir = Direction.West;
                }
            }
            else if (c == '|')
            {
                if (dir == Direction.West || dir == Direction.East)
                {
                    FollowBeam(map, Direction.South, x, y + 1, covered, xLength, yLength, seen);
                    dir = Direction.North;
                }
            }

            x = dir switch
            {
                Direction.East => x + 1,
                Direction.West => x - 1,
                _ => x
            };

            y = dir switch
            {
                Direction.South => y + 1,
                Direction.North => y - 1,
                _ => y
            };
        }
        
    }

    public record Point(int x, int y);

    public enum Direction
    {
        North,
        South,
        East,
        West
    }
}