using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Identity.Errors;

public static class IdentityErrors
{
    public static readonly Error InvalidCredentials = Error.Failure(
        "Identity.InvalidCredentials",
        "Tên tài khoản hoặc mật khẩu không đúng");

    public static readonly Error InvalidRefreshToken = Error.Failure(
        "Unauthorized",
        "Refresh token không hợp lệ");
}
