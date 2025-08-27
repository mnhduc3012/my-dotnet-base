using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override Task<User?> GetByIdAsync(UserId userId)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == userId);
    }
}
