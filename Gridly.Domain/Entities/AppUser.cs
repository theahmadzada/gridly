using Microsoft.AspNetCore.Identity;

namespace Loot.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpires { get; set; }
}