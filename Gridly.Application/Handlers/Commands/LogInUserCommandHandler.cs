using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using ErrorOr;

using Loot.Application.Commands;
using Loot.Application.Dtos;
using Loot.Application.ServiceContracts;
using Loot.Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Loot.Application.Handlers.Commands;

public class LogInUserCommandHandler : IRequestHandler<LogInUserCommand, ErrorOr<LogInDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;
    
    public LogInUserCommandHandler(UserManager<AppUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ErrorOr<LogInDto>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Error.NotFound("User.NotFound", "User not found");

        var email = await _userManager.IsEmailConfirmedAsync(user);
        if (!email) return Error.Unauthorized("User.EmailNotConfirmed", "Email not confirmed");
        
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result) return Error.Unauthorized("User.WrongPassword", "Wrong password");

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken.Token;
        user.RefreshTokenExpires = refreshToken.Expires;
        
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded) 
            return updateResult.Errors.Select(x => Error.Unexpected(x.Code, x.Description)).ToList();
        
        return new LogInDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            AccessToken = accessToken.Token,
            AccessTokenExpiresAt = accessToken.Expires,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.Expires
        };
    }
}