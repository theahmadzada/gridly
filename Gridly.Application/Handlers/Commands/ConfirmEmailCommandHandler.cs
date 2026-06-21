using System.Text;

using ErrorOr;

using Loot.Application.Commands;
using Loot.Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Loot.Application.Handlers.Commands;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ErrorOr<Guid>>
{
    private readonly UserManager<AppUser>  _userManager;

    public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<Guid>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if(user is null) return Error.NotFound("User.NotFound", "User not found.");
        
        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded) return result.Errors
            .Select(e => Error.Unexpected(e.Code, e.Description))
            .ToList();
        
        return user.Id;
    }
}