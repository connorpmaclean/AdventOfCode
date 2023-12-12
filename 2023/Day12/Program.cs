using System.Text;

await Problem2();

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

static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long count = 0;
    foreach (string line in lines)
    {
        var comp = line.Split(' ');
        var map = comp[0];
        var groups = comp[1].Split(",").Select(int.Parse).ToArray();

        int missingSprings = groups.Sum() - map.Count(c => c == '#');
        int[] unknownIndices = map.Select((c, i) => (c, i)).Where(x => x.c == '?').Select(x => x.i).ToArray();

        if (missingSprings == 0)
        {
            count++;
            continue;
        }

        try
        {
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
                    count++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed row: {line}");
        }

    }

    Console.WriteLine(count);
}


static async Task Problem2()
{
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long count = 0;
    foreach (string line in lines)
    {
        var comp = line.Split(' ');
        var mapTemp = comp[0];

        string map = new StringBuilder()
            .Append(mapTemp)
            .Append(mapTemp)
            .Append(mapTemp)
            .Append(mapTemp)
            .Append(mapTemp)
            .ToString();
            ;

        Console.WriteLine(map);

        var groupsTemp = comp[1].Split(",").Select(int.Parse).ToArray();
        var groups = groupsTemp.Concat(groupsTemp).Concat(groupsTemp).Concat(groupsTemp).Concat(groupsTemp).ToArray();

        int missingSprings = groups.Sum() - map.Count(c => c == '#');
        int[] unknownIndices = map.Select((c, i) => (c, i)).Where(x => x.c == '?').Select(x => x.i).ToArray();

        if (missingSprings == 0)
        {
            count++;
            continue;
        }

        var allCombs = CombinationsRosettaWoRecursion(unknownIndices, missingSprings);

        long localCount = 0;
        Parallel.ForEach(allCombs, comb =>
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
                Interlocked.Increment(ref localCount);
            }
        });

        count += localCount;
        Console.WriteLine(localCount);
    }

    Console.WriteLine("Total: " + count);
}