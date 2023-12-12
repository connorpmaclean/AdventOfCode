using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;

//var p1 = await Problem1();
var p2 = await Problem2();

//foreach (var item in p1)
//{
//    if (p2[item.Key] != item.Value)
//    {
//        Console.WriteLine($"{item.Key} {item.Value} {p2[item.Key]}");
//    }
//}


static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, int length) where T : IComparable
{
    if (length == 1) return list.Select(t => new T[] { t });
    return GetKCombs(list, length - 1)
        .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
            (t1, t2) => t1.Concat(new T[] { t2 }));
}


// From https://codereview.stackexchange.com/questions/194967/get-all-combinations-of-selecting-k-elements-from-an-n-sized-array
static IEnumerable<int[]> CombinationsRosettaWoRecursion2(int m, int n)
{
    int[] result = new int[m];
    Stack<int> stack = new Stack<int>(m);
    stack.Push(0);
    while (stack.Count > 0)
    {
        int index = stack.Count - 1;
        int value = stack.Pop();
        while (value < n)
        {
            result[index++] = value++;
            stack.Push(value);
            if (index != m) continue;
            yield return (int[])result.Clone(); // thanks to @xanatos
                                                //yield return result;
            break;
        }
    }
}

static IEnumerable<T[]> CombinationsRosettaWoRecursion<T>(T[] array, int m)
{
    if (array.Length < m)
        throw new ArgumentException("Array length can't be less than number of selected elements");
    if (m < 1)
        throw new ArgumentException("Number of selected elements can't be less than 1");
    T[] result = new T[m];
    foreach (int[] j in CombinationsRosettaWoRecursion2(m, array.Length))
    {
        for (int i = 0; i < m; i++)
        {
            result[i] = array[j[i]];
        }
        yield return result;
    }
}

static async Task<Dictionary<string, long>> Problem1()
{
    var results = new Dictionary<string, long>();
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long count = 0;
    foreach (string line in lines)
    {
        long localCount = 0;
        var comp = line.Split(' ');
        var map = comp[0];
        var groups = comp[1].Split(",").Select(int.Parse).ToArray();

        int missingSprings = groups.Sum() - map.Count(c => c == '#');
        int[] unknownIndices = map.Select((c, i) => (c, i)).Where(x => x.c == '?').Select(x => x.i).ToArray();

        if (missingSprings == 0)
        {
            localCount++;
            results[line] = localCount;
            count += localCount;
            continue;
        }

        var allCombs = CombinationsRosettaWoRecursion(unknownIndices, missingSprings);

        foreach (var comb in allCombs)
        {
            var mapCopy = new StringBuilder(map);
            foreach (int index in comb)
            {
                mapCopy[index] = '#';
            }

            mapCopy.Replace('?', '.');
            var testGroups = mapCopy.ToString().Split('.').Where(c => c != "").Select(g => g.Length);

            if (Enumerable.SequenceEqual(groups, testGroups))
            {
                localCount++;
            }
        }

        results[line] = localCount;
        count += localCount;
    }

    Console.WriteLine(count);
    return results;
}


static async Task<Dictionary<string, long>> Problem2()
{
    var results = new Dictionary<string, long>();
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long count = 0;
    int row = 0;
    foreach (string line in lines)
    {
        long localCount = 0;
        if (line.StartsWith("//"))
        {
            continue;
        }

        Console.Write($"{row++}: {line} => ");
        var comp = line.Split(' ');
        var mapTemp = comp[0];

        string map = new StringBuilder()
            .Append(mapTemp)
            .Append("?")
            .Append(mapTemp)
            .Append("?")
            .Append(mapTemp)
            .Append("?")
            .Append(mapTemp)
            .Append("?")
            .Append(mapTemp)
            .ToString();
            ;

        var groupsTemp = comp[1].Split(",").Select(int.Parse).ToArray();
        var groups = groupsTemp
                .Concat(groupsTemp).Concat(groupsTemp).Concat(groupsTemp).Concat(groupsTemp)
                .ToArray();

        int missingSprings = groups.Sum() - map.Count(c => c == '#');

        if (missingSprings == 0)
        {
            localCount++;
        }
        else
        {
            localCount = GetPossibilities(map.AsSpan(), groups, new Dictionary<Key, long>());
        }

        count += localCount;
        Console.WriteLine(localCount);
        results[line] = localCount;
    }

    Console.WriteLine(count);
    return results;
}

static long GetPossibilities(ReadOnlySpan<char> map, ReadOnlySpan<int> remaining, IDictionary<Key, long> cache)
{
    Key key = new Key(map, remaining);
    if (cache.TryGetValue(key, out long value))
    {
        return value;
    }

    int current = remaining[0];

    int minSpacesRemaining = current - 1;
    for (int i = 1; i < remaining.Length; i++)
    {
        minSpacesRemaining += 1 + remaining[i];
    }

    long possibilities = 0;
    for (int i = 0; i < map.Length - minSpacesRemaining; i++)
    {
        //StringBuilder mapCopy = new StringBuilder(map.ToString());

        bool valid = true;
        int j = i;
        char originalChar = map[i];
        for (; j < i + current; j++)
        {
            if (map[j] == '.')
            {
                valid = false;
                break;
            }
            else
            {
                //mapCopy[j] = '#';
            }
        }

        if (valid)
        {
            if (remaining.Length == 1)
            {
                // There should be no more springs in the map
                if (map.Length == j)
                {
                    possibilities += 1;
                }
                else
                {
                    var mapAfter = map.Slice(j);
                    bool hasNoSprings = true;
                    for (int k = 0; k < mapAfter.Length; k++)
                    {
                        if (mapAfter[k] == '#')
                        {
                            hasNoSprings = false;
                            break;
                        }
                    }

                    if (hasNoSprings)
                    {
                        possibilities += 1;
                    }
                }
            }
            else if (map[j] == '.' || map[j] == '?')
            {
                //mapCopy[j] = '.';
                possibilities += GetPossibilities(map.Slice(j + 1), remaining.Slice(1), cache);
            }
        }

        if (originalChar == '#') // if we hit a spring, the group must start here, so no point going further
        {
            break;
        }
    }

    cache[key] = possibilities;
    return possibilities;
}

public record Key(string mapString, string remainingValue)
{
    public Key(ReadOnlySpan<char> map, ReadOnlySpan<int> remaining)
        : this (map.ToString(), GetRemainingValue(remaining))
    {
    }

    private static string GetRemainingValue(ReadOnlySpan<int> remaining)
    {
        var sb = new StringBuilder();
        foreach (int i in remaining)
        {
            sb.Append(i);
            sb.Append(',');
        }

        return sb.ToString();
        //checked
        //{
        //    long val = 0;
        //    long multiplier = 1;
        //    foreach (var item in remaining)
        //    {
        //        val += item * multiplier;
        //        multiplier *= 10;
        //    }

        //    return val;
        //}
        
    }
}