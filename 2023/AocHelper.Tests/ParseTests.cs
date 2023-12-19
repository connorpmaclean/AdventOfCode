namespace AocHelper.Tests;
using AocHelper;

public class ParseTests
{
    [Theory]
    [InlineData("abcFGHjkl", "")]
    [InlineData("abcFGHjklabc", "abc")]
    [InlineData("jklabcFGHjkl", "")]
    public void ParseTests_SuffixPrefix_String(string test, string remainder)
    {
        string testRemainder = test.Parse("abc", out string value, "jkl");
        Assert.Equal("FGH", value);
        Assert.Equal(remainder, testRemainder);
    }

    [Theory]
    [InlineData("abc124jkl", "")]
    [InlineData("abc124jklabc", "abc")]
    [InlineData("jklabc124jkl", "")]
    public void ParseTests_SuffixPrefix_Int(string test, string remainder)
    {
        string testRemainder = test.Parse("abc", out int value, "jkl");
        Assert.Equal(124, value);
        Assert.Equal(remainder, testRemainder);
    }

    [Theory]
    [InlineData("abc124", "")]
    [InlineData("hjdsabc124", "")]
    public void ParseTests_PrefixOnly(string test, string remainder)
    {
        string testRemainder = test.Parse("abc", out int value);
        Assert.Equal(124, value);
        Assert.Equal(remainder, testRemainder);
    }

    [Theory]
    [InlineData("124jkl", "")]
    [InlineData("124jklabc", "abc")]
    public void ParseTests_SuffixOnly(string test, string remainder)
    {
        string testRemainder = test.Parse("", out int value, "jkl");
        Assert.Equal(124, value);
        Assert.Equal(remainder, testRemainder);
    }
}