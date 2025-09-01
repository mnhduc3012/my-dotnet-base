using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Domain.Roles.Errors;
public class RoleErrors
{
    public static readonly Error EmptyRoleName = Error.Failure(
        "Role.EmptyRoleName",
        "Tên vai trò không thể để trống");

    public static readonly Error NotFound = Error.Failure(
        "Role.NotFound",
        "Vai trò không tồn tại");
}
