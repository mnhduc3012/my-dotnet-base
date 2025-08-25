using MyDotNetBase.Domain.Roles.Errors;
using MyDotNetBase.Domain.Roles.ValueObjects;
using MyDotNetBase.Domain.Shared.Aggregates;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Domain.Roles.Enitties;

public class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    private Role(RoleId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    public static Result<Role> Create(
        string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        var role = new Role(
            RoleId.New(),
            name,
            description);
        return role;
    }
}
