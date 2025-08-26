using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Users.Services;
using MyDotNetBase.Infrastructure.Persistence;

namespace MyDotNetBase.Infrastructure.Services;

public class UserService : 
    IEmailUniquenessChecker
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context) => _context = context;
    public Task<bool> IsUniqueEmail(string email)
    {
        throw new NotImplementedException();
    }
}
