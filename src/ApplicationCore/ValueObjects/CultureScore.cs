using ApplicationCore.Enums;

namespace ApplicationCore.ValueObjects;

public class CultureScore: IComparable<CultureScore>
{
    public uint Value { get; set; }
    public Culture Culture { get; set; }

    
    /// <summary>
    /// Scale the score
    /// </summary>
    /// <param name="scalingFactor"></param>
    /// <returns></returns>
    public CultureScore Scale(float scalingFactor)
    {
        var scaledScore = Math.Round(Value / scalingFactor);
        
        return new CultureScore
        {
            Culture = Culture,
            Value = Convert.ToUInt32(scalingFactor)
        };
    }
    

    /// <summary>
    /// Default to ascending
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(CultureScore? other)
    {
        if (other is null) return 1;
        return (int)(Value - other.Value);
    }
}