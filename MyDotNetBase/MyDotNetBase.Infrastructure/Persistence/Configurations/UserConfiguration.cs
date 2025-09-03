using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Domain.Users.ValueObjects;
using MyDotNetBase.Domain.Shared.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("users");

        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .ValueGeneratedNever();

        builder
            .Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value));
            
        builder.HasIndex(u => u.Email).IsUnique();

        builder
            .HasMany(u => u.Roles)
            .WithMany();

        builder
            .HasQueryFilter(u => !u.IsDeleted);
    }
}
