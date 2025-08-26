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
        List<Permission> permissions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        var role = new Role(
            RoleId.New(),
            name,
            description);

        foreach (var permission in permissions)
            role.AddPermission(permission);

        return role;
    }

    public void AddPermission(Permission permission)
    {
        var rolePermission = new RolePermission(permission);
        if (!_permissions.Contains(rolePermission))
            _permissions.Add(rolePermission);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(new RolePermission(permission));
    }
}
