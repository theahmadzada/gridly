using ErrorOr;

using FluentValidation;

using Gridly.Application.Dtos;

using MediatR;

namespace Gridly.Application.Commands;

public record LogInUserCommand : IRequest<ErrorOr<LogInDto>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LogInUserCommandValidator : AbstractValidator<LogInUserCommand>
{
    public LogInUserCommandValidator()
    {
        RuleFor(option => option.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(option => option.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters.");
    }
}