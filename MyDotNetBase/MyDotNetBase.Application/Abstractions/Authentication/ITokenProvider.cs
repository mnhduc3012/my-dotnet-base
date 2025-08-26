namespace MyDotNetBase.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string GenerateAccessToken(string userId);
    string GenerateRefreshToken(string userId);
    bool ValidateAccessToken(string token);
    bool ValidateRefreshToken(string token);
    string? GetUserIdFromAccessToken(string token);
    string? GetUserIdFromRefreshToken(string token);
}



