using ErrorOr;
using FluentValidation;
using Loot.Application.Dtos;
using MediatR;

namespace Loot.Application.Commands;

public record CreateUserCommand : IRequest<ErrorOr<UserDto>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(option => option.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(30).WithMessage("First name cannot exceed 30 characters.");
        RuleFor(option => option.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(30).WithMessage("Last name cannot exceed 30 characters.");
        RuleFor(option => option.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(option => option.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters.");
        RuleFor(option => option.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(option => option.Password)
            .WithMessage("Password and confirm password do not match.");
    }
}