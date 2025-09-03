using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Shared.ValueObjects;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(ApplicationDbContext context) :
    Repository<User, UserId>(context),
    IUserRepository
{
    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public override Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
    }
}
