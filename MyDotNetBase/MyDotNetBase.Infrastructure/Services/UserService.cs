using MyDotNetBase.Domain.Shared.ValueObjects;
using MyDotNetBase.Domain.Users.Services;
using MyDotNetBase.Domain.Users.ValueObjects;
using MyDotNetBase.Infrastructure.Persistence;

namespace MyDotNetBase.Infrastructure.Services;

public sealed class UserService :
    IEmailUniquenessChecker
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context) => _context = context;
    public async Task<bool> IsUniqueEmail(Email email)
    {
        return !await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}
