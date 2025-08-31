using MediatR;
using Microsoft.Extensions.Logging;
using MyDotNetBase.Domain.Shared.DomainEvents;
using MyDotNetBase.Infrastructure.Persistence;
using Newtonsoft.Json;
using Quartz;

namespace MyDotNetBase.Infrastructure.Outbox;

public sealed class ProcessOutboxMessagesJob : IJob
{
    private const int MaxRetryCount = 3;
    private const int BatchSize = 20;
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ApplicationDbContext dbContext,
        IPublisher publisher,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOn == null && m.RetryCount < MaxRetryCount)
            .OrderBy(m => m.OccurredOn)
            .Take(BatchSize)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            try
            {
                IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(outboxMessage.Content);

                if (domainEvent == null)
                {
                    _logger.LogWarning("Could not deserialize outbox message with ID {OutboxMessageId}", outboxMessage.Id);
                    continue;
                }

                await _publisher.Publish(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedOn = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing outbox message with ID {OutboxMessageId}", outboxMessage.Id);
                outboxMessage.RetryCount++;
                outboxMessage.Error = $"{ex.Message}. {ex.StackTrace}";
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}