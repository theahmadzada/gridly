using Loot.Domain.Enums;

namespace Loot.Domain.Entities;

public class BoardMember
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public Guid UserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public BoardRole Role { get; set; }
}