using ErrorOr;

using Gridly.Application.Dtos;

using MediatR;

namespace Gridly.Application.Commands;

public record CreateColumnCommand : IRequest<ErrorOr<ColumnDto>>
{
    public Guid BoardId { get; set; }
    public Guid UserId { get; set; }
    public required string Name { get; set; }
}