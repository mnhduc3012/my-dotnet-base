using MyDotNetBase.Domain.Shared.Results;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Domain.Users.Errors;
public class UserErrors
{
    public static Error DuplicateEmail(Email email) => Error.Failure(
        "User.DuplicateEmail",
        $"Email {email.Value} đã tồn tại trong hệ thống");

    public static readonly Error NoRolesAssigned = Error.Failure(
        "User.NoRolesAssigned",
        "Người dùng phải có ít nhất 1 vai trò");

    public static readonly Error NotFound = Error.Failure(
        "User.NotFound",
        "Người dùng không tồn tại");

    public static readonly Error EmailAlreadyVerified = Error.Failure(
        "User.EmailAlreadyVerified",
        "Email đã được xác thực");
}
