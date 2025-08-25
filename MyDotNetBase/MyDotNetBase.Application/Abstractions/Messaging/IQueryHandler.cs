using MediatR;
using MyDotNetBase.Domain.Shared.Entities;

namespace MyDotNetBase.Application.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
