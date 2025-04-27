namespace Shared.DataTransferObjects;

public record ClientDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}