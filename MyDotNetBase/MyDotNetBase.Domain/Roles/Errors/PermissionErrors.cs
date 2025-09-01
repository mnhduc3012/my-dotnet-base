using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Roles.Errors;

public static class PermissionErrors
{
    public static Error InvalidPermission(string value) => Error.Failure(
        "Permission.InvalidPermission",
        $"Quyền '{value}' không hợp lệ");
}
