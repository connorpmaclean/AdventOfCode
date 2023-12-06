



//await Problem1();
using System;
using System.Collections.Generic;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        //await Problem1.Problem1Solution();
        await Problem2Solution.Problem2();
    }

    public class Problem2Solution
    {
        public static async Task Problem2()
        {
            var lines = await File.ReadAllLinesAsync("./Data2.aoc");
            var seedsNums = lines[0].Split(' ').Skip(1).Select(s => long.Parse(s)).ToArray();

            var seedRanges = new List<(long start, long end)>();
            for (int j = 0; j < seedsNums.Count(); j++)
            {
                long start = seedsNums[j++];
                long end = start + seedsNums[j] - 1;
                seedRanges.Add((start, end));
            }

            var maps = new List<MapV2>();
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

                maps.Add(new MapV2(mapData));

                if (lines[i] == "eof")
                    break;
            }

            var seedMap = new MapV2();
            {
                foreach (var seedRange in seedRanges)
                {
                    seedMap.list.Add(new MapEntry(
                       seedRange.start,
                       seedRange.end,
                       seedRange.start,
                       seedRange.end));
                }
            }

            var consolidatedMap = seedMap;
            foreach (var entry in consolidatedMap.list.OrderBy(m => m.SourceStart))
            {
                Console.WriteLine($"{entry.SourceStart},{entry.SourceEnd} -> {entry.DestStart},{entry.DestEnd}");
            }

            Console.WriteLine();
            foreach (var map in maps)
            {
                consolidatedMap = MapV2.MergeMaps(consolidatedMap, map);

                foreach (var entry in consolidatedMap.list.OrderBy(m => m.SourceStart))
                {
                    Console.WriteLine($"{entry.SourceStart},{entry.SourceEnd} -> {entry.DestStart},{entry.DestEnd}");
                }

                Console.WriteLine();
            }

            foreach (var entry in consolidatedMap.list.OrderBy(m => m.DestStart))
            {
                Console.WriteLine($"{entry.SourceStart},{entry.SourceEnd} -> {entry.DestStart},{entry.DestEnd}");
            }

            Console.WriteLine();
            Console.WriteLine("Solution: " + consolidatedMap.list.OrderBy(m => m.DestStart).First().DestStart);
        }

        public record MapEntry(long SourceStart, long SourceEnd, long DestStart, long DestEnd)
        {
            public override string ToString() => $"{SourceStart},{SourceEnd} -> {DestStart},{DestEnd}";
        };

        public class MapV2
        {
            public List<MapEntry> list = new();

            public MapV2() { }

            public MapV2(IEnumerable<string> lines)
            {
                foreach (var line in lines)
                {
                    var comp = line.Split(" ").Select(long.Parse).ToArray();
                    long source = comp[1];
                    long dest = comp[0];
                    long range = comp[2];
                    list.Add(new MapEntry(
                        source,
                        source + range -1,
                        dest,
                        dest + range -1
                        ));
                }

                list = list.OrderBy(x => x.SourceStart).ToList();
            }

            public long MapValue(long sourceValue)
            {
                foreach (var mapping in list)
                {
                    if (sourceValue >= mapping.SourceStart && sourceValue <= mapping.SourceEnd)
                    {
                        return sourceValue - mapping.SourceStart + mapping.DestStart;
                    }
                }

                return sourceValue;
            }

            public static MapV2 MergeMaps(MapV2 a, MapV2 b)
            {
                var newMap = new MapV2();
                var destEntries = b.list.OrderBy(a => a.SourceEnd).ToList();
                var sourceEntries = new Queue<MapEntry>(a.list.OrderBy(a => a.DestStart));
                
                while (sourceEntries.TryDequeue(out var sourceEntry))
                {
                    int i = 0;
                    while (i < destEntries.Count 
                        && destEntries[i].SourceEnd < sourceEntry.DestStart)
                    {
                        i++;
                    }


                    if (i == destEntries.Count)
                    {
                        newMap.list.Add(sourceEntry);
                        continue;
                    }

                    // Start edge
                    var destEntry = destEntries[i];
                    if (destEntry.SourceStart < sourceEntry.DestStart
                        && destEntry.SourceEnd >= sourceEntry.DestStart
                        && destEntry.SourceEnd <= sourceEntry.DestEnd)
                    {
                        long range = destEntry.SourceEnd - sourceEntry.DestStart;
                        long offset = (sourceEntry.DestStart - sourceEntry.SourceStart) + (destEntry.DestStart - destEntry.SourceStart);
                        newMap.list.Add(new MapEntry(
                            sourceEntry.SourceStart,
                            sourceEntry.SourceStart + range,
                            sourceEntry.SourceStart + offset,
                            sourceEntry.SourceStart + offset + range
                            ));

                        if (sourceEntry.SourceStart + range < sourceEntry.SourceEnd)
                        {
                            var remainingEntry = new MapEntry(
                                sourceEntry.SourceStart + range + 1,
                                sourceEntry.SourceEnd,
                                sourceEntry.DestStart + range + 1,
                                sourceEntry.DestEnd
                            );

                            sourceEntries.Enqueue(remainingEntry);
                        }

                        continue;
                    }
                    else if (destEntry.SourceStart >= sourceEntry.DestStart // Inner
                        && destEntry.SourceEnd <= sourceEntry.DestEnd)
                    {
                        long range = destEntry.SourceEnd - destEntry.SourceStart;
                        long startOffset = destEntry.SourceStart - sourceEntry.DestStart;
                        long endOffset = sourceEntry.DestEnd - destEntry.SourceEnd;
                        long destOffset = (sourceEntry.DestStart - sourceEntry.SourceStart) + (destEntry.DestStart - destEntry.SourceStart);

                        newMap.list.Add(new MapEntry(
                            sourceEntry.SourceStart + startOffset,
                            sourceEntry.SourceStart + startOffset + range,
                            sourceEntry.SourceStart + startOffset + destOffset,
                            sourceEntry.SourceStart + startOffset + destOffset + range
                            ));

                        // Split before
                        if (startOffset > 0)
                        {
                            var remainingEntry = new MapEntry(
                                sourceEntry.SourceStart,
                                sourceEntry.SourceStart + startOffset - 1,
                                sourceEntry.DestStart,
                                sourceEntry.DestStart + startOffset - 1
                            );

                            sourceEntries.Enqueue(remainingEntry);
                        }

                        // Split after
                        if (endOffset > 0)
                        {
                            var remainingEntry = new MapEntry(
                                sourceEntry.SourceEnd - endOffset + 1,
                                sourceEntry.SourceEnd,
                                sourceEntry.DestEnd - endOffset + 1,
                                sourceEntry.DestEnd
                            );

                            sourceEntries.Enqueue(remainingEntry);
                        }

                        continue;
                    }
                    else if (destEntry.SourceStart <= sourceEntry.DestEnd
                        && destEntry.SourceStart > sourceEntry.DestStart
                        && destEntry.SourceEnd > sourceEntry.DestEnd
                        && destEntry.SourceEnd >= sourceEntry.DestStart)
                    {
                        long range = sourceEntry.DestEnd - destEntry.SourceStart;
                        long offset = (sourceEntry.DestStart - sourceEntry.SourceStart) + (destEntry.DestStart - destEntry.SourceStart);

                        newMap.list.Add(new MapEntry(
                            sourceEntry.SourceEnd - range,
                            sourceEntry.SourceEnd,
                            sourceEntry.SourceEnd - range + offset,
                            sourceEntry.SourceEnd + offset
                            ));

                        if (sourceEntry.SourceEnd - range != sourceEntry.SourceStart)
                        {
                            var remainingEntry = new MapEntry(
                                sourceEntry.SourceStart,
                                sourceEntry.SourceEnd - range - 1,
                                sourceEntry.DestStart,
                                sourceEntry.DestEnd - range - 1);

                            sourceEntries.Enqueue(remainingEntry);
                        }
                    }
                    else if (destEntry.SourceStart <= sourceEntry.DestStart
                        && destEntry.SourceEnd >= sourceEntry.DestEnd)
                    {
                        long offset = destEntry.DestStart - destEntry.SourceStart;
                        newMap.list.Add(new MapEntry(
                            sourceEntry.SourceStart,
                            sourceEntry.SourceEnd,
                            sourceEntry.DestStart + offset,
                            sourceEntry.DestEnd + offset
                            ));
                    }
                    else
                    {
                        newMap.list.Add(sourceEntry);
                    }
                }

                newMap.list = newMap.list.OrderBy(x => x.SourceStart).ToList();
                return newMap;
            }
        }
    }

    public class Problem2Attempt1
    {
        public static async Task Problem2Attempt1Ans()
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
        }
    }

    public class Problem1
    {
        public static async Task Problem1Solution()
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
    }
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
