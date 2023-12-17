using System.Runtime.InteropServices.Marshalling;

internal class Program
{

    public class Problem1
    {
        public static async Task RunProblem1()
        {
            string[] lines = await File.ReadAllLinesAsync("Data.aoc");

            int yLength = lines.Length;
            int xLength = lines[0].Length;
            var map = new int[lines[0].Length, lines.Length];

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    map[x, y] = int.Parse(lines[y][x].ToString());
                }
            }

            PriorityQueue<(MapPoint point, RouteData data), int> queue = new();
            queue.Enqueue((new MapPoint(0, 0, Direction.None, 0), new RouteData()), 0);

            var dict = new Dictionary<MapPoint, RouteData>();
            int minresult = int.MaxValue;
            while (queue.TryDequeue(out var current, out int value))
            {
                if (dict.TryGetValue(current.point, out var found) && value >= found.Value)
                {
                    continue;
                }
                else
                {
                    dict.Add(current.point, current.data);
                }

                if (current.point.x == xLength - 1 && current.point.y == yLength - 1)
                {
                    minresult = Math.Min(minresult, value);
                }

                foreach (var dir in AllDirs)
                {
                    (int x, int y) = dir switch
                    {
                        Direction.North => (current.point.x, current.point.y - 1),
                        Direction.South => (current.point.x, current.point.y + 1),
                        Direction.West => (current.point.x - 1, current.point.y),
                        Direction.East => (current.point.x + 1, current.point.y),
                    };

                    if (x < 0 || x >= xLength || y < 0 || y >= yLength)
                    {
                        continue;
                    }

                    if (current.data.CanGoDir(dir))
                    {
                        int nextVal = map[x, y];
                        var nextData = new RouteData(current.data, dir, nextVal);
                        var nextPoint = new MapPoint(x, y, dir, nextData.History.Count);
                        queue.Enqueue((nextPoint, nextData), nextData.Value);
                    }
                }
            }

            Console.WriteLine(minresult);
        }

        public record MapPoint(int x, int y, Direction d, int dCount);

        public class RouteData
        {
            public List<Direction> History = new List<Direction>();

            public int Value = 0;

            public RouteData()
            {

            }

            public RouteData(RouteData curr, Direction next, int nextValue)
            {
                if (curr.History.Count > 0 && next == curr.History[0])
                {
                    History = new List<Direction>(curr.History);
                }

                Value = curr.Value + nextValue;
                History.Add(next);
            }

            public bool CanGoDir(Direction next)
            {
                if (History.Count == 0)
                {
                    return true;
                }

                if (History.Count < 3 || next != History[0])
                {
                    return (next, History[0]) switch
                    {
                        (Direction.North, Direction.South) => false,
                        (Direction.South, Direction.North) => false,
                        (Direction.West, Direction.East) => false,
                        (Direction.East, Direction.West) => false,
                        _ => true
                    };
                }

                return false;
            }
        }

        public static Direction[] AllDirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        };

        public enum Direction
        {
            None,
            North,
            South,
            East,
            West
        }
    }

    public class Problem2
    {
        public static async Task RunProblem2()
        {
            string[] lines = await File.ReadAllLinesAsync("Data.aoc");

            int yLength = lines.Length;
            int xLength = lines[0].Length;
            var map = new int[lines[0].Length, lines.Length];

            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    map[x, y] = int.Parse(lines[y][x].ToString());
                }
            }

            PriorityQueue<(MapPoint point, RouteData data), int> queue = new();
            queue.Enqueue((new MapPoint(0, 0, Direction.None, 0), new RouteData()), 0);

            var dict = new Dictionary<MapPoint, RouteData>();
            int minresult = int.MaxValue;
            while (queue.TryDequeue(out var current, out int value))
            {
                if (dict.TryGetValue(current.point, out var found) && value >= found.Value)
                {
                    continue;
                }
                else
                {
                    dict.Add(current.point, current.data);
                }

                if (current.data.History.Count >= 4
                    && current.point.x == xLength - 1 && current.point.y == yLength - 1)
                {
                    minresult = Math.Min(minresult, value);
                }

                foreach (var dir in AllDirs)
                {
                    (int x, int y) = dir switch
                    {
                        Direction.North => (current.point.x, current.point.y - 1),
                        Direction.South => (current.point.x, current.point.y + 1),
                        Direction.West => (current.point.x - 1, current.point.y),
                        Direction.East => (current.point.x + 1, current.point.y),
                    };

                    if (x < 0 || x >= xLength || y < 0 || y >= yLength)
                    {
                        continue;
                    }

                    if (current.data.CanGoDir(dir))
                    {
                        int nextVal = map[x, y];
                        var nextData = new RouteData(current.data, dir, nextVal);
                        var nextPoint = new MapPoint(x, y, dir, nextData.History.Count);
                        queue.Enqueue((nextPoint, nextData), nextData.Value);
                    }
                }
            }

            Console.WriteLine(minresult);
        }

        public record MapPoint(int x, int y, Direction d, int dCount);

        public class RouteData
        {
            public List<Direction> History = new List<Direction>();

            public int Value = 0;

            public RouteData()
            {

            }

            public RouteData(RouteData curr, Direction next, int nextValue)
            {
                if (curr.History.Count > 0 && next == curr.History[0])
                {
                    History = new List<Direction>(curr.History);
                }

                Value = curr.Value + nextValue;
                History.Add(next);
            }

            public bool CanGoDir(Direction next)
            {
                if (History.Count == 0)
                {
                    return true;
                }

                if (History.Count < 4)
                {
                    return next == History[0];
                }

                bool isNotReverse = (next, History[0]) switch
                {
                    (Direction.North, Direction.South) => false,
                    (Direction.South, Direction.North) => false,
                    (Direction.West, Direction.East) => false,
                    (Direction.East, Direction.West) => false,
                    _ => true
                };

                if (History.Count >= 10)
                {
                    return isNotReverse && next != History[0];
                }

                return isNotReverse;
            }
        }

        public static Direction[] AllDirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.East,
            Direction.West
        };

        public enum Direction
        {
            None,
            North,
            South,
            East,
            West
        }
    }

    public static async Task Main(string[] args)
    {
        await Problem2.RunProblem2();
    }

   
}