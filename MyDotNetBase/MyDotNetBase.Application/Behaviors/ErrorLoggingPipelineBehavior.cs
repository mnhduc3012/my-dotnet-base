using Microsoft.Extensions.Logging;
using MyDotNetBase.Domain.Shared.Entities;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDotNetBase.Application.Behaviors
{
    internal sealed class ErrorLoggingPipelineBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ErrorLoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public ErrorLoggingPipelineBehavior(
            ILogger<ErrorLoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                string requestName = typeof(TRequest).Name;
                _logger.LogError(ex,
                    "Unhandled exception for request {RequestName} with message {Message}",
                    requestName,
                    ex.Message);
                throw;
            }
        }
    }
}
