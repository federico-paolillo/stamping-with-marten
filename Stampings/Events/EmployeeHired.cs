namespace Stampings.Events;

public sealed class EmployeeHired
{
    public Guid Id { get; init; }

    public string Number { get; init; } = string.Empty;
}