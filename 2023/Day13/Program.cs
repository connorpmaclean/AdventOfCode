

using System.Collections;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem1();
        await Problem2();

        
    }

    static IEnumerable<List<string>> ParseInput(string[] lines)
    {
        var list = new List<string>();
        foreach (var line in lines)
        {
            if (line == "")
            {
                yield return list;
                list = new List<string>();
            }
            else
            {
                list.Add(line);
            }
        }

        yield return list;
    }

    static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        var inputs = ParseInput(lines);

        long result = 0;
        foreach (var input in inputs)
        {
            long mapValue = GetMirrorValue(input, false);
            Console.WriteLine(mapValue);
            result += mapValue;
        }

        Console.WriteLine(result);
    }

    private static long GetMirrorValue(List<string> input, bool p2)
    {
        var columns = new List<BitArray>();
        var rows = new List<BitArray>();
        for (int y = 0; y < input.Count; y++)
        {
            var row = new BitArray(input[0].Length);
            rows.Add(row);

            for (int x = 0; x < input[0].Length; x++)
            {
                if (y == 0)
                {
                    columns.Add(new BitArray(input.Count));
                }

                if (input[y][x] == '#')
                {
                    row[x] = true;
                    columns[x][y] = true;
                }
            }
        }

        if (p2)
        {
            long result = FindMirrorValueP2(columns);
            if (result == 0)
            {
                result = 100 * FindMirrorValueP2(rows);
            }

            return result;
        }
        else
        {
            long result = FindMirrorValue(columns);
            if (result == 0)
            {
                result = 100 * FindMirrorValue(rows);
            }


            return result;
        }
    }

    private static long FindMirrorValue(List<BitArray> values)
    {
        for (int x = 0; x < values.Count - 1; x++)
        {
            int i = x;
            int j = x + 1;

            bool valid = true;
            while (i >= 0 && j < values.Count) 
            {
                var result = new BitArray(values[i--]).Xor(values[j++]);

                if (result.HasAnySet())
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                return x + 1;
            }
        }

        return 0;
    }

    private static long FindMirrorValueP2(List<BitArray> values)
    {
        for (int x = 0; x < values.Count - 1; x++)
        {
            int i = x;
            int j = x + 1;

            long errors = 0;
            while (i >= 0 && j < values.Count)
            {
                var result = new BitArray(values[i--]).Xor(values[j++]);

                foreach (var b in result)
                {
                    if ((bool)b)
                    {
                        errors++;
                    }
                }

                if (errors > 1)
                {
                    break;
                }
            }

            if (errors == 1)
            {
                return x + 1;
            }
        }

        return 0;
    }

    static async Task Problem2()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        var inputs = ParseInput(lines);

        long result = 0;
        foreach (var input in inputs)
        {
            long mapValue = GetMirrorValue(input, true);
            Console.WriteLine(mapValue);
            result += mapValue;
        }

        Console.WriteLine(result);
    }
}