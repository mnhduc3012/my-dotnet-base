using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.User.ValueObjects;

namespace MyDotNetBase.Domain.User.Errors;
public class UserErrors
{
    public static Error DuplicateEmail(Email email) => Error.Failure(
        "User.DuplicateEmail",
        $"Email {email.Value} đã tồn tại trong hệ thống");

    public static readonly Error NoRolesAssigned = Error.Failure(
        "User.NoRolesAssigned",
        "Người dùng phải có ít nhất 1 vai trò");
}
