


using AocHelper;
using System.Net.NetworkInformation;
using Xunit;
using static Program;

public class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2();
    }

    public static void TestDoesIntersect()
    {
        var brickA = new Brick(new Point3D(0,0,0), new Point3D(0,0,3));
        var brickB = new Brick(new Point3D(0,0,3), new Point3D(0,0,6));
        Assert.True(Brick.DoesIntersect(brickA, brickB));

        brickA = new Brick(new Point3D(0, 0, 0), new Point3D(0, 3, 0));
        brickB = new Brick(new Point3D(1, 0, 0), new Point3D(1, 3, 0));
        Assert.False(Brick.DoesIntersect(brickA, brickB));

        brickA = new Brick(new Point3D(0, 0, 0), new Point3D(3, 0, 0));
        brickB = new Brick(new Point3D(0, 1, 0), new Point3D(3, 1, 0));
        Assert.False(Brick.DoesIntersect(brickA, brickB));

        brickA = new Brick(new Point3D(0, 0, 0), new Point3D(5, 0, 0));
        brickB = new Brick(new Point3D(5, 5, 0), new Point3D(5, 1, 0));
        Assert.False(Brick.DoesIntersect(brickA, brickB));
    }

    static async Task Problem2()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var bricks = new List<Brick>();

        foreach (var line in lines)
        {
            var ends = line.Split("~");

            var start = new Point3D(ends[0].Split(",").Select(long.Parse).ToArray());
            var end = new Point3D(ends[1].Split(",").Select(long.Parse).ToArray());

            bricks.Add(new Brick(start, end));
        }

        // Fall bricks
        List<List<Brick>> levels = new();
        FallBricks(bricks, levels);

        // Can we remove?
        long result = 0;
        var cache = new Dictionary<string, HashSet<Brick>>();
        for (int i = levels.Count - 1; i >= 0; i--)
        {
            var currentLevel = levels[i];
            foreach (var brick in currentLevel)
            {
                var modifiedLevel = currentLevel.Where(b => b != brick).ToList();

                var fallenBricks = new HashSet<Brick>();
                CountFalls(levels, modifiedLevel, i, cache, fallenBricks);
                result += fallenBricks.Count;
            }
        }

        Console.WriteLine(result);
    }

    public static void CountFalls(
        List<List<Brick>> levels, 
        List<Brick> currentLevel, 
        int currentLevelIndex, 
        Dictionary<string, HashSet<Brick>> cache,
        HashSet<Brick> fallenBricks)
    {
        if (currentLevelIndex + 1 >= levels.Count)
        {
            return;
        }

        // Cache is broken ):
        //string key = Brick.GetCacheKey(currentLevel, currentLevelIndex);
        //if (cache.TryGetValue(key, out var found))
        //{
        //    foreach (var val in found)
        //    {
        //        fallenBricks.Add(val);
        //    }

        //    return;
        //}

        var nextLevel = levels[currentLevelIndex + 1];

        foreach (var nextLevelBrick in nextLevel.Where(b => currentLevelIndex + 1 == b.MinLevel))
        {
            bool conflict = false;
            foreach (var currentLevelBrick in currentLevel)//.Where(b => currentLevelIndex == b.MaxLevel))
            {
                if (Brick.DoesIntersect(currentLevelBrick, nextLevelBrick))
                {
                    conflict = true;
                    break;
                }
            }

            if (!conflict)
            {
                fallenBricks.Add(nextLevelBrick);
            }
        }

        var newNextLevel = nextLevel.Where(b => !fallenBricks.Contains(b)).ToList();
        CountFalls(levels, newNextLevel, currentLevelIndex + 1, cache, fallenBricks);

        //cache.Add(key, new HashSet<Brick>(fallenBricks));
    }

    static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var bricks = new List<Brick>();

        foreach (var line in lines)
        {
            var ends = line.Split("~");

            var start = new Point3D(ends[0].Split(",").Select(long.Parse).ToArray());
            var end = new Point3D(ends[1].Split(",").Select(long.Parse).ToArray());

            bricks.Add(new Brick(start, end));
        }

        // Fall bricks
        List<List<Brick>> levels = new();
        FallBricks(bricks, levels);

        // Can we remove?
        long result = 0;
        for (int i = 0; i < levels.Count; i++)
        {
            var thisLevel = levels[i].Where(b => b.MaxLevel == i);
            if (i >= levels.Count - 1)
            {
                result += thisLevel.Count();
                continue;
            }

            var nextLevel = levels[i + 1].Where(b => i + 1 == b.MinLevel);
            foreach (Brick toRemove in thisLevel)
            {
                var testList = new List<Brick>(thisLevel.Where(b => b != toRemove)).ToList();
                bool canRemove = true;
                foreach (var brickAbove in nextLevel)
                {
                    bool hasConflict = false;
                    foreach (var remainingBrick in testList)
                    {
                        if (Brick.DoesIntersect(brickAbove, remainingBrick))
                        {
                            hasConflict = true;
                            break;
                        }
                    }

                    if (!hasConflict)
                    {
                        canRemove = false;
                        break;
                    }
                }

                if (canRemove)
                {
                    result++;
                }
            }
        }

        Console.WriteLine(result);
    }

    private static void FallBricks(List<Brick> bricks, List<List<Brick>> levels)
    {
        foreach (var brick in bricks.OrderBy(b => b.MinZ))
        {
            int level = levels.Count;
            for (int i = level - 1; i >= 0; i--)
            {
                bool conflict = false;
                foreach (var test in levels[i])
                {
                    if (Brick.DoesIntersect(test, brick))
                    {
                        conflict = true;
                        break;
                    }
                }

                if (conflict)// intersect
                {
                    break;
                }
                else
                {
                    level = i;
                }
            }

            brick.MinLevel = level;
            brick.MaxLevel = level + (brick.MaxZ - brick.MinZ);
            for (int i = brick.MinLevel; i <= brick.MaxLevel; i++)
            {
                if (i >= levels.Count)
                {
                    levels.Add(new List<Brick> { brick });
                }
                else
                {
                    levels[i].Add(brick);
                }
            }
        }
    }

    public record Point3D(long X, long Y, int Z)
    {
        public Point3D(long[] data) : this(data[0], data[1], checked((int)data[2])) { }

        
    }

    public record Brick(Point3D Start, Point3D End)
    {
        public int MinZ => Math.Min(Start.Z, End.Z);
        public int MaxZ => Math.Max(Start.Z, End.Z);

        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }

        public string Orientation()
        {
            if (Start.Y == End.Y)
            {
                return "X";
            }
            else
            {
                return "Y";
            }
        }

        public long MinX => Math.Min(Start.X, End.X);
        public long MaxX => Math.Max(Start.X, End.X);
        public long MinY => Math.Min(Start.Y, End.Y);
        public long MaxY => Math.Max(Start.Y, End.Y);

        public override string ToString()
        {
            return $"{Start.X},{Start.Y},{Start.Z}~{End.X},{End.Y},{End.Z}";
        }

        public static string GetCacheKey(List<Brick> level, int index)
        {
            return $"{index}_{string.Join("|", level)}";
        }

        public static bool DoesIntersect(Brick a, Brick b)
        {
            if (a.Orientation() == "X")
            {
                if (b.Orientation() == "X") 
                {
                    if (a.Start.Y == b.Start.Y)
                    {
                        if (a.MaxX < b.MinX || a.MinX > b.MaxX)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    if (b.Start.X <= a.MaxX && b.Start.X >= a.MinX
                        && a.Start.Y <= b.MaxY && a.Start.Y >= b.MinY)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else // Y
            {
                if (b.Orientation() == "Y")
                {
                    if (a.Start.X == b.Start.X)
                    {
                        if (a.MaxY < b.MinY || a.MinY > b.MaxY)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    if (b.Start.Y <= a.MaxY && b.Start.Y >= a.MinY
                        && a.Start.X <= b.MaxX && a.Start.X >= b.MinX)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}