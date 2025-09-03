namespace MyDotNetBase.Application.Abstractions.Identity;

public interface IOtpGenerator
{
    string GenerateOtp(int length = 6);
}
