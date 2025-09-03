
using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public sealed class OtpRepository(ApplicationDbContext context) :
    Repository<Otp, Guid>(context),
    IOtpRepository
{
    public Task<Otp?> GetByEmailAndCodeAsync(string email, string code, CancellationToken cancellationToken)
    {
        return DbContext.Otps
            .FirstOrDefaultAsync(o => o.Email.Value == email && o.Code == code, cancellationToken);
    }

    public Task<Otp?> GetByEmailAndCodeAsync(Email email, string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Otp?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task<Otp?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
