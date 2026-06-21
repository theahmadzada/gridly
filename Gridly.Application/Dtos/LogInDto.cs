namespace Loot.Application.Dtos;

public record LogInDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string RefreshToken { get; init; }
    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
    public required string AccessToken { get; init; }
    public DateTimeOffset AccessTokenExpiresAt { get; init; }
}