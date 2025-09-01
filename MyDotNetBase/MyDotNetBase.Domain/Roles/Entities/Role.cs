using MyDotNetBase.Domain.Roles.Enums;
using MyDotNetBase.Domain.Roles.Errors;
using MyDotNetBase.Domain.Roles.ValueObjects;
using MyDotNetBase.Domain.Shared.Aggregates;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Roles.Entities;

public sealed class Role : AggregateRoot<RoleId>
{
    private readonly List<RolePermission> _permissions = [];
    public IReadOnlyList<RolePermission> Permissions => _permissions.AsReadOnly();
    public string Name { get; private set; }
    public string Description { get; private set; }
    private Role(RoleId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    public static Result<Role> Create(
        string name,
        string description,
        List<string> permissions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        var role = new Role(
            RoleId.New(),
            name,
            description);

        foreach (var permission in permissions)
        {
            var result = role.AddPermission(permission);
            if (result.IsFailure)
                return result.Error;
        }

        return role;
    }

    public static Result<Role> Update(
        Role role,
        string name,
        string description,
        List<string> permissions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        role.Name = name;
        role.Description = description;
        role.ClearPermissions();
        foreach (var permission in permissions)
        {
            var result = role.AddPermission(permission);
            if (result.IsFailure)
                return result.Error;
        }
        return role;
    }

    public Result AddPermission(string permission)
    {
        var rolePermissionOrError = RolePermission.Create(permission);
        if (rolePermissionOrError.IsFailure)
            return rolePermissionOrError.Error;

        if (!_permissions.Contains(rolePermissionOrError.Value))
            _permissions.Add(rolePermissionOrError.Value);

        return Result.Success();
    }

    public Result RemovePermission(string permission)
    {
        var rolePermissionOrError = RolePermission.Create(permission);
        if (rolePermissionOrError.IsFailure)
            return rolePermissionOrError.Error;

        _permissions.Remove(rolePermissionOrError.Value);

        return Result.Success();
    }

    private void ClearPermissions()
    {
        _permissions.Clear();
    }
}
