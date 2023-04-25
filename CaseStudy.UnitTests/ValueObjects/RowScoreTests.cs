using System.Collections;
using ApplicationCore.Enums;
using ApplicationCore.ValueObjects;
using Xunit;

namespace CaseStudy.UnitTests.ValueObjects;

public sealed class RowScoreTests
{
    [Fact]
    public void Constructor_Test()
    {
        var score = new Score
        {
            Control = 1,
            Create = 2,
            Compete = 3,
            Collaborate = 4
        };

        var sut = new RawScore(score);

        Assert.Equal(score.Control, sut.Control.Value);
        Assert.Equal(score.Create, sut.Create.Value);
        Assert.Equal(score.Compete, sut.Compete.Value);
        Assert.Equal(score.Collaborate, sut.Collaborate.Value);
    }

    [Fact]
    public void ToString_Test()
    {
        var score = new Score
        {
            Control = 1,
            Create = 2,
            Compete = 3,
            Collaborate = 4
        };

        var sut = new RawScore(score);
        var result = sut.ToString();

        Assert.Equal("Collaborate: 4, Create: 2, Compete: 3, Control: 1", result);
    }

    [Theory]
    [ClassData(typeof(ScaleTestData))]
    public void Scale_Test(ScaledScore expected, Score score)
    {
        var sut = new RawScore(score);
        var result = sut.Scale();

        Assert.Equal(expected.Control.Value, result.Control.Value);
        Assert.Equal(expected.Create.Value, result.Create.Value);
        Assert.Equal(expected.Compete.Value, result.Compete.Value);
        Assert.Equal(expected.Collaborate.Value, result.Collaborate.Value);
    }

    internal sealed class ScaleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new ScaledScore(
                    new CultureScore(Culture.Collaborate, 94),
                    new CultureScore(Culture.Create, 100),
                    new CultureScore(Culture.Compete, 88),
                    new CultureScore(Culture.Control, 31)
                ),
                new Score
                {
                    Control = 50,
                    Create = 160,
                    Compete = 140,
                    Collaborate = 151
                }
            };

            yield return new object[]
            {
                new ScaledScore(
                    new CultureScore(Culture.Collaborate, 100),
                    new CultureScore(Culture.Create, 100),
                    new CultureScore(Culture.Compete, 100),
                    new CultureScore(Culture.Control, 100)
                ),
                new Score
                {
                    Control = 1000,
                    Create = 1000,
                    Compete = 1000,
                    Collaborate = 1000
                }
            };

            yield return new object[]
            {
                new ScaledScore(
                    new CultureScore(Culture.Collaborate, 100),
                    new CultureScore(Culture.Create, 100),
                    new CultureScore(Culture.Compete, 100),
                    new CultureScore(Culture.Control, 100)
                ),
                new Score
                {
                    Control = 1,
                    Create = 1,
                    Compete = 1,
                    Collaborate = 1
                }
            };

            yield return new object[]
            {
                new ScaledScore(
                    new CultureScore(Culture.Collaborate, 100),
                    new CultureScore(Culture.Create, 100),
                    new CultureScore(Culture.Compete, 100),
                    new CultureScore(Culture.Control, 100)
                ),
                new Score
                {
                    Control = 0,
                    Create = 0,
                    Compete = 0,
                    Collaborate = 0
                }
            };

            yield return new object[]
            {
                new ScaledScore(
                    new CultureScore(Culture.Collaborate, 100),
                    new CultureScore(Culture.Create, 0),
                    new CultureScore(Culture.Compete, 0),
                    new CultureScore(Culture.Control, 0)
                ),
                new Score
                {
                    Control = 0,
                    Create = 0,
                    Compete = 0,
                    Collaborate = 100
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}