using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.User.Enitties;

namespace MyDotNetBase.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    public UserRepository(ApplicationDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
}
