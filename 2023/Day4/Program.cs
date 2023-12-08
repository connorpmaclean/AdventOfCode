await Problem1();
await Problem2();
await Problem2Brian();

static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("data2.aoc");

    int score = 0;
    foreach (var line in lines)
    {
        var comp = line.Split(':');
        var cardSections = comp[1].Split("|");

        var winningNumbers = new HashSet<string>(cardSections[0].Split(" ").Where(x => x != ""));
        var cardNumbers = cardSections[1].Split(" ").Where(x => x != "");

        int numMatch = cardNumbers.Where(n => winningNumbers.Contains(n)).Count();

        score += (int)Math.Pow(2, numMatch - 1);
    }

    Console.WriteLine(score);
}


static async Task Problem2()
{
    var lines = await File.ReadAllLinesAsync("data2.aoc");

    int score = 0;
    Dictionary<int, int> scoreByCard = new();

    Queue<(int, string)> queue = new();

    for (int i = 0; i < lines.Length; i++ )
    {
        queue.Enqueue((i + 1, lines[i]));
    }

    while (queue.TryDequeue(out (int index, string line) nextItem))
    {
        if (!scoreByCard.TryGetValue(nextItem.index, out int cardScore))
        {
            var comp = nextItem.line.Split(':');
            var cardSections = comp[1].Split("|");

            var winningNumbers = new HashSet<string>(cardSections[0].Split(" ").Where(x => x != ""));
            var cardNumbers = cardSections[1].Split(" ").Where(x => x != "");

            cardScore = cardNumbers.Where(n => winningNumbers.Contains(n)).Count();
            scoreByCard.Add(nextItem.index, cardScore);
        }

        for (int i = nextItem.index; i < nextItem.index + cardScore; i++)
        {
            queue.Enqueue((i + 1, lines[i]));
        }

        score++;
    }

    Console.WriteLine(score);
}

static async Task Problem2Brian()
{
    var lines = await File.ReadAllLinesAsync("data2.aoc");
    Dictionary<int, int> cardCounts = new();

    for (var i = 0; i < lines.Length; i++)
    {
        cardCounts[i] = cardCounts.ContainsKey(i) ? cardCounts[i] + 1 : 1;
        var comp = lines[i].Split(':');
        var cardSections = comp[1].Split("|");

        var winningNumbers = new HashSet<string>(cardSections[0].Split(" ").Where(x => x != ""));
        var cardNumbers = cardSections[1].Split(" ").Where(x => x != "");

        var matchCount = cardNumbers.Where(n => winningNumbers.Contains(n)).Count();

        for (var j = 0; j < matchCount; j++)
        {
            var newIndex = i + j + 1;
            cardCounts[newIndex] = cardCounts.ContainsKey(newIndex) ? cardCounts[newIndex] + cardCounts[i] : cardCounts[i];
        }
    }

    var numCards = cardCounts.Sum(x => x.Value);

    Console.WriteLine(numCards);
}