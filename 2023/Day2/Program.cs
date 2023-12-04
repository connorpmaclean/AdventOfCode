
using System.Text.RegularExpressions;

await Problem1();
await Problem2();

static async Task Problem1()
{
    var gameRegex = new Regex("Game ([0-9]+)", RegexOptions.Compiled);
    var colorRegex = new Regex("([0-9]+) ([a-z]+)", RegexOptions.Compiled);

    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    Dictionary<int, bool> results = new();

    foreach (string line in lines)
    {
        var comp = line.Split(':');
        var gameMatch = gameRegex.Match(comp[0]);

        int game = int.Parse(gameMatch.Groups[1].Value);
        if (!results.ContainsKey(game))
        {
            results[game] = true;
        }

        var draws = comp[1].Split(";");

        foreach (var draw in draws)
        {
            var colorMatchs = colorRegex.Matches(draw);

            foreach (Match match in colorMatchs)
            {
                int count = int.Parse(match.Groups[1].Value);
                string color = match.Groups[2].Value;

                bool fail = (color, count) switch
                {
                    ("red", > 12) => false,
                    ("green", > 13) => false,
                    ("blue", > 14) => false,
                    _ => true
                };

                results[game] &= fail;
            }
        }

    }

    int finalSum = results.Where(kvp => kvp.Value).Sum(kvp => kvp.Key);
    Console.WriteLine(finalSum);
}

static async Task Problem2()
{
    var colorRegex = new Regex("([0-9]+) ([a-z]+)", RegexOptions.Compiled);

    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    int sum = 0;

    foreach (string line in lines)
    {
        var comp = line.Split(':');

        var mins = comp[1]
            .Split(";")
            .SelectMany(d => GetCounts(d, colorRegex))
            .GroupBy(pair => pair.color)
            .Select(group => group.Max(g => g.count));

        var product = mins.Aggregate((a, b) => a * b);
        sum += product;
    }

    Console.WriteLine(sum);
}

static IEnumerable<(string color, int count)> GetCounts(string draw, Regex regex)
{
    var colorMatchs = regex.Matches(draw);

    foreach (Match match in colorMatchs)
    {
        int count = int.Parse(match.Groups[1].Value);
        string color = match.Groups[2].Value;

        yield return (color, count);
    }
}