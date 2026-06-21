using Loot.Domain.Entities;

namespace Loot.Application.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }

    public static explicit operator UserDto(AppUser appUser)
    {
        return new UserDto
        {
            Id = appUser.Id,
            Email = appUser.Email!,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName
        };
    }
}