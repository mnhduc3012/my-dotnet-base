using MyDotNetBase.Domain.Roles.Enitties;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRoleRepository
{
    Task<Result<Role>> GetDefaultRoleAsync();
}
