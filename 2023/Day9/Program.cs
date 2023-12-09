await Problem2();

static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long result = 0;
    foreach (var line in lines)
    {
        var currentLine = line.Split(' ').Select(long.Parse).ToArray();

        Stack<long> stack = new Stack<long>();
        while (true)
        {
            var nextLine = new long[currentLine.Length - 1];
            bool isEnd = true;
            for (int i = 0; i < currentLine.Length - 1; i++)
            {
                nextLine[i] = currentLine[i + 1] - currentLine[i];
                if (nextLine[i] != 0)
                {
                    isEnd = false;
                }
            }

            stack.Push(currentLine.Last());

            if (isEnd)
            {
                break;
            }

            currentLine = nextLine;
        }

        result += stack.Sum();
    }


    Console.WriteLine(result);
}

static async Task Problem2()
{
    var lines = await File.ReadAllLinesAsync("./Data1.aoc");

    long result = 0;
    foreach (var line in lines)
    {
        var currentLine = line.Split(' ').Select(long.Parse).ToArray();

        Stack<long> stack = new Stack<long>();
        while (true)
        {
            var nextLine = new long[currentLine.Length - 1];
            bool isEnd = true;
            for (int i = 0; i < currentLine.Length - 1; i++)
            {
                nextLine[i] = currentLine[i + 1] - currentLine[i];
                if (nextLine[i] != 0)
                {
                    isEnd = false;
                }
            }

            stack.Push(currentLine.First());

            if (isEnd)
            {
                break;
            }

            currentLine = nextLine;
        }

        long current = 0;
        while (stack.TryPop(out long nextValue)) 
        {
            current = nextValue - current;
        }

        result += current;
    }

    Console.WriteLine(result);
}