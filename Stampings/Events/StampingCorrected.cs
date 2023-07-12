namespace Stampings.Events;

public sealed class StampingCorrected
{
    public Guid Employee { get; init; }

    public Guid Stamping { get; init; }

    public TimeOnly Time { get; init; }
}