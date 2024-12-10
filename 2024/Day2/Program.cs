// See https://aka.ms/new-console-template for more information
using AocHelper;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class Program
{
    private static void Main(string[] args)
    {
        P2_Working();
    }

    private static void P2_Working()
    {
        var rows = File.ReadAllLines("./input1.aoc");

        int safeCount = 0;
        foreach (var row in rows)
        {
            if (P2Row_Working(row))
            {
                safeCount++;
            }
        }

        Console.WriteLine(safeCount);
    }

    private static bool P2Row_Working(string row)
    {
        row.ParseMany("", " ", out int[] values);
        bool isSafe = true;
        if (!CheckIsValuesSafe_Working(values))
        {
            isSafe = false;
            for (int i = 0; i < values.Length; i++)
            {
                isSafe |= CheckIsValuesSafe_Working(values, i);
                if (isSafe)
                {
                    break;
                }
            }
        }

        return isSafe;
    }

    private static bool SolveP2_Attempt2(string row)
    {
        row.ParseMany("", " ", out int[] values);
        bool isIncreasing = IsIncreasing(values);

        if (isIncreasing)
        {
            values = values.Reverse().ToArray();
        }

        bool isSafe = true;
        if (!CheckIsValuesSafe_Working(values))
        {
            isSafe = false;
            for (int i = 0; i < values.Length; i++)
            {
                isSafe |= CheckIsValuesSafe_Working(values, i);
                if (isSafe)
                {
                    break;
                }
            }
        }

        return isSafe;
    }

    private static bool CheckIsValuesSafe_Working(int[] values, int skip = -1)
    {
        int first = 0; int second = 1;
        bool? isAsc = null;
        while (second < values.Length)
        {
            if (skip == second)
            {
                second++;
                continue;
            }

            if (first == skip)
            {
                first++;
                if (first == second)
                {
                    second++;
                }

                continue;
            }

            if (isAsc == null)
            {
                isAsc = values[second] > values[first];
            }

            if (!IsSafeP2_Working(values[first], values[second], isAsc.Value))
            {
                return false;
            }

            second++;
            first++;
        }

        return true;
    }

    private static bool IsSafeP2_Working(int val1, int val2, bool isAsc)
    {
        int diff;
        if (isAsc)
        {
            diff = val2 - val1;
            
        }
        else
        {
            diff = val1 - val2;
        }

        if (diff >= 4 || diff <= 0)
        {
            return false;
        }

        return true;
    }

    private static void P1()
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
            int first = 0; int second = 1;
            while (second < values.Length)
            {
                if (!IsSafe(values[first], values[second]))
                {
                    isSafe = false;
                    break;
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


    //604 high
    //592 low
    // 593 bad
    // 594 bad
    // 595 bad
    private static void P2_Attempt1()
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