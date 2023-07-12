using System.Collections.Immutable;

namespace Stampings.Models;

public sealed class Timesheet
{
    public Guid Id { get; set; }

    public long Version { get; set; }

    public string Number { get; init; } = string.Empty;

    public ImmutableList<Stamping> Stampings { get; init; } = ImmutableList<Stamping>.Empty;
}