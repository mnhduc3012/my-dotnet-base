using MyDotNetBase.Application.Abstractions.Authentication;

namespace MyDotNetBase.Infrastructure.Identity;

public sealed class JwtTokenProvider : ITokenProvider
{
    public string GenerateAccessToken(string userId)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken(string userId)
    {
        throw new NotImplementedException();
    }

    public string? GetUserIdFromAccessToken(string token)
    {
        throw new NotImplementedException();
    }

    public string? GetUserIdFromRefreshToken(string token)
    {
        throw new NotImplementedException();
    }

    public bool ValidateAccessToken(string token)
    {
        throw new NotImplementedException();
    }

    public bool ValidateRefreshToken(string token)
    {
        throw new NotImplementedException();
    }
}
