using JasperFx.CodeGeneration;
using Marten;
using Marten.Events.Projections;
using Stampings.Events;
using Stampings.Projections;
using Weasel.Core;

namespace Stampings;

public sealed class CustomMartenOptions : StoreOptions
{
    public CustomMartenOptions()
    {
        Connection("Host=localhost;Port=65432;Database=sampling;Username=postgres;Password=adminADMIN1234!;");

        Events.MetadataConfig.HeadersEnabled = false;
        Events.MetadataConfig.CausationIdEnabled = false;
        Events.MetadataConfig.CausationIdEnabled = false;

        Events.AddEventType<EmployeeHired>();
        Events.AddEventType<StampedIn>();
        Events.AddEventType<StampedOut>();
        Events.AddEventType<StampingCorrected>();
        Events.AddEventType<StampingDeleted>();

        AutoCreateSchemaObjects = AutoCreate.All;
        
        GeneratedCodeMode = TypeLoadMode.Dynamic;
        
        Projections.Add<TimesheetProjection>(ProjectionLifecycle.Live);
        Projections.Add<WorkingDayProjection>(ProjectionLifecycle.Live);
    }
}