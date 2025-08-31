using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : 
    Repository<RefreshToken, Guid>(context),
    IRefreshTokenRepository
{
    public Task DeleteByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public override Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return DbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}
