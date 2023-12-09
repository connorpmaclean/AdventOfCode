using System.Text.RegularExpressions;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await Problem2Solution();

    }

    private static async Task Problem1()
    {
        string[] data = await File.ReadAllLinesAsync("./Data1.aoc");

        Node.ParseInput(data.Skip(2));

        string dirs = data[0];

        Node current = Node.AllNodes["AAA"];

        int i = 0;
        int count = 0;
        while (current.Name != "ZZZ")
        {
            char dir = dirs[i];

            current = dir == 'L' ? current.Left : current.Right;

            i++;
            i %= dirs.Length;
            count++;
        }

        Console.WriteLine(count);
    }

    private static async Task Problem2Solution()
    {
        string[] data = await File.ReadAllLinesAsync("./Data1.aoc");

        Node.ParseInput(data.Skip(2));

        string dirs = data[0];

        var currents = Node.AllNodes.Values.Where(n => n.IsA).OrderBy(n => n.Name).ToArray();
        var zNodes = Node.AllNodes.Where(n => n.Value.IsZ).Select(n => n.Value).ToArray();

        Console.WriteLine("start");

        var loopValues = new List<long>();

        for (int j = 0; j < currents.Length; j++)
        {
            int i = 0;
            long count = 0;
            var dict = new Dictionary<(string, int), long>();
            var current = currents[j];

            while (!currents.All(n => n.IsZ))
            {
                char dir = dirs[i];

                current = dir == 'L'
                    ? current.Left
                    : current.Right;

                count++;
                if (current.IsZ)
                {
                    if (dict.TryGetValue((current.Name, i), out long foundCount))
                    {
                        Console.WriteLine($"{j}: Loops at node {current.Name} at counts {foundCount} and {count}. The path hits {dict.Count} Z nodes total.");
                        loopValues.Add(foundCount);
                        break;
                    }
                    else
                    {
                        dict.Add((current.Name, i), count);
                    }
                }

                i++;
                i %= dirs.Length;
            }
        }

        long lcm = loopValues.Aggregate(LCM);
        Console.WriteLine(lcm);

        static long GCF(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static long LCM(long a, long b)
        {
            return (a / GCF(a, b)) * b;
        }
    }

    private static async Task Problem2BruteForce()
    {
        string[] data = await File.ReadAllLinesAsync("./Data1.aoc");

        Node.ParseInput(data.Skip(2));

        string dirs = data[0];

        var currents = Node.AllNodes.Values.Where(n => n.IsA).OrderBy(n => n.Name).ToArray();
        var zNodes = Node.AllNodes.Where(n => n.Value.IsZ).Select(n => n.Value).ToArray();

        Console.WriteLine("start");

        int i = 0;
        long count = 0;
        while (!currents.All(n => n.IsZ))
        {
            char dir = dirs[i];

            Func<Node, Node> selector = dir == 'L'
                ? n => n.Left
                : n => n.Right;

            for (int j = 0; j < currents.Length; j++) 
            {
                currents[j] = selector(currents[j]);
            }

            count++;

            //if (currents.Any(n => n.IsZ))
            //{
            //    Console.WriteLine($"{dir} {string.Join(' ', currents.Select(n => n.IsZ ? "Z" : "N"))} {count}");
            //}

            i++;
            i %= dirs.Length;
        }

        Console.WriteLine(count);
    }

    public record Node
    {
        private static Regex regex = new Regex("([A-Z0-9]{3}) = \\(([A-Z0-9]{3}), ([A-Z0-9]{3})\\)", RegexOptions.Compiled);
        public static Dictionary<string, Node> AllNodes = new();

        public Node Left;
        public Node Right;
        public string Name;

        public bool IsA;
        public bool IsZ;

        public Node(string name)
        {
            Name = name;
            IsA = name.EndsWith("A");
            IsZ = name.EndsWith("Z");
        }

        public static void ParseInput(IEnumerable<string> lines)
        {
            
            foreach (string line in lines)
            {
                var match = regex.Match(line);
                string name = match.Groups[1].Value;
                string leftName = match.Groups[2].Value;
                string rightName = match.Groups[3].Value;

                if (!AllNodes.TryGetValue(name, out Node node))
                {
                    node = new Node(name);
                    AllNodes.Add(name, node);
                }

                if (!AllNodes.TryGetValue(leftName, out Node leftNode))
                {
                    leftNode = new Node(leftName);
                    AllNodes.Add(leftName, leftNode);
                }

                if (!AllNodes.TryGetValue(rightName, out Node rightNode))
                {
                    rightNode = new Node(rightName);
                    AllNodes.Add(rightName, rightNode);
                }

                node.Right = rightNode;
                node.Left = leftNode;
            }
        }
    }
}