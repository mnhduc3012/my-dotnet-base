namespace MyDotNetBase.Application.Abstractions.Authentication;

public interface IUserContext
{
    string? UserId { get; }
    string? Username { get; }
}
