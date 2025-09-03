using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IOtpRepository : IRepository<Otp, Guid>
{
    Task<Otp?> GetByEmailAndCodeAsync(Email email, string code, CancellationToken cancellationToken);
    Task<Otp?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
}
