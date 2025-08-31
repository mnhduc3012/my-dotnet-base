using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository(ApplicationDbContext context) :
    Repository<Role, RoleId>(context),
    IRoleRepository
{
    public override Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        return DbContext.Roles
            .Include(r => r.Permissions)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Role?> GetDefaultRoleAsync(CancellationToken cancellationToken)
    {
        return DbContext.Roles.FirstOrDefaultAsync(cancellationToken);
    }
}
