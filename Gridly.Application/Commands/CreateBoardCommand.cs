using ErrorOr;

using FluentValidation;

using Loot.Application.Dtos;

using MediatR;

namespace Loot.Application.Commands;

public record CreateBoardCommand : IRequest<ErrorOr<BoardDto>>
{
    public required string Name { get; set; }
    public Guid UserId { get; set; }
}

public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
    }
}