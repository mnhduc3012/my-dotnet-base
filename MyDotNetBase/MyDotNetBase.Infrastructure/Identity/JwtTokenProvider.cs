using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyDotNetBase.Application.Abstractions.Authentication;
using MyDotNetBase.Domain.Shared.Constants;
using MyDotNetBase.Domain.Users.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyDotNetBase.Infrastructure.Identity;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtConfiguration _jwtConfiguration;
    public JwtTokenProvider(IOptions<JwtConfiguration> jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration.Value;
    }

    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email,user.Email)
        };

        if (user.Roles is not null && user.Roles.Any())
        {
            claims.AddRange(
                user.Roles
                    .Where(r => r.Permissions is not null && r.Permissions.Any())
                    .SelectMany(role =>
                        role.Permissions.Select(permission =>
                            new Claim(CustomClaimTypes.Permission, permission.Permission.ToString())))
            );
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpiryMinutes),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
