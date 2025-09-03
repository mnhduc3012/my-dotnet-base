namespace MyDotNetBase.Api.Contracts.Identity;

public sealed record VerifyEmailRequest(
    string Email,
    string Code);
