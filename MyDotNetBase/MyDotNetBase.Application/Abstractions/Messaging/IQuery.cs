using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
