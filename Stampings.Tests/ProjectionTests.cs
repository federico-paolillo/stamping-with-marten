using Stampings.Models;
using Stampings.Tests.Helpers;

namespace Stampings.Tests;

public sealed class ProjectionTests : TestBase
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

        var employee = rSession.Events.AggregateStream<Employee>(employeeId);
        
        Assert.That(employee, Is.Not.Null);
        
        Assert.That(employee!.Id, Is.EqualTo(employeeId));
        Assert.That(employee!.Number, Is.EqualTo(employeeNumber));
        Assert.That(employee!.Version, Is.EqualTo(5));
        Assert.That(employee!.Stampings, Has.Count.EqualTo(4));
    }

    [Test]
    public async Task DeletesStampings()
    {
        using var store = GetStore();
        
        await using var wSession = store.LightweightSession();

        const string employeeNumber = "balbhbhaba";
        
        var employeeId = wSession.HireEmployee(employeeNumber);

        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(8, 0));
        var stampingToDelete = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(12, 0));
        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(13, 0));
        _ = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(17, 0));
        wSession.DeleteStamping(employeeId, stampingToDelete);

        await wSession.SaveChangesAsync();

        await using var rSession = store.LightweightSession();

        var employee = rSession.Events.AggregateStream<Employee>(employeeId);
        
        Assert.That(employee, Is.Not.Null);
        
        Assert.That(employee!.Id, Is.EqualTo(employeeId));
        Assert.That(employee!.Number, Is.EqualTo(employeeNumber));
        Assert.That(employee!.Version, Is.EqualTo(6));
        Assert.That(employee!.Stampings, Has.Count.EqualTo(3)); 
        Assert.That(employee!.Stampings.FirstOrDefault(x => x.Id == stampingToDelete), Is.Null);
    }
    
    [Test]
    public async Task CorrectsStampings()
    {
        using var store = GetStore();
        
        await using var wSession = store.LightweightSession();

        const string employeeNumber = "balbhbhaba";
        
        var employeeId = wSession.HireEmployee(employeeNumber);

        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(8, 0));
        var stampingToChange = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(12, 0));
        _ = wSession.StampIn(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(13, 0));
        _ = wSession.StampOut(employeeId, new DateOnly(2023, 07, 11), new TimeOnly(17, 0));
        wSession.CorrectStamping(employeeId, stampingToChange, new TimeOnly(12, 30));

        await wSession.SaveChangesAsync();

        await using var rSession = store.LightweightSession();

        var employee = rSession.Events.AggregateStream<Employee>(employeeId);
        
        Assert.That(employee, Is.Not.Null);
        
        Assert.That(employee!.Id, Is.EqualTo(employeeId));
        Assert.That(employee!.Number, Is.EqualTo(employeeNumber));
        Assert.That(employee!.Version, Is.EqualTo(6));
        Assert.That(employee!.Stampings, Has.Count.EqualTo(4));

        var stampingChanged = employee!.Stampings.First(x => x.Id == stampingToChange);
        
        Assert.That(stampingChanged.Time, Is.EqualTo(new TimeOnly(12, 30)));
    }
}