using MyDotNetBase.Domain.User.Enitties;

namespace MyDotNetBase.Application.Abstractions.Data;

public interface IUserRepository
{
    Task AddAsync(User user);
}
