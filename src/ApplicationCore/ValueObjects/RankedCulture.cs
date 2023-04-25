using ApplicationCore.Enums;

namespace ApplicationCore.ValueObjects;

public sealed record RankedCulture
(
    Culture First,
    Culture Second,
    Culture Third,
    Culture Fourth
);