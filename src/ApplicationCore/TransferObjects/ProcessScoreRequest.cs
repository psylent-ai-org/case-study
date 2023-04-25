using ApplicationCore.ValueObjects;

namespace ApplicationCore.TransferObjects;

public sealed class ProcessScoreRequest
{
    public Score Data { get; set; }
}