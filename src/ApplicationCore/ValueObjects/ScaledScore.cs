namespace ApplicationCore.ValueObjects;

public sealed record ScaledScore
(
    CultureScore Collaborate,
    CultureScore Create,
    CultureScore Compete,
    CultureScore Control
);