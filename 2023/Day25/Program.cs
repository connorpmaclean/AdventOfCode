using AocHelper;
using System.Runtime.InteropServices;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var lines = await File.ReadAllLinesAsync("Data.aoc");

        var comps = new Dictionary<string, Component>();

        foreach (var line in lines)
        {
            line.Parse(out string start, ": ").ParseMany("", " ", out string[] connections);

            Component source;
            if (!comps.TryGetValue(start, out source))
            {
                source = new Component(start);
                comps[start] = source;
            }

            foreach (var destName in connections)
            {
                Component dest;
                if (!comps.TryGetValue(destName, out dest))
                {
                    dest = new Component(destName);
                    comps[destName] = dest;
                }

                source.Connections.Add(dest);
                dest.Connections.Add(source);
            }
        }

        var possibleEliminations = new HashSet<(string a, string b)>();
        foreach (var eligible in comps.Values.Where(c => c.Connections.Count >= 3))
        {
            foreach (var split in eligible.Connections.Where(c => c.Name != eligible.Name))
            {
                string a = eligible.Name;
                string b = split.Name;
                if (possibleEliminations.Contains((a, b)) || possibleEliminations.Contains((b, a)))
                {
                    continue;
                }

                possibleEliminations.Add((a, b));

                //var copy = new List<Component>(comps.Values.Select(c => c with { }));
                //copy.Where(c => c.Name == eligible.Name).Single().Connections.RemoveAll(m => m.Name == split.Name);
                //copy.Where(c => c.Name == split.Name).Single().Connections.RemoveAll(m => m.Name == eligible.Name);

                //var dict = new Dictionary<string, int>();
                //Traverse(dict, comps.Values.First());

                //if (dict.Count != comps.Count)
                //{
                //    // Good split.
                //}
            }
        }

        Random random = new Random();
        var pairs = new (string, string)[3];
        var possibleEliminationsList = possibleEliminations.ToList();
        for (int i = 0; i < possibleEliminationsList.Count; i++)
        {
            for (int j = i + 1; j < possibleEliminationsList.Count; j++)
            {
                for (int k = j + 1; k < possibleEliminationsList.Count; k++)
                {
                    pairs[0] = possibleEliminationsList[i];
                    pairs[1] = possibleEliminationsList[j];
                    pairs[2] = possibleEliminationsList[k];

                    try
                    {
                        //var set = new HashSet<(string a, string b)>();
                        var copy = new List<Component>(comps.Values.Select(c => c with { }));
                        foreach (var pair in pairs)
                        {
                            (string a, string b) = pair;
                            copy.Where(c => c.Name == a).Single().Connections.RemoveAll(m => m.Name == b);
                            copy.Where(c => c.Name == b).Single().Connections.RemoveAll(m => m.Name == a);
                            //Console.WriteLine("Removing: " + pair);
                        }

                        var dict = new Dictionary<string, int>();
                        Traverse(dict, comps.Values.First());
                        int group1 = dict.Count;
                        int group2 = comps.Count - dict.Count;

                        if (dict.Count != comps.Count && group1 > 1 && group2 > 1)
                        {
                            var remainingComps = new Dictionary<string, Component>();
                            foreach (var item in comps)
                            {
                                if (!dict.ContainsKey(item.Key))
                                {
                                    remainingComps.Add(item.Key, item.Value);
                                }
                            }

                            dict = new Dictionary<string, int>();
                            Traverse(dict, remainingComps.Values.First());

                            if (dict.Count == remainingComps.Count)
                            {
                                Console.WriteLine(group1 * group2);
                                break;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }
        }

        //while (true)
        //{

        //    try
        //    {
        //        //var set = new HashSet<(string a, string b)>();
        //        var copy = new List<Component>(comps.Values.Select(c => c with { }));
        //        foreach (var pair in possibleEliminations.OrderBy(p => random.Next()).Take(3))
        //        {
        //            (string a, string b) = pair;
        //            copy.Where(c => c.Name == a).Single().Connections.RemoveAll(m => m.Name == b);
        //            copy.Where(c => c.Name == b).Single().Connections.RemoveAll(m => m.Name == a);
        //            //Console.WriteLine("Removing: " + pair);
        //        }

        //        var dict = new Dictionary<string, int>();
        //        Traverse(dict, comps.Values.First());
        //        int group1 = dict.Count;
        //        int group2 = comps.Count - dict.Count;

        //        if (dict.Count != comps.Count && group1 > 1 && group2 > 1)
        //        {
        //            var remainingComps = new Dictionary<string, Component>();
        //            foreach (var item in comps)
        //            {
        //                if (!dict.ContainsKey(item.Key))
        //                {
        //                    remainingComps.Add(item.Key, item.Value);
        //                }
        //            }

        //            dict = new Dictionary<string, int>();
        //            Traverse(dict, remainingComps.Values.First());

        //            if (dict.Count == remainingComps.Count)
        //            {
        //                Console.WriteLine(group1 * group2);
        //                break;
        //            }

                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        continue;
        //    }
        //}

        
    }

    public static void Traverse(Dictionary<string, int> counts, Component current)
    {
        if (!counts.ContainsKey(current.Name))
        {
            counts[current.Name] = 0;
        }
        else
        {
            return;
        }
        
        foreach (var c in current.Connections)
        {
            Traverse(counts, c);
        }
    }

    public record Component(string Name, List<Component> Connections)
    {
        public List<Component> Connections = new List<Component>();

        public Component(string name) : this(name, new List<Component>())
        {
        }
    }
}