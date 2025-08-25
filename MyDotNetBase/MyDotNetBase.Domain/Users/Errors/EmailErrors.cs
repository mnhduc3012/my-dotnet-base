using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.User.Errors;

public static class EmailErrors
{
    public readonly static Error Invalid = Error.Failure(
        "Email.Invalid",
        "Email không hợp lệ");
}
