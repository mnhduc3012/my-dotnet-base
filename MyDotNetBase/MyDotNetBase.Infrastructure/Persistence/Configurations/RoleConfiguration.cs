using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Roles.ValueObjects;

namespace MyDotNetBase.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .ToTable("roles");

        builder
            .HasKey(r => r.Id);

        builder
            .Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new RoleId(value))
            .ValueGeneratedNever();

        builder
            .Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .OwnsMany(r => r.Permissions, rp =>
            {
                rp.Property(p => p.Permission).HasConversion<int>();
                rp.HasKey("RoleId", nameof(RolePermission.Permission));
            });
    }
}
