using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Application.Abstractions.Messaging;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Application.Users.Queries;

public sealed record GetUserByIdQuery(
    Guid UserId
) : IQuery<User>;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;
    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(new UserId(request.UserId));
    }
}