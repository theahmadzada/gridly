namespace Loot.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Column> Columns { get; set; } = [];
    public ICollection<BoardMember> Members { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedAt { get; set; }
}