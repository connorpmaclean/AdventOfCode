
internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2.Run();
    }

    public class Problem2
    {
        public static async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Data.aoc");

            Dictionary<string, IList<Rule>> rules = new Dictionary<string, IList<Rule>>();

            int lineIndex = 0;
            var partList = new List<Dictionary<string, long>>();
            foreach (var line in lines)
            {
                lineIndex++;
                if (line == "")
                {
                    break;
                }

                var ruleStart = line.IndexOf('{');
                string name = line.Substring(0, ruleStart);

                string[] rulesRaw = line[(ruleStart + 1)..^1].Split(",");
                var rulesList = new List<Rule>();
                rules[name] = rulesList;

                for (int i = 0; i < rulesRaw.Length - 1; i++)
                {
                    string ruleRaw = rulesRaw[i];
                    var comp = ruleRaw[0].ToString();
                    var cond = ruleRaw[1].ToString();
                    var valueRaw = ruleRaw[2..(ruleRaw.IndexOf(":"))];
                    var value = int.Parse(valueRaw);
                    var result = ruleRaw[(ruleRaw.IndexOf(":") + 1)..^0];

                    var rule = new Rule(comp, value, cond, result);
                    rulesList.Add(rule);
                }

                var finalRule = new Rule(null, default, null, rulesRaw.Last());
                rulesList.Add(finalRule);
            }

            Node current = new Node();
            current.Ranges = new()
            {
                {"a", (1, 4000) },
                {"m", (1, 4000) },
                {"s", (1, 4000) },
                {"x", (1, 4000) }
            };

            var outcome = RunRuleSet(current, "in", rules);
            Console.WriteLine(outcome);
        }

        public static long RunRuleSet(Node current, string ruleName, Dictionary<string, IList<Rule>> ruleSets)
        {
            if (ruleName == "A")
            {
                long localOptions = 1;
                foreach (var range in current.Ranges.Values)
                {
                    localOptions *= (range.end - range.start + 1);
                }

                return localOptions;
            }
            else if (ruleName == "R") 
            {
                return 0;
            }

            var ruleSet = ruleSets[ruleName];
            long value = 0;
            foreach (var rule in ruleSet)
            {
                if (rule.Condition == null)
                {
                    value += RunRuleSet(current, rule.Result, ruleSets);
                }
                else
                {
                    (int start, int end) = current.Ranges[rule.Comp];
                    if (rule.Condition == ">")
                    {
                        int nextStart = rule.Value + 1;
                        
                        Node next = new Node(current, rule.Comp, nextStart, end);
                        value += RunRuleSet(next, rule.Result, ruleSets);
                        if (start >= nextStart)
                        {
                            break; // double check
                        }

                        current.Ranges[rule.Comp] = (start, nextStart - 1);
                    }
                    else // <
                    {
                        int nextEnd = rule.Value - 1;

                        Node next = new Node(current, rule.Comp, start, nextEnd);
                        value += RunRuleSet(next, rule.Result, ruleSets);
                        if (end <= nextEnd)
                        {
                            break; // double check
                        }

                        current.Ranges[rule.Comp] = (nextEnd + 1, end);
                    }
                }
            }

            return value;
        }

        public class Node
        {
            public Dictionary<string, (int start, int end)> Ranges = new();

            public Node()
            {

            }

            public Node(Node current, string comp, int start, int end)
            {
                Ranges = new Dictionary<string, (int start, int end)>(current.Ranges);
                Ranges[comp] = (start, end);
            }
        }

        public class Rule
        {
            //Func<long, long, string?> Func;
            public string Comp;
            public string? Condition;
            public string Result;
            public int Value;

            public Rule(string comp, int value, string condition, string result)
            {
                Comp = comp;
                Value = value;
                Condition = condition;
                Result = result;
                //if (condition == ">")
                //{
                //    Func = (x, y) => x > y ? result : null;
                //}
                //else if (condition == "<")
                //{
                //    Func = (x, y) => x < y ? result : null;
                //}
                //else
                //{
                //    Func = (_, _) => result;
                //}
            }

            //public string RunRule(Dictionary<string, long> part)
            //{
            //    if (Comp == null)
            //    {
            //        return Func(0, Value);
            //    }

            //    long testValue = part[Comp];
            //    return Func(testValue, Value);
            //}
        }
    }

    public class Problem1 
    {
        public static async Task Run()
        {
            var lines = await File.ReadAllLinesAsync("Data.aoc");

            Dictionary<string, IList<Rule>> rules = new Dictionary<string, IList<Rule>>();

            int lineIndex = 0;
            var partList = new List<Dictionary<string, long>>();
            foreach (var line in lines)
            {
                lineIndex++;
                if (line == "")
                {
                    break;
                }

                var ruleStart = line.IndexOf('{');
                string name = line.Substring(0, ruleStart);

                string[] rulesRaw = line[(ruleStart + 1)..^1].Split(",");
                var rulesList = new List<Rule>();
                rules[name] = rulesList;

                for (int i = 0; i < rulesRaw.Length - 1; i++)
                {
                    string ruleRaw = rulesRaw[i];
                    var comp = ruleRaw[0].ToString();
                    var cond = ruleRaw[1].ToString();
                    var valueRaw = ruleRaw[2..(ruleRaw.IndexOf(":"))];
                    var value = long.Parse(valueRaw);
                    var result = ruleRaw[(ruleRaw.IndexOf(":") + 1)..^0];

                    var rule = new Rule(comp, value, cond, result);
                    rulesList.Add(rule);
                }

                var finalRule = new Rule(null, default, null, rulesRaw.Last());
                rulesList.Add(finalRule);
            }

            foreach (string line in lines[lineIndex..^0])
            {
                Dictionary<string, long> part = new();
                string[] split = line.Replace("{", "").Replace("}", "").Split(",");
                foreach (string comp in split)
                {
                    string compName = comp[0].ToString();
                    string rawValue = comp[2..^0];
                    long value = long.Parse(rawValue);
                    part.Add(compName, value);
                }

                partList.Add(part);
            }

            long outcome = 0;
            foreach (var part in partList)
            {
                var currentRules = rules["in"];
                bool partComplete = false;
                while (!partComplete)
                {
                    foreach (var rule in currentRules)
                    {
                        string result = rule.RunRule(part);
                        if (result == null)
                        {
                            continue;
                        }
                        else if (result == "A")
                        {
                            partComplete = true;
                            outcome += part.Values.Sum();
                            break;
                        }
                        else if (result == "R")
                        {
                            partComplete = true;
                            break;
                        }
                        else
                        {
                            currentRules = rules[result];
                            break;
                        }
                    }
                }

            }

            Console.WriteLine(outcome);
        }


        public class Rule
        {
            Func<long, long, string?> Func;
            string _comp;
            long _value;

            public Rule(string comp, long value, string condition, string result)
            {
                _comp = comp;
                _value = value;
                if (condition == ">")
                {
                    Func = (x, y) => x > y ? result : null;
                }
                else if (condition == "<")
                {
                    Func = (x, y) => x < y ? result : null;
                }
                else
                {
                    Func = (_, _) => result;
                }
            }

            public string RunRule(Dictionary<string, long> part)
            {
                if (_comp == null)
                {
                    return Func(0, _value);
                }

                long testValue = part[_comp];
                return Func(testValue, _value);
            }
        }
    }

}