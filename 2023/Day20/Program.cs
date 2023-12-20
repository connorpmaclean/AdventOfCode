using AocHelper;
using System.Collections;
using static Program;

internal class Program
{
    public static async Task Main(string[] args)
    {
        await P2();
    }

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

    public static async Task P2()
    {
        // Manual math - these values are the first button press count when each respective input on mf is true.
        // mf is the conj module that feeds into rx, so it is the main one we care about
        // The loops are perfect (no offset) so just find LCM.
        long[] values = { 3821, 3919, 3923, 4019 };

        long lcm = values[0];
        for (int i = 1; i < values.Length; i++)
        {
            lcm = LCM(lcm, values[i]);
        }

        Console.WriteLine(lcm);


        // Discover loops
        string[] lines = await File.ReadAllLinesAsync("Data.aoc");

        var modules = new Dictionary<string, IModule>();

        foreach (string line in lines)
        {
            line.Parse(out string module, " -> ").Parse(out string dest);

            var dests = dest.Replace(" ", "").Split(",");

            if (module == "broadcaster")
            {
                modules.Add(module, new Broadcaster(dests));
            }
            else if (module.StartsWith("&"))
            {
                string name = module[1..^0];
                modules.Add(name, new Conjunction(name, dests));
            }
            else
            {
                string name = module[1..^0];
                modules.Add(name, new FlipFlop(name, dests));
            }
        }

        foreach (var module in modules.Values)
        {
            foreach (var dest in module.Dests)
            {
                if (!modules.TryGetValue(dest, out var next))
                {
                    continue;
                }

                if (next is Conjunction c)
                {
                    c.Inputs[module.Name] = false;
                }
            }
        }

        var mf = modules["mf"];

        long lowCount = 0;
        long highCount = 0;

        for (int i = 0; i < 10000; i++)
        {
            //Console.WriteLine();
            Queue<Pulse> queue = new Queue<Pulse>();
            queue.Enqueue(new Pulse("button", false, "broadcaster", i + 1));
            while (queue.TryDequeue(out var pulse))
            {
                //Console.WriteLine(pulse);
                if (pulse.signal)
                {
                    highCount++;
                }
                else
                {
                    lowCount++;
                }

                if (!modules.TryGetValue(pulse.dest, out var next))
                {
                    continue;
                }

                next.HandlePulse(pulse, queue);
            }
        }

        Console.WriteLine("low " + lowCount);
        Console.WriteLine("high " + highCount);
        Console.WriteLine(lowCount * highCount);
    }

    public static async Task P1()
    {
        string[] lines = await File.ReadAllLinesAsync("Data.aoc");

        var modules = new Dictionary<string, IModule>();

        foreach (string line in lines)
        {
            line.Parse(out string module, " -> ").Parse(out string dest);

            var dests = dest.Replace(" ", "").Split(",");

            if (module == "broadcaster")
            {
                modules.Add(module, new Broadcaster(dests));
            }
            else if (module.StartsWith("&"))
            {
                string name = module[1..^0];
                modules.Add(name, new Conjunction(name, dests));
            }
            else
            {
                string name = module[1..^0];
                modules.Add(name, new FlipFlop(name, dests));
            }
        }

        foreach (var module in modules.Values)
        {
            foreach (var dest in module.Dests)
            {
                if (!modules.TryGetValue(dest, out var next))
                {
                    continue;
                }

                if (next is Conjunction c)
                {
                    c.Inputs[module.Name] = false;
                }
            }
        }

        var mf = modules["mf"];

        long lowCount = 0;
        long highCount = 0;

        for (int i = 0; i < 1000; i++)
        {
            //Console.WriteLine();
            Queue<Pulse> queue = new Queue<Pulse>();
            queue.Enqueue(new Pulse("button", false, "broadcaster", i + 1));
            while (queue.TryDequeue(out var pulse))
            {
                //Console.WriteLine(pulse);
                if (pulse.signal)
                {
                    highCount++;
                }
                else
                {
                    lowCount++;
                }

                if (!modules.TryGetValue(pulse.dest, out var next))
                {
                    continue;
                }

                next.HandlePulse(pulse, queue);
            }
        }

        Console.WriteLine("low " + lowCount);
        Console.WriteLine("high " + highCount);
        Console.WriteLine(lowCount * highCount);
    }

    public interface IModule
    {
        string[] Dests { get; }

        string Name { get; }

        void HandlePulse(Pulse pulse, Queue<Pulse> queue);
    }

    public record Pulse(string source, bool signal, string dest, long press);

    public class Conjunction(string name, string[] intDests) : IModule
    {
        public Dictionary<string, bool> Inputs = new();

        public string[] Dests => intDests;

        public string Name => name;

        public void HandlePulse(Pulse pulse, Queue<Pulse> queue)
        {
            if (name == "mf")
            {
                if (Inputs[pulse.source] != pulse.signal)
                {
                    Console.WriteLine($"{pulse.source}: {Inputs[pulse.source]} -> {pulse.signal}, {pulse.press}");
                }
            }

            Inputs[pulse.source] = pulse.signal;

            if (pulse.signal && Inputs.Values.All(v => v))
            {
                foreach (string dest in intDests)
                {
                    queue.Enqueue(new Pulse(name, false, dest, pulse.press));
                }
            }
            else
            {
                foreach (string dest in intDests)
                {
                    queue.Enqueue(new Pulse(name, true, dest, pulse.press));
                }
            }
        }
    }

    public class FlipFlop(string name, string[] intDests) : IModule
    {
        bool State = false;

        public string[] Dests => intDests;

        public string Name => name;

        public void HandlePulse(Pulse pulse, Queue<Pulse> queue)
        {
            if (!pulse.signal)
            {
                State = !State;
                foreach (string dest in intDests)
                {

                    queue.Enqueue(new Pulse(name, State, dest, pulse.press));
                }
            }
        }
    }

    public class Broadcaster(string[] intDests) : IModule
    {
        public string[] Dests => intDests;

        public string Name  => "broadcaster";

        public void HandlePulse(Pulse pulse, Queue<Pulse> queue)
        {
            foreach (string s in intDests)
            {
                queue.Enqueue(new Pulse(Name, pulse.signal, s, pulse.press));
            }
        }
        }
}