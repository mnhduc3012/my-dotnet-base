using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Shared.ValueObjects;
using MyDotNetBase.Domain.Users.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public sealed class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        builder
            .ToTable("otps");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever();

        builder
             .Property(u => u.Email)
             .HasColumnName("email")
             .HasConversion(
                 e => e.Value,
                 value => new Email(value));

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder
            .HasIndex(x => new { x.Email, x.Code })
            .IsUnique();
    }
}
