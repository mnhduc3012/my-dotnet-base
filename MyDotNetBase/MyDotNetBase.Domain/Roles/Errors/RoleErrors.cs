using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Roles.Errors;
public class RoleErrors
{
    public static readonly Error EmptyRoleName = Error.Failure(
        "Role.EmptyRoleName",
        "Tên vai trò không thể để trống");
}
