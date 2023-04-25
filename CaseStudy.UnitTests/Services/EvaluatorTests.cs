using System.Collections;
using ApplicationCore.Enums;
using ApplicationCore.Services;
using ApplicationCore.ValueObjects;
using Xunit;

namespace CaseStudy.UnitTests.Services;

public sealed class EvaluatorTests
{
    [Theory]
    [ClassData(typeof(CheckAllZeroRuleTestData))]
    public void CheckAllZeroRule_Test(EvaluationResult expected, RawScore rawScore)
    {
        var result = Evaluator.CheckAllZeroRule(rawScore);

        Assert.Equal(expected, result);
    }

    [Theory]
    [ClassData(typeof(CheckAllLowScoreRuleTestData))]
    public void CheckAllLowScoreRule_Test(EvaluationResult expected, RawScore rawScore)
    {
        var result = Evaluator.CheckAllLowScoreRule(rawScore);

        Assert.Equal(expected, result);
    }

    internal sealed class CheckAllZeroRuleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                EvaluationResult.Applied,
                new RawScore(new Score {Control = 0, Collaborate = 0, Compete = 0, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 1, Collaborate = 0, Compete = 0, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 1, Compete = 0, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 0, Compete = 1, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 0, Compete = 0, Create = 1})
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal sealed class CheckAllLowScoreRuleTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                EvaluationResult.Applied,
                new RawScore(new Score {Control = 9, Collaborate = 9, Compete = 9, Create = 9})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 10, Collaborate = 0, Compete = 0, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 10, Compete = 0, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 0, Compete = 10, Create = 0})
            };

            yield return new object[] {
                EvaluationResult.FailedChecks,
                new RawScore(new Score {Control = 0, Collaborate = 0, Compete = 0, Create = 10})
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}