namespace Stampings.Models;

public sealed class Stamping
{
    public Guid Id { get; init; }

    public Direction Direction { get; init; }

    public DateOnly Day { get; init; }

    public TimeOnly Time { get; init; }
}