using MyDotNetBase.Domain.Users.Entities;

namespace MyDotNetBase.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}



