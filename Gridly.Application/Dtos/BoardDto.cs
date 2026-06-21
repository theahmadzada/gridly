namespace Loot.Application.Dtos;

public record BoardDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
};