namespace Stampings.Events;

public sealed class StampingDeleted
{
    public Guid Employee { get; init; }
    
    public Guid Stamping { get; init; }
}