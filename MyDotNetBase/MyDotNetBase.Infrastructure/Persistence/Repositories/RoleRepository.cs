using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository : Repository<Role, RoleId>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<Role?> GetByIdAsync(RoleId id)
    {
        return DbContext.Roles
            .Include(r => r.Permissions)
            .SingleOrDefaultAsync(r => r.Id == id);
    }

    public Task<Role?> GetDefaultRoleAsync()
    {
        return DbContext.Roles.FirstOrDefaultAsync();
    }
}
