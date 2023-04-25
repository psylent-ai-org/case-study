namespace ApplicationCore.ValueObjects;

public struct Score
{
    public uint Collaborate { get; set; }
    public uint Create { get; set; }
    public uint Compete { get; set; }
    public uint Control { get; set; }
}