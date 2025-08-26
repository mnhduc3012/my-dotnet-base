using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.ValueObjects;
using MyDotNetBase.Domain.Shared.Entities;
using MyDotNetBase.Domain.Shared.Enums;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository : Repository<Role, RoleId>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<Role?> GetByIdAsync(RoleId id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Role>> GetDefaultRoleAsync()
    {
        return Task.FromResult(Result.Failure<Role>(new Error("", "default role", ErrorType.Failure)));
    }
}
