using System.Security.Claims;

using Gridly.Application.Dtos;

namespace Gridly.Application.ServiceContracts;

public interface IJwtService
{
    TokenDto GenerateAccessToken(IEnumerable<Claim> claims);
    TokenDto GenerateRefreshToken();
}