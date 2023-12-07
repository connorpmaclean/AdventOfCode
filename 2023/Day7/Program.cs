


internal class Program
{
    private static async Task Main(string[] args)
    {
        await Problem2();

    }

    public class HandP2
    {
        int[] cards;
        public int bid;
        public long handValue;
        string originalLine;

        public HandP2(string line)
        {
            checked
            {
                originalLine = line;
                var comp = line.Split(' ');
                bid = int.Parse(comp[1]);

                cards = comp[0].Select(GetCardValue).ToArray();
                handValue = GetHandValue();
            }
        }

        public override string ToString()
        {
            return originalLine;
        }

        long GetHandValue()
        {
            

            long multiplier = 1;
            long value = 0;
            foreach (int card in cards.Reverse())
            {
                value += card * multiplier;
                multiplier *= 100;
            }

            var ordering = cards
                .Where(c => c != 1)
                .GroupBy(c => c)
                .Select(c => c.Count())
                .OrderByDescending(c => c).ToList();

            int jokers = cards.Count(c => c == 1);
            if (jokers == 5)
            {
                ordering = new List<int> { 0 };
            }

            ordering[0] += jokers;

            value += multiplier * ordering switch
            {
                [5, ..] => 7,
                [4, ..] => 6,
                [3, 2, ..] => 5,
                [3, 1, ..] => 4,
                [2, 2, ..] => 3,
                [2, 1, ..] => 2,
                _ => 1
            };

            return value;
        }

        static int GetCardValue(char c) => c switch
        {
            >= '2' and <= '9' => c - '0',
            'T' => 10,
            'J' => 1,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
        };
    }

    private static async Task Problem2()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        var rows = lines.Select(line => new HandP2(line));

        var ordered = rows.OrderBy(r => r.handValue).ToList();
        int i = 1;
        long result = 0;
        foreach (var row in ordered)
        {
            result += row.bid * i++;
        }

        Console.WriteLine(result);
    }

    private static async Task Problem1()
    {
        var lines = await File.ReadAllLinesAsync("./Data1.aoc");

        var rows = lines.Select(line => new HandP1(line));

        var ordered = rows.OrderBy(r => r.handValue).ToList();
        int i = 1;
        long result = 0;
        foreach (var row in ordered)
        {
            result += row.bid * i++;
        }

        Console.WriteLine(result);
    }

    public class HandP1
    {
        int[] cards;
        public int bid;
        public long handValue;
        string originalLine;

        public HandP1(string line) 
        {
            checked
            {
                originalLine = line;
                var comp = line.Split(' ');
                bid = int.Parse(comp[1]);

                cards = comp[0].Select(GetCardValue).ToArray();
                handValue = GetHandValue();
            }
        }

        public override string ToString()
        {
            return originalLine;
        }

        long GetHandValue()
        {
            var ordering = cards.GroupBy(c => c).Select(c => c.Count()).OrderByDescending(c => c).ToList();

            long multiplier = 1;
            long value = 0;
            foreach (int card in cards.Reverse())
            {
                value += card * multiplier;
                multiplier *= 100;
            }

            value += multiplier * ordering switch
            {
                [5, ..] => 7,
                [4, ..] => 6,
                [3, 2, ..] => 5,
                [3, 1, ..] => 4,
                [2, 2, ..] => 3,
                [2, 1, ..] => 2,
                _ => 1
            };

            return value;
        }

        static int GetCardValue(char c) => c switch
        {
            >= '2' and <= '9' => c - '0',
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
        };
    }
}