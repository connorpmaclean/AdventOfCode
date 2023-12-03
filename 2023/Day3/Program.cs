

using System.Diagnostics.CodeAnalysis;

await Problem1();
await Problem2();

static async Task Problem1()
{
    string[] lines = await File.ReadAllLinesAsync("./Data2.txt");

    int xLength = lines[0].Length;
    int yLength = lines.Length;

    var map = new char[lines[0].Length, lines.Length];

    int y = 0;
    foreach (var line in lines)
    {
        int x = 0;
        foreach (char c in line)
        {
            map[x++, y] = c;
        }

        y++;
    }

    int? currentNumber = null;
    int result = 0;
    for (y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {
            char current = map[x, y];
            if (char.IsDigit(current))
            {
                if (currentNumber == null)
                {
                    currentNumber = current - '0';
                }
                else
                {
                    currentNumber *= 10;
                    currentNumber += current - '0';
                }
            }

            if (x + 1 >= xLength || !char.IsDigit(map[x + 1, y]))
            {
                if (currentNumber.HasValue)
                {
                    if (HasAdjSymbol(currentNumber.Value, map, xLength, yLength, x, y))
                    {
                        result += currentNumber.Value;
                    }

                    currentNumber = null;
                }

            }
        }
    }

    Console.WriteLine(result);

    static bool HasAdjSymbol(int number, char[,] map, int xLength, int yLength, int x, int y)
    {
        int numLength = number.ToString().Length;

        int maxY = Math.Min(y + 1, yLength - 1);
        int minY = Math.Max(y - 1, 0);
        int maxX = Math.Min(x + 1, xLength - 1);
        int minX = Math.Max(x - numLength, 0);

        for (int yy = minY; yy <= maxY; yy++)
        {
            for (int xx = minX; xx <= maxX; xx++)
            {
                char c = map[xx, yy];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
        }

        return false;
    }
}


static async Task Problem2()
{
    string[] lines = await File.ReadAllLinesAsync("./Data2.txt");

    int xLength = lines[0].Length;
    int yLength = lines.Length;

    var map = new char[lines[0].Length, lines.Length];

    int y = 0;
    foreach (var line in lines)
    {
        int x = 0;
        foreach (char c in line)
        {
            map[x++, y] = c;
        }

        y++;
    }

    int? currentNumber = null;
    Dictionary<(int x, int y), List<int>> gears = new();
    for (y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {
            char current = map[x, y];
            if (char.IsDigit(current))
            {
                if (currentNumber == null)
                {
                    currentNumber = current - '0';
                }
                else
                {
                    currentNumber *= 10;
                    currentNumber += current - '0';
                }
            }

            if (x + 1 >= xLength || !char.IsDigit(map[x + 1, y]))
            {
                if (currentNumber.HasValue)
                {
                    AddToGears(currentNumber.Value, map, xLength, yLength, x, y, gears);
                    currentNumber = null;
                }
            }
        }
    }

    int result = gears.Values.Where(x => x.Count == 2).Sum(x => x[0] * x[1]);

    Console.WriteLine(result);

    static void AddToGears(int number, char[,] map, int xLength, int yLength, int x, int y, Dictionary<(int x, int y), List<int>> gears)
    {
        int numLength = number.ToString().Length;

        int maxY = Math.Min(y + 1, yLength - 1);
        int minY = Math.Max(y - 1, 0);
        int maxX = Math.Min(x + 1, xLength - 1);
        int minX = Math.Max(x - numLength, 0);

        for (int yy = minY; yy <= maxY; yy++)
        {
            for (int xx = minX; xx <= maxX; xx++)
            {
                char c = map[xx, yy];
                if (c == '*')
                {
                    if (!gears.TryGetValue((xx, yy), out var list))
                    {
                        list = new List<int>();
                    }

                    list.Add(number);
                    gears[(xx, yy)] = list;
                }
            }
        }
    }
}

