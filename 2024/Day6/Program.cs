using System.ComponentModel.Design;
using AocHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        P2();
    }

    //619 too low
    private static void P2()
    {
        var lines = File.ReadAllLines("./sample.aoc");
        var map = lines.GetMap<char>(out int xLength, out int yLength);

        int x = -1, y = -1;
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                if (map[i, j] == '^')
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }

        Direction dir = Direction.North;
        var marked = new HashSet<PathPoint>();
        var obs = new HashSet<Point>();
        while (true)
        {
            marked.Add(new PathPoint(x, y, dir));

            if (x == 2 && y == 8)
            {
                Console.WriteLine();
            }

            (int nextX, int nextY) = GetNext(x, y, dir);
            if (nextX >= xLength || nextX < 0 || nextY >= yLength || nextY < 0)
            {
                break;
            }

            if (map[nextX, nextY] == '#')
            {
                while (map[nextX, nextY] == '#')
                {
                    dir = Rotate(dir);
                    (nextX, nextY) = GetNext(x, y, dir);
                }
            }
            else
            {
                Direction hypDir = Rotate(dir);
                (int hypX, int hypY) = GetNext(x, y, hypDir);
                while (map[hypX, hypY] == '#')
                {
                    hypDir = Rotate(hypDir);
                    (hypX, hypY) = GetNext(x, y, hypDir);
                }

                var markedCopy = new HashSet<PathPoint>(marked);
                if (marked.Contains(new PathPoint(x, y, hypDir))
                    || DetectLoop(map, xLength, yLength, hypDir, hypX, hypY, markedCopy))
                {
                    obs.Add(new Point(nextX, nextY));
                }
            }

            x = nextX; y = nextY;
        }
        

        foreach (var obs1 in obs)
        {
            Console.WriteLine(obs1);
        }

        Console.WriteLine(obs.Count);
    }

    private static bool DetectLoop(char[,] map, int xLength, int yLength, Direction dir, int x, int y, HashSet<PathPoint> marked)
    {
        while (true)
        {
            var p = new PathPoint(x, y, dir);
            if (marked.Contains(p))
            {
                return true;
            }

            marked.Add(p);

            (int nextX, int nextY) = GetNext(x, y, dir);
            if (nextX >= xLength || nextX < 0 || nextY >= yLength || nextY < 0)
            {
                return false;
            }

            x = nextX; y = nextY;
        }
    }

    private static void P1()
    {
        var lines = File.ReadAllLines("./data.aoc");
        var map = lines.GetMap<char>(out int xLength, out int yLength);

        int x = -1, y = -1;
        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < yLength; j++)
            {
                if (map[i, j] == '^')
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }

        Direction dir = Direction.North;
        var marked = new HashSet<Point>();
        while (true)
        {
            marked.Add(new Point(x, y));

            (int nextX, int nextY) = GetNext(x, y, dir);
            if (nextX >= xLength || nextX < 0 || nextY >= yLength || nextY < 0)
            {
                break;
            }

            while (map[nextX, nextY] == '#')
            {
                dir = Rotate(dir);
                (nextX, nextY) = GetNext(x, y, dir);
            }

            x = nextX; y = nextY;
        }

        Console.WriteLine(marked.Count);
    }

    public static (int x, int y) GetNext(int x, int y, Direction dir)
    {
        return dir switch
        {
            Direction.North => (x, y - 1),
            Direction.South => (x, y + 1),
            Direction.East => (x + 1, y),
            Direction.West => (x - 1, y),
        };
    }

    public static Direction Rotate(Direction dir) => dir switch
    {
        Direction.North => Direction.East,
        Direction.East => Direction.South,
        Direction.South => Direction.West,
        Direction.West => Direction.North,
    };

    public record PathPoint(int x, int y, Direction dir);
}