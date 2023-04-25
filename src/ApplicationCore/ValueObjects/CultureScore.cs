using ApplicationCore.Enums;

namespace ApplicationCore.ValueObjects;

public struct CultureScore: IComparable<CultureScore>
{
    private const float MinScalingFactor = 0.000000001f;

    public uint Value { get; set; }
    public Culture Culture { get; set; }

    public CultureScore(Culture culture, uint value)
    {
        Culture = culture;
        Value = value;
    }

    /// <summary>
    /// Scale the score
    /// </summary>
    /// <param name="scalingFactor"></param>
    /// <returns></returns>
    public CultureScore Scale(float scalingFactor)
    {
        if (scalingFactor <= MinScalingFactor)
            throw new ArgumentException(nameof(scalingFactor));

        var scaledScore = Math.Round(Value / scalingFactor);
        
        return new CultureScore(Culture, Convert.ToUInt32(scaledScore));
    }
    
    /// <summary>
    /// Default to ascending
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(CultureScore other) => 
        Value.CompareTo(other.Value); // no ties breaker => in case of equal value Culture order isn't specified
}