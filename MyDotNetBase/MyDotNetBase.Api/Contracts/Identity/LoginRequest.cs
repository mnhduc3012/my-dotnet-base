namespace MyDotNetBase.Api.Contracts.Identity;

public record LoginRequest(
    string Username,
    string Password);
