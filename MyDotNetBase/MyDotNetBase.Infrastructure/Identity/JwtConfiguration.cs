namespace MyDotNetBase.Infrastructure.Identity;

public class JwtConfiguration
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpiryMinutes { get; set; }
}
