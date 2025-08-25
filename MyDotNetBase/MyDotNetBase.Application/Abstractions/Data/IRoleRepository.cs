using MyDotNetBase.Domain.Roles.Enitties;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRoleRepository
{
    Task<Result<Role>> GetDefaultRoleAsync();
}
