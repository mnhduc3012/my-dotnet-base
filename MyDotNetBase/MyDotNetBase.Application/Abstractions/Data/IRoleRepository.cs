using MyDotNetBase.Domain.Roles.Entities;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRoleRepository
{
    Task<Result<Role>> GetDefaultRoleAsync();
}
