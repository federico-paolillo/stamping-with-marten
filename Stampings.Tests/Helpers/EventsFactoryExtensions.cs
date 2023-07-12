using Marten;
using Stampings.Events;

namespace Stampings.Tests.Helpers;

public static class EventsFactoryExtensions
{
    public static Guid HireEmployee(this IDocumentSession session, string employeeNumber)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(employeeNumber);

        var employeeId = Guid.NewGuid();

        var @event = new EmployeeHired
        {
            Id = employeeId,
            Number = employeeNumber
        };

        session.Events.Append(employeeId, @event);

        return employeeId;
    }

    public static Guid StampIn(this IDocumentSession session, Guid employee, DateOnly date, TimeOnly time)
    {
        ArgumentNullException.ThrowIfNull(session);

        var stampingId = Guid.NewGuid();

        var @event = new StampedIn
        {
            Day = date,
            Time = time,
            Id = stampingId,
            Employee = employee
        };

        session.Events.Append(employee, @event);

        return stampingId;
    }

    public static Guid StampOut(this IDocumentSession session, Guid employee, DateOnly date, TimeOnly time)
    {
        ArgumentNullException.ThrowIfNull(session);

        var stampingId = Guid.NewGuid();

        var @event = new StampedOut
        {
            Day = date,
            Time = time,
            Id = stampingId,
            Employee = employee
        };

        session.Events.Append(employee, @event);

        return stampingId;
    }

    public static void CorrectStamping(this IDocumentSession session, Guid employee, Guid stamping, TimeOnly newTime)
    {
        ArgumentNullException.ThrowIfNull(session);

        var @event = new StampingCorrected
        {
            Stamping = stamping,
            Employee = employee,
            Time = newTime
        };

        session.Events.Append(employee, @event);
    }

    public static void DeleteStamping(this IDocumentSession session, Guid employee, Guid stamping)
    {
        ArgumentNullException.ThrowIfNull(session);

        var @event = new StampingDeleted
        {
            Stamping = stamping,
            Employee = employee
        };

        session.Events.Append(employee, @event);
    }
}