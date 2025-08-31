using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    Task DeleteByUserIdAsync(UserId userId);

    Task<RefreshToken?> GetByTokenAsync(string token);
}
