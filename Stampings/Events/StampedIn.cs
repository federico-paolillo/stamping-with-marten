namespace Stampings.Events;

public sealed class StampedIn
{
    public Guid Id { get; init; }
    
    public Guid Employee { get; init; }
    
    public DateOnly Date { get; init; }
    
    public TimeOnly Time { get; init; }
}