using ApplicationCore.Enums;

namespace ApplicationCore.ValueObjects;

public sealed class RawScore
{
    public CultureScore Collaborate { get; set; }
    public CultureScore Create { get; set; }
    public CultureScore Compete { get; set; }
    public CultureScore Control { get; set; }

    public RawScore()
    {
    }

    public RawScore(Score inputScore)
    {
        Collaborate = new CultureScore(Culture.Collaborate, inputScore.Collaborate);
        Create = new CultureScore(Culture.Create, inputScore.Create);
        Compete = new CultureScore(Culture.Compete, inputScore.Compete);
        Control = new CultureScore(Culture.Control, inputScore.Control);
    }

    public override string ToString() =>
        $"Collaborate: {Collaborate.Value}, Create: {Create.Value}, Compete: {Compete.Value}, Control: {Control.Value}";

    /// <summary>
    /// Scaling the score
    /// </summary>
    /// <returns></returns>
    public ScaledScore Scale()
    {
        var maxScore = MaxScore();
        if (maxScore == 0)
        {
            return new ScaledScore(
                new CultureScore(Culture.Collaborate, 100),
                new CultureScore(Culture.Create, 100),
                new CultureScore(Culture.Compete, 100),
                new CultureScore(Culture.Control, 100)
            );
        }

        var scalingFactor = maxScore / 100f;

        return new ScaledScore
        (
            Collaborate.Scale(scalingFactor),
            Create.Scale(scalingFactor),
            Compete.Scale(scalingFactor),
            Control.Scale(scalingFactor)
        );
    }

    /// <summary>
    /// Get max score from all cultures
    /// </summary>
    /// <returns></returns>
    private uint MaxScore()
    {
        var max1 = Math.Max(Collaborate.Value, Create.Value);
        var max2 = Math.Max(Compete.Value, Control.Value);
        return Math.Max(max1, max2);
    }
}