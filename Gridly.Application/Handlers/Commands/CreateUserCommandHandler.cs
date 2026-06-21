using System.Text;

using ErrorOr;

using Gridly.Application.Commands;
using Gridly.Application.Dtos;

using Gridly.Domain;
using Gridly.Domain.Entities;
using Gridly.Shared.Events;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Gridly.Application.Handlers.Commands;

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