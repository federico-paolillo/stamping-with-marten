namespace Stampings.Events;

public sealed class StampedOut
{
    public Guid Id { get; init; }

    public Guid Employee { get; init; }

    public DateOnly Day { get; init; }

    public TimeOnly Time { get; init; }
}