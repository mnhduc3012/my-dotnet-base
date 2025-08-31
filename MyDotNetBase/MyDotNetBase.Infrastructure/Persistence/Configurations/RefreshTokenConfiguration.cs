using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .ToTable("refresh_tokens");

        builder
            .HasKey(rt => rt.Id);

        builder
            .Property(u => u.UserId)
            .HasColumnName("user_id")
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.
            Property(rt => rt.Token)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(rt => rt.ExpiresOnUtc)
            .IsRequired();

        builder
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId);
    }
}
