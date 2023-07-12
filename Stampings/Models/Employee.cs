using System.Collections.Immutable;
using Stampings.Events;

namespace Stampings.Models;

public sealed class Employee
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Number { get; init; } = string.Empty;

    public List<Stamping> Stampings { get; set; } = new();

    public void Apply(StampedIn @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        
        Version++;
    }

    public void Handle(StampedOut @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        
        
        Version++;
    }

    public void Handle(StampingDeleted @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        
        
        Version++;
    }

    public void Handle(StampingCorrected @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

       

        Version++;
    }
}