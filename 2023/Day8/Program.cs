using System.Text.RegularExpressions;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await Problem2();

    }


    private static async Task Problem2()
    {
        string[] data = await File.ReadAllLinesAsync("./Data1.aoc");

        NodeP1.ParseInput(data.Skip(2));

        string dirs = data[0];

        var currents = NodeP1.AllNodes.Values.Where(n => n.IsA).ToArray();

        Console.WriteLine("start");

        int i = 0;
        int count = 0;
        while (!currents.All(n => n.IsZ))
        {
            char dir = dirs[i];

            Func<NodeP1, NodeP1> selector = dir == 'L'
                ? n => n.Left
                : n => n.Right;

            for (int j = 0; j < currents.Length; j++) 
            {
                currents[j] = selector(currents[j]);
            }

            i++;
            i %= dirs.Length;
            count++;
        }

        Console.WriteLine(count);
    }

    private static async Task Problem1()
    {
        string[] data = await File.ReadAllLinesAsync("./Data1.aoc");

        NodeP1.ParseInput(data.Skip(2));

        string dirs = data[0];

        NodeP1 current = NodeP1.AllNodes["AAA"];

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

    public record NodeP1
    {
        private static Regex regex = new Regex("([A-Z0-9]{3}) = \\(([A-Z0-9]{3}), ([A-Z0-9]{3})\\)", RegexOptions.Compiled);
        public static Dictionary<string, NodeP1> AllNodes = new();

        public NodeP1 Left;
        public NodeP1 Right;
        public string Name;

        public bool IsA;
        public bool IsZ;

        public NodeP1(string name)
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

                if (!AllNodes.TryGetValue(name, out NodeP1 node))
                {
                    node = new NodeP1(name);
                    AllNodes.Add(name, node);
                }

                if (!AllNodes.TryGetValue(leftName, out NodeP1 leftNode))
                {
                    leftNode = new NodeP1(leftName);
                    AllNodes.Add(leftName, leftNode);
                }

                if (!AllNodes.TryGetValue(rightName, out NodeP1 rightNode))
                {
                    rightNode = new NodeP1(rightName);
                    AllNodes.Add(rightName, rightNode);
                }

                node.Right = rightNode;
                node.Left = leftNode;
            }
        }
    }
}