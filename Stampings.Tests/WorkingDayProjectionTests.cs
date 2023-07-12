using Stampings.Models;

namespace Stampings.Tests;

public sealed class WorkingDayProjectionTests : TestBase
{
    [Test]
    public async Task AppliesStampingEvents()
    {
        using var store = GetStore();

        await using var wSession = store.LightweightSession();

        const string employeeNumber = "balbhbhaba";

        var employeeId = wSession.HireEmployee(employeeNumber);

        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(8, 0));
        _ = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(12, 0));
        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(13, 0));
        _ = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(17, 0));

        await wSession.SaveChangesAsync();

        await using var rSession = store.LightweightSession();

        var initialWorkingDay = new WorkingDay { Day = new DateOnly(2023, 07, 11) };

        var workingDay = rSession.Events.AggregateStream(employeeId, state: initialWorkingDay);

        Assert.That(workingDay, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(workingDay!.Id, Is.EqualTo(employeeId));
            Assert.That(workingDay!.Version, Is.EqualTo(5));
            Assert.That(workingDay!.Stampings, Has.Count.EqualTo(4));
            Assert.That(workingDay!.HoursWorked(), Is.EqualTo(8d));
        }); 
    }
}