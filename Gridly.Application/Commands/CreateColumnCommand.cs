using ErrorOr;

using Loot.Application.Dtos;

using MediatR;

namespace Loot.Application.Commands;

public record CreateColumnCommand : IRequest<ErrorOr<ColumnDto>>
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
}