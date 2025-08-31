using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Infrastructure.Outbox;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder
            .ToTable("outbox_messages");
    }
}
