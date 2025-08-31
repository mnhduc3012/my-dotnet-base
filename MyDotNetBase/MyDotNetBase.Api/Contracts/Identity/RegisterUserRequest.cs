namespace MyDotNetBase.Api.Contracts.Identity;

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FullName);
