using System.Collections.Concurrent;
using FPIS.ErosRamazzottiTicketBooking.Api.Utility;
using Xunit;

namespace FPIS.Api.Tests.Utility;

public class TokenGeneratorTests
{
    private const string ExpectedAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    [Fact]
    public void GenerateTokenValue_DefaultLength_ReturnsTenCharacters()
    {
        var token = TokenGenerator.GenerateTokenValue();
        Assert.Equal(10, token.Length);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(64)]
    public void GenerateTokenValue_CustomLength_ReturnsExactLength(int length)
    {
        var token = TokenGenerator.GenerateTokenValue(length);
        Assert.Equal(length, token.Length);
    }

    [Fact]
    public void GenerateTokenValue_ZeroLength_ReturnsEmptyString()
    {
        var token = TokenGenerator.GenerateTokenValue(0);
        Assert.Equal(string.Empty, token);
    }

    [Fact]
    public void GenerateTokenValue_AllCharactersFromExpectedAlphabet()
    {
        var token = TokenGenerator.GenerateTokenValue(100);
        Assert.All(token, ch => Assert.Contains(ch, ExpectedAlphabet));
    }

    [Fact]
    public void GenerateTokenValue_NegativeLength_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TokenGenerator.GenerateTokenValue(-1));
    }

    [Fact]
    public void GenerateTokenValue_ParallelCalls_ProduceDistinctTokens()
    {
        var bag = new ConcurrentBag<string>();
        Parallel.For(0, 1000, _ => bag.Add(TokenGenerator.GenerateTokenValue(16)));

        Assert.Equal(1000, bag.Distinct().Count());
    }
}
