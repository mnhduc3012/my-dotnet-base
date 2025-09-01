using MyDotNetBase.Domain.Shared.Results;

namespace MyDotNetBase.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
