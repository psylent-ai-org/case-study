using ApplicationCore.Enums;
using ApplicationCore.Services;
using ApplicationCore.ValueObjects;

namespace ApplicationCore.Tests;

public class ScoringTests
{
    [Theory]
    [InlineData(151, 160, 140, 50, 94, 100, 88, 31)]
    [InlineData(100, 100, 100, 100, 100, 100, 100, 100)]
    [InlineData(50, 50, 50, 200, 25, 25, 25, 100)]
    public void Test_ScaleRawScore(int col, int crt, int cmp, int ctrl, int expCol, int expCrt, int expCmp, int expCtrl)
    {
        // Arrange
        var inputScore = new Score
        {
            Collaborate = col,
            Create = crt,
            Compete = cmp,
            Control = ctrl
        };

        var expectedScaledScore = new ScaledScore
        (
            collaborate: new CultureScore { Culture = Culture.Collaborate, Value = Convert.ToUInt32(expCol) },
            create: new CultureScore { Culture = Culture.Create, Value = Convert.ToUInt32(expCrt) },
            compete: new CultureScore { Culture = Culture.Compete, Value = Convert.ToUInt32(expCmp) },
            control: new CultureScore { Culture = Culture.Control, Value = Convert.ToUInt32(expCtrl) }
        );

        var scoringService = new ScoringService();

        // Act
        var rawScore = new RawScore(inputScore);
        var actualScaledScore = scoringService.Scale(rawScore);

        // Assert
        Assert.Equal(expectedScaledScore.Collaborate.Value, actualScaledScore.Collaborate.Value);
        Assert.Equal(expectedScaledScore.Create.Value, actualScaledScore.Create.Value);
        Assert.Equal(expectedScaledScore.Compete.Value, actualScaledScore.Compete.Value);
        Assert.Equal(expectedScaledScore.Control.Value, actualScaledScore.Control.Value);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, EvaluationResult.Applied)]
    [InlineData(1, 0, 0, 0, EvaluationResult.FailedChecks)]
    [InlineData(0, 0, 1, 0, EvaluationResult.FailedChecks)]
    public void Test_CheckAllZeroRule(int col, int crt, int cmp, int ctrl, EvaluationResult expectedResult)
    {
        // Arrange
        var score = new Score
        {
            Collaborate = col,
            Create = crt,
            Compete = cmp,
            Control = ctrl
        };
        var rawScore = new RawScore(score);

        // Act
        var result = Evaluator.CheckAllZeroRule(rawScore);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(9, 5, 8, 3, EvaluationResult.Applied)]
    [InlineData(10, 5, 8, 3, EvaluationResult.FailedChecks)]
    [InlineData(9, 11, 8, 3, EvaluationResult.FailedChecks)]
    public void Test_CheckAllLowScoreRule(int col, int crt, int cmp, int ctrl, EvaluationResult expectedResult)
    {
        // Arrange
        var score = new Score
        {
            Collaborate = col,
            Create = crt,
            Compete = cmp,
            Control = ctrl
        };
        var rawScore = new RawScore(score);

        // Act
        var result = Evaluator.CheckAllLowScoreRule(rawScore);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}