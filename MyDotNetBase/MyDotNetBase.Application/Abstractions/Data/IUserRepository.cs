using MyDotNetBase.Domain.Users.Entities;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IUserRepository
{
    Task AddAsync(User user);
}
