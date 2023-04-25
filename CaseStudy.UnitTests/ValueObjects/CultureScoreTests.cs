using System.Collections;
using ApplicationCore.Enums;
using ApplicationCore.ValueObjects;
using Xunit;

namespace CaseStudy.UnitTests.ValueObjects;

public sealed class CultureScoreTests
{
    [Theory]
    [ClassData(typeof(CompareToTestData))]
    public void CompareTo_Test(int expected, CultureScore first, CultureScore second)
    {
        var result = first.CompareTo(second);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.000000001f)]
    public void ScaleArgumentException_Test(float scoreFactor)
    {
        var input = new CultureScore(Culture.Control, 1);
        Assert.Throws<ArgumentException>(() => input.Scale(scoreFactor));
    }

    [Theory]
    [InlineData(1, 100, 100)]
    public void Scale_Test(uint expected, uint value, float scoreFactor)
    {
        var input = new CultureScore(Culture.Control, value);
        var result = input.Scale(scoreFactor);

        Assert.Equal(Culture.Control, input.Culture);
        Assert.Equal(expected, result.Value);
    }

    internal sealed class CompareToTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                0,
                new CultureScore(Culture.Collaborate, 1),
                new CultureScore(Culture.Create, 1)
            };

            yield return new object[] {
                -1,
                new CultureScore(Culture.Collaborate, 0),
                new CultureScore(Culture.Control, 1)
            };

            yield return new object[] {
                1,
                new CultureScore(Culture.Create, 1),
                new CultureScore(Culture.Compete, 0)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}