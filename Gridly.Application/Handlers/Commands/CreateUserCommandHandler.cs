using System.Text;

using ErrorOr;

using Loot.Application.Commands;
using Loot.Application.Dtos;
using Loot.Domain;
using Loot.Domain.Entities;
using Loot.Shared.Events;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Loot.Application.Handlers.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPublishEndpoint _endpoint;
    
    public CreateUserCommandHandler(UserManager<AppUser> userManager, IPublishEndpoint endpoint)
    {
        _userManager = userManager;
        _endpoint = endpoint;
    }
    
    public async Task<ErrorOr<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
        if (!roleResult.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Unexpected(e.Code, e.Description))
                .ToList();
        }
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        await _endpoint.Publish(
            new UserCreatedEvent
            {
                Id = user.Id, 
                Email = user.Email,
                FirstName = user.FirstName, 
                LastName = user.LastName,
                Token = encodedToken,
            }, cancellationToken);
        return (UserDto)user;
    }
}