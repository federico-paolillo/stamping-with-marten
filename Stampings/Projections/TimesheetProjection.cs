using Marten.Events.Aggregation;
using Stampings.Events;
using Stampings.Models;
using Stampings.Reducer;

namespace Stampings.Projections;

public sealed class TimesheetProjection : SingleStreamProjection<Timesheet>
{
    public Timesheet Create(EmployeeHired @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        return new Timesheet
        {
            Number = @event.Number
        };
    }

    public Timesheet Apply(StampedIn @event, Timesheet timesheet)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(timesheet);

        var newStampingEntry = new Stamping
        {
            Day = @event.Day,
            Direction = Direction.In,
            Id = @event.Id,
            Time = @event.Time
        };

        var newStampingsList = timesheet.Stampings.Add(newStampingEntry);

        return new Timesheet
        {
            Number = timesheet.Number,
            Stampings = newStampingsList,
        };
    }

    public Timesheet Apply(StampedOut @event, Timesheet timesheet)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(timesheet);

        var newStampingEntry = new Stamping
        {
            Day = @event.Day,
            Direction = Direction.Out,
            Id = @event.Id,
            Time = @event.Time
        };

        var newStampingsList = timesheet.Stampings.Add(newStampingEntry);
        
        return new Timesheet
        {
            Number = timesheet.Number,
            Stampings = newStampingsList,
        };
    }

    public Timesheet Apply(StampingDeleted @event, Timesheet timesheet)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(timesheet);

        var newStampingsList = StampingsReducer.Apply(@event, timesheet.Stampings);
        
        return new Timesheet
        {
            Number = timesheet.Number,
            Stampings = newStampingsList,
        };
    }

    public Timesheet Apply(StampingCorrected @event, Timesheet timesheet)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(timesheet);

        var newStampingsList = StampingsReducer.Apply(@event, timesheet.Stampings);
        
        return new Timesheet
        {
            Number = timesheet.Number,
            Stampings = newStampingsList,
        }; 
    }
}