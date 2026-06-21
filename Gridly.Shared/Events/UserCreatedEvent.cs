namespace Loot.Shared.Events;

public record UserCreatedEvent
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Token { get; init; }
}