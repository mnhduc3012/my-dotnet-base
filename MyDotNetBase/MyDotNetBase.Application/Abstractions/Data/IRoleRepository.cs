using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.ValueObjects;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRoleRepository : IRepository<Role, RoleId>
{
    Task<Role?> GetDefaultRoleAsync();
}
