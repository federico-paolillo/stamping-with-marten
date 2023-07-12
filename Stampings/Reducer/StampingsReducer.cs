using System.Collections.Immutable;
using Stampings.Events;
using Stampings.Models;

namespace Stampings.Reducer;

public static class StampingsReducer
{
    public static ImmutableList<Stamping> Apply(StampedIn @event, ImmutableList<Stamping> stampings)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(stampings);

        var newStampingEntry = new Stamping
        {
            Day = @event.Day,
            Direction = Direction.In,
            Id = @event.Id,
            Time = @event.Time
        };

        return stampings.Add(newStampingEntry);
    }

    public static ImmutableList<Stamping> Apply(StampedOut @event, ImmutableList<Stamping> stampings)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(stampings);

        var newStampingEntry = new Stamping
        {
            Day = @event.Day,
            Direction = Direction.Out,
            Id = @event.Id,
            Time = @event.Time
        };

        return stampings.Add(newStampingEntry);
    }

    public static ImmutableList<Stamping> Apply(StampingDeleted @event, ImmutableList<Stamping> stampings)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(stampings);

        var stampingToDeleteIndex = stampings.FindIndex(x => x.Id == @event.Stamping);

        return stampings.RemoveAt(stampingToDeleteIndex);
    }

    public static ImmutableList<Stamping> Apply(StampingCorrected @event, ImmutableList<Stamping> stampings)
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(stampings);

        var stampingToChange = stampings.Find(x => x.Id == @event.Stamping);

        if (stampingToChange is null)
        {
            return stampings;
        }

        var stampingChanged = new Stamping
        {
            Id = stampingToChange.Id,
            Day = stampingToChange.Day,
            Direction = stampingToChange.Direction,
            Time = @event.Time
        };

        var newStampingsList = stampings.Remove(stampingToChange);

        return newStampingsList.Add(stampingChanged);
    }
}