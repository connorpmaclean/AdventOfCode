using AocHelper;

internal class Program
{
    public record Route(Point Start)
    {
        public Point? End {  get; set; }

        public HashSet<Point> Points = new HashSet<Point>() { Start };
    }

    private static async Task Main(string[] args)
    {
        await Problem2();


        
    }

    static bool CanMoveP1(char[,] map, Point current, Point next)
    {
        if (next.X < 0 || next.Y < 0)
        {
            return false;
        }

        var c = map[next.X, next.Y];
        if (c == '#')
        {
            return false;
        }

        if (c == '.')
        {
            return true;
        }

        if (c == '>' && current.X + 1 == next.X
            || c == '^' && current.Y - 1 == next.Y
            || c == '<' && current.X - 1 == next.X
            || c == 'v' && current.Y + 1 == next.Y)
        {
            return true;
        }

        return false;
    }

    static bool CanMoveP2(char[,] map, Point next, long xLength, long yLength)
    {
        if (next.X < 0 || next.Y < 0 || next.X >= xLength || next.Y >= yLength)
        {
            return false;
        }

        var c = map[next.X, next.Y];
        if (c == '#')
        {
            return false;
        }

        return true;
    }

    static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var map = lines.GetMap<char>(out int xLength, out int yLength);

        Point start = default;
        Point end = default;
        for (int x = 0; x < xLength; x++)
        {
            if (map[x, 0] == '.')
            {
                start = new Point(x, 0);
            }

            if (map[x, yLength - 1] == '.')
            {
                end = new Point(x, yLength - 1);
            }
        }

        var queue = new Queue<(Point, HashSet<Point>)>();
        queue.Enqueue((start, new HashSet<Point>()));
        long longest = 0;
        while (queue.TryDequeue(out var dequeued))
        {
            (Point current, HashSet<Point> history) = dequeued;
            history.Add(current);

            if (current == end)
            {
                longest = Math.Max(longest, history.Count);
                continue;
            }

            var right = new Point(current.X + 1, current.Y);
            if (CanMoveP1(map, current, right) && !history.Contains(right))
            {
                history = new HashSet<Point>(history);
                queue.Enqueue((right, history));
            }

            var left = new Point(current.X - 1, current.Y);
            if (CanMoveP1(map, current, left) && !history.Contains(left))
            {
                history = new HashSet<Point>(history);
                queue.Enqueue((left, history));
            }

            var up = new Point(current.X, current.Y - 1);
            if (CanMoveP1(map, current, up) && !history.Contains(up))
            {
                history = new HashSet<Point>(history);
                queue.Enqueue((up, history));
            }

            var down = new Point(current.X, current.Y + 1);
            if (CanMoveP1(map, current, down) && !history.Contains(down))
            {
                history = new HashSet<Point>(history);
                queue.Enqueue((down, history));
            }
        }

        Console.WriteLine(longest - 1);
    }

    static async Task Problem2()
    {
        var lines = await File.ReadAllLinesAsync("Sample.aoc");

        var map = lines.GetMap<char>(out int xLength, out int yLength);

        Point start = default;
        Point end = default;
        for (int x = 0; x < xLength; x++)
        {
            if (map[x, 0] == '.')
            {
                start = new Point(x, 0);
            }

            if (map[x, yLength - 1] == '.')
            {
                end = new Point(x, yLength - 1);
            }
        }

        var visited = new HashSet<Point>();
        var allRoutes = new List<Route>();

        var queue = new Queue<(Point, Route)>();
        queue.Enqueue((start, new Route(start)));
        long longest = 0;
        while (queue.TryDequeue(out var next))
        {
            (Point current, Route route) = next;
            allRoutes.Add(route);
            //if (current == end)
            //{
            //    longest = Math.Max(longest, history.Count);
            //    continue;
            //}

            while (true)
            {
                route.Points.Add(current);
                visited.Add(current);

                var moves = new List<Point>(4);
                var right = new Point(current.X + 1, current.Y);
                if (CanMoveP2(map, right, xLength, yLength) && !route.Points.Contains(right))
                {
                    moves.Add(right);
                }

                var left = new Point(current.X - 1, current.Y);
                if (CanMoveP2(map, left, xLength, yLength) && !route.Points.Contains(left))
                {
                    moves.Add(left);
                }

                var up = new Point(current.X, current.Y - 1);
                if (CanMoveP2(map, up, xLength, yLength) && !route.Points.Contains(up))
                {
                    moves.Add(up);
                }

                var down = new Point(current.X, current.Y + 1);
                if (CanMoveP2(map, down, xLength, yLength) && !route.Points.Contains(down))
                {
                    moves.Add(down);
                }

                if (moves.Count >= 2 || current == end)
                {
                    route.End = current;
                    foreach (var move in moves)
                    {
                        if (!visited.Contains(move))
                        {
                            queue.Enqueue((move, new Route(current)));
                        }
                    }

                    break;
                }
                else if (moves.Count == 1)
                {
                    current = moves[0];
                }
                else
                {
                    //?
                }
            }
            
        }

        Console.WriteLine(longest - 1);
    }

    static async Task Problem2BruteForce()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var map = lines.GetMap<char>(out int xLength, out int yLength);

        Point start = default;
        Point end = default;
        for (int x = 0; x < xLength; x++)
        {
            if (map[x, 0] == '.')
            {
                start = new Point(x, 0);
            }

            if (map[x, yLength - 1] == '.')
            {
                end = new Point(x, yLength - 1);
            }
        }

        var queue = new Queue<(Point, HashSet<Point>)>();
        queue.Enqueue((start, new HashSet<Point>()));
        long longest = 0;
        while (queue.TryDequeue(out var dequeued))
        {
            (Point current, HashSet<Point> history) = dequeued;
            history.Add(current);

            if (current == end)
            {
                longest = Math.Max(longest, history.Count);
                continue;
            }

            var moves = new List<Point>(4);
            var right = new Point(current.X + 1, current.Y);

            if (CanMoveP2(map, right, xLength, yLength) && !history.Contains(right))
            {
                moves.Add(right);
            }

            var left = new Point(current.X - 1, current.Y);
            if (CanMoveP2(map, left, xLength, yLength) && !history.Contains(left))
            {
                moves.Add(left);
            }

            var up = new Point(current.X, current.Y - 1);
            if (CanMoveP2(map, up, xLength, yLength) && !history.Contains(up))
            {
                moves.Add(up);
            }

            var down = new Point(current.X, current.Y + 1);
            if (CanMoveP2(map, down, xLength, yLength) && !history.Contains(down))
            {
                moves.Add(down);
            }

            for (int i = 0; i < moves.Count - 1; i++)
            {
                var newHistory = new HashSet<Point>(history);
                queue.Enqueue((moves[i], newHistory));
            }

            if (moves.Any())
            {
                queue.Enqueue((moves.Last(), history));
            }
        }

        Console.WriteLine(longest - 1);
    }
}