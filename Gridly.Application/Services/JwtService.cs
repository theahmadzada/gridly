using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Loot.Application.Dtos;
using Loot.Application.ServiceContracts;
using Loot.Shared.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Loot.Application.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private IOptions<JwtSettings> _jwtSettings;
    private IOptions<RefreshTokenSettings> _refreshTokenSettings;
    
    public JwtService(IConfiguration config, IOptions<JwtSettings> jwtSettings, IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _config = config;
        _jwtSettings = jwtSettings;
        _refreshTokenSettings = refreshTokenSettings;
    }

    public TokenDto GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var options = _jwtSettings.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(options.ValidFor);

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new TokenDto() { Token = tokenString, Expires = expires };
    }

    public TokenDto GenerateRefreshToken()
    {
        var options = _refreshTokenSettings.Value;
        var randomNumber = new byte[32];
        
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        var token = Convert.ToBase64String(randomNumber);
        return new TokenDto()
        {
            Token = token,
            Expires = DateTimeOffset.UtcNow.AddMinutes(options.ValidFor)
        };
    }
}