using ErrorOr;

using FluentValidation;

using MediatR;

namespace Loot.Application.Commands;

public record ConfirmEmailCommand : IRequest<ErrorOr<Guid>>
{
    public Guid Id { get; init; }
    public required string Token { get; init; }
}

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(options => options.Id)
            .NotEmpty().WithMessage("Id is required.");
        RuleFor(options => options.Token)
            .NotEmpty().WithMessage("Token is required.");;
    }
}