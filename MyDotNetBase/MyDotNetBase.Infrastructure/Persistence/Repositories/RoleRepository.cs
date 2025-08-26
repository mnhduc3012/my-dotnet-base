using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.Shared.Enums;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    public Task<Result<Role>> GetDefaultRoleAsync()
    {
        return Task.FromResult(Result.Failure<Role>(new Error("", "default role", ErrorType.Failure)));
    }
}
