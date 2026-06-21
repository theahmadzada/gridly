namespace Loot.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid ColumnId { get; set; }
    public Column Column { get; set; } = null!;
    public DateTimeOffset? DueTo { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset? UpdatedDate { get; set; }
}