using MediatR;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
