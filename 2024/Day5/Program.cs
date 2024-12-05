using AocHelper;

internal class Program
{


    private static void Main(string[] args)
    {
        P1();
    }

    public static void P1()
    {
        var lines = File.ReadAllLines("data.aoc");

        Dictionary<int, Rule> ruleDict = new Dictionary<int, Rule>();

        int i = 0;
        for (; i < lines.Length; i++) 
        {
            string? line = lines[i];
            if (line == string.Empty)
            {
                break;
            }

            line.ParseMany("", "|", out int[] values);

            var first = ruleDict.GetOrAdd(values[0], () => new Rule(values[0]));
            first.MustBeBefore.Add(values[1]);

            var second = ruleDict.GetOrAdd(values[1], () => new Rule(values[1]));
            second.MustBeAfter.Add(values[0]);


        }

        i++;

        long result = 0;
        for (; i < lines.Length; i++)
        {
            string? line = lines[i];
            line.ParseMany("", ",", out int[] values);

            
            bool failed = false;
            var disallowed = new HashSet<int>();
            foreach (var value in values)
            {
                if (disallowed.Contains(value))
                {
                    failed = true;
                    break;
                }

                if (ruleDict.TryGetValue(value, out Rule rule))
                {
                    foreach (var addToDisallow in rule.MustBeAfter)
                    {
                        disallowed.Add(addToDisallow);
                    }
                }
            }

            disallowed = new HashSet<int>();
            foreach (var value in values.Reverse())
            {
                if (disallowed.Contains(value))
                {
                    failed = true;
                    break;
                }

                if (ruleDict.TryGetValue(value, out Rule rule))
                {
                    foreach (var addToDisallow in rule.MustBeBefore)
                    {
                        disallowed.Add(addToDisallow);
                    }
                }
            }

            if (!failed)
            {
                result += values[values.Length / 2];
            }

        }

        Console.WriteLine(result);


    }

    public record Rule(int id)
    {
        public HashSet<int> MustBeBefore = new();
        public HashSet<int> MustBeAfter = new();
    }
}