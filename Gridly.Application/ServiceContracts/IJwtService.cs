using System.Security.Claims;

using Loot.Application.Dtos;

namespace Loot.Application.ServiceContracts;

public interface IJwtService
{
    TokenDto GenerateAccessToken(IEnumerable<Claim> claims);
    TokenDto GenerateRefreshToken();
}