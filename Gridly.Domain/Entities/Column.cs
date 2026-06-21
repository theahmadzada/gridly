namespace Loot.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;
    public ICollection<TaskItem> Tasks { get; set; } = [];
}