using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task DeleteByUserIdAsync(UserId userId)
    {
        return DbContext.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ExecuteDeleteAsync();
    }

    public override Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return DbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }
}
