
//await Problem1();
using System.Collections.Generic;

//await Problem1();
await Problem2();



static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./Data2.aoc");
    long[] seeds = lines[0].Split(' ').Skip(1).Select(s => long.Parse(s)).ToArray();

    //string[] mapNames = ["seed-to-soil map:", "soil-to-fertilizer map:", "fertilizer-to-water map:", "water-to-light map:", "light-to-temperature map:", "temperature-to-humidity map:", "humidity-to-location map:"];

    //Dictionary<string, Map> mapDict = new Dictionary<string, Map>();

    var maps = new List<Map>();
    int i = 1;
    while (true)
    {
        while (lines[i] == "")
        {
            i++;
        }

        i++; // skip header
        List<string> mapData = new List<string>();
        while (lines[i] != "" && lines[i] != "eof")
        {
            mapData.Add(lines[i++]);
        }

        maps.Add(new Map(mapData));

        if (lines[i] == "eof")
            break;
    }

    long minValue = int.MaxValue;
    foreach (long seed in seeds)
    {
        long currentValue = seed;
        foreach (var map in maps)
        {
            currentValue = map.MapValue(currentValue);
        }

        minValue = Math.Min(currentValue, minValue);
    }

    Console.WriteLine(minValue);
}

static async Task Problem2()
{
    var lines = await File.ReadAllLinesAsync("./Data2.aoc");
    var seedsNums = lines[0].Split(' ').Skip(1).Select(s => long.Parse(s)).ToArray();

    var seedRanges = new List<(long start, long end)>();
    for (int j = 0; j < seedsNums.Count(); j++)
    {
        long start = seedsNums[j++];
        long end = start + seedsNums[j];
        seedRanges.Add((start, end));
    }

    var maps = new List<Map>();
    int i = 1;
    while (true)
    {
        while (lines[i] == "")
        {
            i++;
        }

        i++; // skip header
        List<string> mapData = new List<string>();
        while (lines[i] != "" && lines[i] != "eof")
        {
            mapData.Add(lines[i++]);
        }

        maps.Add(new Map(mapData, true));

        if (lines[i] == "eof")
            break;
    }

    maps.Reverse();

    IEnumerable<long> lowValues = Enumerable.Range(0, 1000000).Select(x => (long)x);
    foreach (long locValue in lowValues.Union(maps.First().EnumerateValues()))
    {
        long currentValue = locValue;
        foreach (var map in maps)
        {
            currentValue = map.MapValue(currentValue);
        }

        //Console.WriteLine(locValue);
        foreach (var seedRange in seedRanges)
        {
            if (currentValue >= seedRange.start && currentValue < seedRange.end)
            {
                Console.WriteLine(locValue);
                Environment.Exit(0);
            }
        }
    }



    //foreach (long seed in seeds)
    //{
    //    long currentValue = seed;
    //    foreach (var map in maps)
    //    {
    //        currentValue = map.MapValue(currentValue);
    //    }

    //    minValue = Math.Min(currentValue, minValue);
    //}

    //Console.WriteLine(minValue);
}

class Map
{
    public SortedList<long, (long sourceStart, long sourceEnd, long destStart)> list = new();

    public Map(IEnumerable<string> lines, bool inverse = false)
    {
        if (!inverse)
        {
            foreach (var line in lines)
            {
                var comp = line.Split(" ").Select(long.Parse).ToArray();
                long source = comp[1];
                long dest = comp[0];
                long range = comp[2];
                list.Add(source, (source, source + range, dest));
            }
        }
        else
        {
            foreach (var line in lines)
            {
                var comp = line.Split(" ").Select(long.Parse).ToArray();
                long dest = comp[1];
                long source = comp[0];
                long range = comp[2];
                list.Add(source, (source, source + range, dest));
            }
        }
    }

    public IEnumerable<long> EnumerateValues()
    {
        foreach (var mapping in list)
        {
            for (long i = mapping.Value.sourceStart; i < mapping.Value.sourceEnd; i++)
            {
                yield return i;
            }
        }
        
    }

    public long MapValue(long sourceValue)
    {
        foreach (var mapping in list)
        {
            if (sourceValue >= mapping.Value.sourceStart && sourceValue < mapping.Value.sourceEnd)
            {
                return sourceValue - mapping.Value.sourceStart + mapping.Value.destStart;
            }
        }

        return sourceValue;
    }
}
