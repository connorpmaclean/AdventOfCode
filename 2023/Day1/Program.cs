// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

await Problem1();
await Problem2();
await Problem2Improved();

static async Task Problem1()
{
    var lines = await File.ReadAllLinesAsync("./data1.aoc");

    int sum = 0;
    foreach (var line in lines)
    {
        foreach (char c in line)
        {
            if (char.IsDigit(c))
            {
                sum += (c - '0') * 10;
                break;
            }
        }

        foreach (char c in line.Reverse())
        {
            if (char.IsDigit(c))
            {
                sum += c - '0';
                break;
            }
        }
    }

    Console.WriteLine(sum);
}

static async Task Problem2()
{
    var forward = new Regex(@"(zero|one|two|three|four|five|six|seven|eight|nine|[0-9])", options: RegexOptions.IgnoreCase | RegexOptions.Compiled);
    var reverse = new Regex(@"(orez|eno|owt|eerht|ruof|evif|xis|neves|thgie|enin|[0-9])", options: RegexOptions.IgnoreCase | RegexOptions.Compiled);

    var lines = await File.ReadAllLinesAsync("./data1.aoc");

    int sum = 0;
    foreach (var line in lines)
    {
        var forwardCapture = forward.Match(line);
        int forwardValue = TranslateCapture(forwardCapture.Value);

        sum += 10 * forwardValue;

        string reverseLine = new string(line.ToCharArray().Reverse().ToArray());
        var backwardCapture = reverse.Match(reverseLine);
        int backwardValue = TranslateCapture(backwardCapture.Value);

        sum += backwardValue;
        //Console.WriteLine($"{line}, {forwardValue}, {backwardValue}");
    }

    Console.WriteLine(sum);

    static int TranslateCapture(string capture)
    {
        switch (capture)
        {
            case "0":
            case "zero":
            case "orez":
                return 0;
            case "1":
            case "one":
            case "eno":
                return 1;
            case "2":
            case "two":
            case "owt":
                return 2;
            case "3":
            case "three":
            case "eerht":
                return 3;
            case "4":
            case "four":
            case "ruof":
                return 4;
            case "5":
            case "five":
            case "evif":
                return 5;
            case "6":
            case "six":
            case "xis":
                return 6;
            case "7":
            case "seven":
            case "neves":
                return 7;
            case "8":
            case "eight":
            case "thgie":
                return 8;
            case "9":
            case "nine":
            case "enin":
                return 9;
            default:
                throw new InvalidOperationException(capture);
        }
    }
}

static async Task Problem2Improved()
{
    string regexSearch = @"(zero|one|two|three|four|five|six|seven|eight|nine|[0-9])";

    var forward = new Regex(regexSearch, options: RegexOptions.IgnoreCase | RegexOptions.Compiled);
    var reverse = new Regex(regexSearch, options: RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.RightToLeft);

    var lines = await File.ReadAllLinesAsync("./data1.aoc");

    int sum = 0;
    foreach (var line in lines)
    {
        var forwardCapture = forward.Match(line);
        int forwardValue = TranslateCapture(forwardCapture.Value);

        sum += 10 * forwardValue;

        var backwardCapture = reverse.Match(line);
        int backwardValue = TranslateCapture(backwardCapture.Value);

        sum += backwardValue;
    }

    Console.WriteLine(sum);

    static int TranslateCapture(string capture)
    {
        switch (capture)
        {
            case "0":
            case "zero":
                return 0;
            case "1":
            case "one":
                return 1;
            case "2":
            case "two":
                return 2;
            case "3":
            case "three":
                return 3;
            case "4":
            case "four":
                return 4;
            case "5":
            case "five":
                return 5;
            case "6":
            case "six":
                return 6;
            case "7":
            case "seven":
                return 7;
            case "8":
            case "eight":
                return 8;
            case "9":
            case "nine":
                return 9;
            default:
                throw new InvalidOperationException(capture);
        }
    }
}