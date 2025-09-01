using MyDotNetBase.Domain.Roles.Enums;
using MyDotNetBase.Domain.Roles.Errors;
using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Domain.Roles.ValueObjects;

public sealed record RolePermission
{
    public Permission Permission { get; }

    private RolePermission(Permission permission)
    {
        Permission = permission;
    }

    public static Result<RolePermission> Create(string value)
    {
        if (Enum.TryParse<Permission>(value, ignoreCase: true, out var parsed) &&
            Enum.IsDefined(typeof(Permission), parsed))
        {
            return new RolePermission(parsed);
        }

        return PermissionErrors.InvalidPermission(value);
    }

    public static RolePermission FromEnum(Permission permission) => new(permission);
    public override string ToString() => Permission.ToString();
}

