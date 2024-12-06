using AocHelper;

internal class Program
{
    private static char[] p1Target = "XMAS".ToCharArray();
    private static HashSet<string> p2Targets= new() { "MMSS", "SMMS", "SSMM", "MSSM" };

    private static (Func<int, int> x, Func<int, int> y)[] p2Funcs = new (Func<int, int>, Func<int, int>)[]
    {
        (x => x + 1, y => y - 1),
        (x => x, y => y + 2),
        (x => x - 2, y => y),
        (x => x, y => y - 2),
    };

    private static void Main(string[] args)
    {
        P2();
    }

    // 2024 too high
    private static void P2()
    {
        Console.WriteLine("Hello, World!");

        string[] data = File.ReadAllLines("./data.aoc");
        var map = data.GetMap<char>(out int xLength, out int yLength);

        int count = 0;
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                if (map[x, y] == 'A')
                {
                    char[] target = new char[4];

                    CheckSequenceP2(map, x, y, 0, target);

                    string found = new string(target);

                    if (p2Targets.Contains(found))
                    {
                        count++;
                    }
                }
            }
        }

        Console.WriteLine(count);
    }

    private static void CheckSequenceP2(char[,] map, int x, int y, int letter, char[] target)
    {
        x = p2Funcs[letter].x(x);
        y = p2Funcs[letter].y(y);
        

        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
        {
            return;
        }

        target[letter] = map[x, y];
        letter++;

        if (letter == 4)
        {
            return;
        }

        CheckSequenceP2(map, x, y, letter, target);
    }

    //private static void CheckP2(char[,] map, int x, int y, ref int m, ref int s)
    //{
    //    if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
    //    {
    //        return;
    //    }

    //    char c = map[x, y];
    //    if (c == 'M')
    //    {
    //        m += 1;
    //    }
    //    else if (c == 'S')
    //    {
    //        s += 1;
    //    }
    //}

    private static void P1()
    {
        Console.WriteLine("Hello, World!");

        string[] data = File.ReadAllLines("./data.aoc");
        var map = data.GetMap<char>(out int xLength, out int yLength);

        int count = 0;
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                if (map[x, y] == p1Target[0])
                {
                    count += CheckSequenceP1(map, x, y, 0, (x) => x, (y) => y + 1);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x + 1, (y) => y + 1);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x + 1, (y) => y);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x + 1, (y) => y - 1);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x, (y) => y - 1);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x - 1, (y) => y - 1);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x - 1, (y) => y);
                    count += CheckSequenceP1(map, x, y, 0, (x) => x - 1, (y) => y + 1);
                }
            }
        }

        Console.WriteLine(count);
    }

    private static int CheckSequenceP1(char[,] map, int x, int y, int letter, Func<int, int> xFunc, Func<int, int> yFunc)
    {
        x = xFunc(x);
        y = yFunc(y);
        letter++;

        if (x < 0 || y < 0 || x >= map.GetLength(0) || y >= map.GetLength(1))
        {
            return 0;
        }

        if (map[x, y] != p1Target[letter])
        {
            return 0;
        }

        if (letter == p1Target.Length - 1)
        {
            return 1;
        }

        return CheckSequenceP1(map, x, y, letter, xFunc, yFunc);
    }
}