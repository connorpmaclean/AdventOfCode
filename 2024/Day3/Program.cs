

using System.Text.RegularExpressions;
using AocHelper;

internal class Program
{

    private const string regexText = "mul\\(\\d{1,3},\\d{1,3}\\)";
    private static void Main(string[] args)
    {
        P2();
    }

    private static void P1()
    {
        //string input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

        string input = File.ReadAllText("./input.aoc");

        var regex = new Regex(regexText, RegexOptions.Compiled);

        MatchCollection matches = regex.Matches(input);

        long result = 0;
        foreach (Match match in matches)
        {
            match.Value.ParseMany("mul(", ",", out long[] values, ")");
            result += values[0] * values[1];
        }

        Console.WriteLine(result);
    }

    private static void P2()
    {
        long result = 0;
        ReadOnlySpan<char> text = "do()" + File.ReadAllText("./input.aoc");
        //ReadOnlySpan<char> text = "do()xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        while (text.Length > 0)
        {
            text = GetSubstringBetween(text, "do()", "don't()", out var target);

            foreach (ValueMatch m in Regex.EnumerateMatches(target, regexText))
            {
                target.Slice(m.Index, m.Length).ToString().ParseMany("mul(", ",", out long[] values, ")");
                result += values[0] * values[1];
            }

            text = GetSubstringBetween(text, "don't()", "do()", out _);
        }

        Console.WriteLine(result);
        
    }

    static ReadOnlySpan<char> GetSubstringBetween(ReadOnlySpan<char> text, string start, string end, out ReadOnlySpan<char> target)
    {
        int startIndex = text.IndexOf(start);
        if (startIndex != -1)
        {
            startIndex += start.Length;
        }
        else
        {
            startIndex = 0;
        }

        text = text.Slice(startIndex);
        int endIndex = text.IndexOf(end);
        if (endIndex == -1)
        {
            target = text;
            return ReadOnlySpan<char>.Empty;
        }

        target = text.Slice(0, endIndex);
        return text.Slice(endIndex);
    }
}