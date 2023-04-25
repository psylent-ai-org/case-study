using ApplicationCore.Interfaces;
using ApplicationCore.ValueObjects;

namespace ApplicationCore.Services;

public class ScoringService: IScoring
{
    public ScaledScore Scale(RawScore rawScore)
    {
        return rawScore.Scale();
    }

    public ScaledScore Scale(Score score)
    {
        var raw = new RawScore(score);
        return raw.Scale();
    }

    public RankedCulture Rank(ScaledScore score)
    {
        var arr = new List<CultureScore> { score.Collaborate, score.Create, score.Compete, score.Control };
        arr = arr.OrderByDescending(x=>x).ToList();
        return new RankedCulture
        {
            First = arr[0].Culture,
            Second = arr[1].Culture,
            Third = arr[2].Culture,
            Fourth = arr[3].Culture,
        };
    }
}