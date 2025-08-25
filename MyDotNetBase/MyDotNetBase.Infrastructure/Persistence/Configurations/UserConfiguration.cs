using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Domain.Roles.Enitties;
using MyDotNetBase.Domain.User.Enitties;
using MyDotNetBase.Domain.User.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .ValueGeneratedNever();

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                 .IsRequired();
        });

        builder.HasMany<Role>("_roleIds")
            .WithMany();
    }
}
