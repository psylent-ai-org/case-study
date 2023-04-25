using ApplicationCore.Enums;
using ApplicationCore.Services;
using ApplicationCore.ValueObjects;
using Xunit;

namespace CaseStudy.UnitTests.Services;

public sealed class ScoringServiceTests
{
    private readonly ScoringService _sut = new();

    [Fact]
    public void Scale_Test()
    {
        var score = new Score
        {
            Control = 50,
            Create = 160,
            Compete = 140,
            Collaborate = 151
        };

        var result = _sut.Scale(score);

        Assert.Equal((uint)31, result.Control.Value);
        Assert.Equal((uint)100, result.Create.Value);
        Assert.Equal((uint)88, result.Compete.Value);
        Assert.Equal((uint)94, result.Collaborate.Value);
    }

    [Fact]
    public void Rank_Test()
    {
        var score = new Score
        {
            Control = 50,
            Create = 160,
            Compete = 140,
            Collaborate = 151
        };

        var scaledScore = _sut.Scale(score);
        var result = _sut.Rank(scaledScore);

        Assert.Equal(Culture.Create, result.First);
        Assert.Equal(Culture.Collaborate, result.Second);
        Assert.Equal(Culture.Compete, result.Third);
        Assert.Equal(Culture.Control, result.Fourth);
    }
}