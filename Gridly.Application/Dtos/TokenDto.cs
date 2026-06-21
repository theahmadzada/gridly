namespace Loot.Application.Dtos;

public record TokenDto
{
    public required string Token { get; init; }
    public DateTimeOffset Expires { get; init; }
}