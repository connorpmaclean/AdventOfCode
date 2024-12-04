// See https://aka.ms/new-console-template for more information
using AocHelper;

Console.WriteLine("Hello, World!");


var rows = File.ReadAllLines("./sample.aoc");

int safeCount = 0;
foreach (var row in rows)
{
    row.ParseMany<int>("", " ", out int[] values);

    IList<int> data = values;
    if (values[1] > values[0])
    {
        data = data.Reverse().ToList();
    }


    int safe = 2;
    foreach (var it in data.GetDoubleIterator<int>())
    {
        int diff = it.Item1 - it.Item2;
        if (diff >= 4 || diff <= 0)
        {
            if (--safe <= 0)
            {
                break;
            }
        }
    }

    safe = Math.Min(safe, 1);
    safeCount += safe;
}

Console.WriteLine(safeCount);