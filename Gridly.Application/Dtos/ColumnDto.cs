namespace Gridly.Application.Dtos;

public record ColumnDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}