using ApplicationCore.Interfaces;
using ApplicationCore.ValueObjects;

namespace ApplicationCore.Services;

public sealed class ScoringService: IScoring
{
    public ScaledScore Scale(RawScore rawScore)
    {
        return rawScore.Scale();
    }

    public ScaledScore Scale(Score score)
    {
        var rawScore = new RawScore(score);
        return Scale(rawScore);
    }

    public RankedCulture Rank(ScaledScore score)
    {
        var arr = new List<CultureScore> { score.Collaborate, score.Create, score.Compete, score.Control };
        arr.Sort();
        return new RankedCulture
        (
            arr[3].Culture,
            arr[2].Culture,
            arr[1].Culture,
            arr[0].Culture
        );
    }
}