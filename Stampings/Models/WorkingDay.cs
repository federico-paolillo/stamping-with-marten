using System.Collections.Immutable;

namespace Stampings.Models;

public sealed class WorkingDay
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public ImmutableList<Stamping> Stampings { get; init; } = ImmutableList<Stamping>.Empty;

    public Guid Employee { get; init; }

    public DateOnly Day { get; init; }

    public double HoursWorked()
    {
        var ins = Stampings.Where(x => x.Direction == Direction.In);
        var outs = Stampings.Where(x => x.Direction == Direction.Out);

        var stampingPairs = ins.Zip(outs);

        var differences = stampingPairs.Select(tuple => tuple.Second.Time - tuple.First.Time);

        var total = differences.Sum(x => x.TotalHours);

        return total;
    }
}