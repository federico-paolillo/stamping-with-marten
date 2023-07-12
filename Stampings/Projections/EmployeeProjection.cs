using Marten.Events.Aggregation;
using Stampings.Events;
using Stampings.Models;

namespace Stampings.Projections;

public sealed class EmployeeProjection : SingleStreamProjection<Employee>
{
    public Employee Create(EmployeeHired @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new Employee
        {
            Id = @event.Id,
            Number = @event.Number
        };
    }
    
    public void Apply(StampedIn @event, Employee employee)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(employee);

        var newStampingEntry = new Stamping
        {
            Date = @event.Date,
            Direction = Direction.In,
            Id = @event.Id,
            Time = @event.Time
        };

        employee.Stampings.Add(newStampingEntry);
    }

    public void Apply(StampedOut @event, Employee employee)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(employee);
        
        var newStampingEntry = new Stamping
        {
            Date = @event.Date,
            Direction = Direction.Out,
            Id = @event.Id,
            Time = @event.Time
        };

        employee.Stampings.Add(newStampingEntry);
    }

    public void Apply(StampingDeleted @event, Employee employee)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(employee);
        
        var stampingToDeleteIndex = employee.Stampings.FindIndex(x => x.Id == @event.Stamping);

        employee.Stampings.RemoveAt(stampingToDeleteIndex);
    }

    public void Apply(StampingCorrected @event, Employee employee)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(employee);
        
        var stampingToChange = employee.Stampings.Find(x => x.Id == @event.Stamping);

        if (stampingToChange is null)
        {
            return;
        }

        var stampingChanged = new Stamping
        {
            Id = stampingToChange.Id,
            Date = stampingToChange.Date,
            Direction = stampingToChange.Direction,
            Time = @event.Time
        };

        employee.Stampings.Remove(stampingToChange);
        employee.Stampings.Add(stampingChanged);
    }   
}