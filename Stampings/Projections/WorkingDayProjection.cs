using Marten.Events.Aggregation;
using Stampings.Events;
using Stampings.Models;
using Stampings.Reducer;

namespace Stampings.Projections;

public sealed class WorkingDayProjection : SingleStreamProjection<WorkingDay>
{
    public WorkingDay Apply(StampedIn @event, WorkingDay workingDay)
    {
        ArgumentNullException.ThrowIfNull(workingDay);
        ArgumentNullException.ThrowIfNull(@event);
        
        if (@event.Day != workingDay.Day)
        {
            return workingDay;
        }

        var newStampingsList = StampingsReducer.Apply(@event, workingDay.Stampings);

        return new WorkingDay
        {
            Stampings = newStampingsList,
            Day = workingDay.Day,
            Employee = workingDay.Employee
        };
    }
    
    public WorkingDay Apply(StampedOut @event, WorkingDay workingDay)
    {
        ArgumentNullException.ThrowIfNull(workingDay);
        ArgumentNullException.ThrowIfNull(@event);
        
        if (@event.Day != workingDay.Day)
        {
            return workingDay;
        }

        var newStampingsList = StampingsReducer.Apply(@event, workingDay.Stampings);

        return new WorkingDay
        {
            Stampings = newStampingsList,
            Day = workingDay.Day,
            Employee = workingDay.Employee
        };
    }
    
    public WorkingDay Apply(StampingCorrected @event, WorkingDay workingDay)
    {
        ArgumentNullException.ThrowIfNull(workingDay);
        ArgumentNullException.ThrowIfNull(@event);

        var stampingToChange = workingDay.Stampings.Find(x => x.Id == @event.Stamping);

        if (stampingToChange is null)
        {
            return workingDay;
        }
        
        if (stampingToChange.Day != workingDay.Day)
        {
            return workingDay;
        }

        var newStampingsList = StampingsReducer.Apply(@event, workingDay.Stampings);

        return new WorkingDay
        {
            Stampings = newStampingsList,
            Day = workingDay.Day,
            Employee = workingDay.Employee
        };
    }
    
    public WorkingDay Apply(StampingDeleted @event, WorkingDay workingDay)
    {
        ArgumentNullException.ThrowIfNull(workingDay);
        ArgumentNullException.ThrowIfNull(@event);
        
        var stampingToDelete = workingDay.Stampings.Find(x => x.Id == @event.Stamping);

        if (stampingToDelete is null)
        {
            return workingDay;
        }
        
        if (stampingToDelete.Day != workingDay.Day)
        {
            return workingDay;
        }

        var newStampingsList = StampingsReducer.Apply(@event, workingDay.Stampings);

        return new WorkingDay
        {
            Stampings = newStampingsList,
            Day = workingDay.Day,
            Employee = workingDay.Employee
        };
    }
}