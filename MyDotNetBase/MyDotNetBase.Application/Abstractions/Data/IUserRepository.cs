using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByUsernameAsync(string username);
}
