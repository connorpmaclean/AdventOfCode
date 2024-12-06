// See https://aka.ms/new-console-template for more information
using AocHelper;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main(string[] args)
    {
        bool p2 = false;
        P2();
    }


    //604 high
    //592 low
    private static void P2()
    {
        var rows = File.ReadAllLines("./input1.aoc");

        int safeCount = 0;
        foreach (var row in rows)
        {
            row.ParseMany("", " ", out int[] values);

            bool isIncreasing = IsIncreasing(values);

            if (isIncreasing)
            {
                values = values.Reverse().ToArray();
            }

            bool isSafe = true;
            bool hasSkip = true;
            int first = 0; int second = 1;
            while (second < values.Length)
            {
                if (!IsSafe(values[first], values[second]))
                {
                    if (!hasSkip)
                    {
                        isSafe = false;
                        break;
                    }

                    second++;
                    hasSkip = false;
                    continue;

                }

                first = ++second - 1;
            }

            if (isSafe)
            {
                safeCount++;
            }
        }

        Console.WriteLine(safeCount);
    }

    private static bool IsSafe(int val1, int val2)
    {
        int diff = val1 - val2;
        if (diff <= 0 || diff >= 4)
        {
            return false;
        }

        return true;
    }

    private static bool IsIncreasing(int[] values)
    {
        int upCount = 0;
        foreach (var it in values[..4].GetDoubleIterator<int>())
        {
            if (it.Item2 > it.Item1)
            {
                upCount++;
            }
        }

        return upCount >= 2;
    }
}