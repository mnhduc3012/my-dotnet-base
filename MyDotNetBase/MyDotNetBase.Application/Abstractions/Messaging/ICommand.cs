using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
