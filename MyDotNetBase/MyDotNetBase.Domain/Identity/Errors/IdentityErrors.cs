using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Domain.Identity.Errors;

public static class IdentityErrors
{
    public static readonly Error InvalidCredentials = Error.Failure(
        "Identity.InvalidCredentials",
        "Tên tài khoản hoặc mật khẩu không đúng");

    public static readonly Error InvalidRefreshToken = Error.Failure(
        "Unauthorized",
        "Refresh token không hợp lệ");

    public static readonly Error EmailNotConfirmed = Error.Failure(
        "Identity.EmailNotConfirmed",
        "Email chưa được xác thực");

    public static readonly Error InvalidOtp = Error.Failure(
        "Identity.OtpNotFound",
        "Mã xác thực không hợp lệ hoặc đã hết hạn");

    public static readonly Error OtpAlreadySent = Error.Failure(
        "Identity.OtpAlreadySent",
        "Mã xác thực đã được gửi, vui lòng kiểm tra email của bạn");
}
