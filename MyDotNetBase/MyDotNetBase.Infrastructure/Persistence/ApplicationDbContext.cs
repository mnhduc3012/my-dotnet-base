using MyDotNetBase.Application.Abstractions.Data;
using MyDotNetBase.Domain.Identity.Entities;
using MyDotNetBase.Domain.Roles.Entities;
using MyDotNetBase.Domain.Users.Entities;
using MyDotNetBase.Infrastructure.Outbox;
using System.Reflection;

namespace MyDotNetBase.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : 
    DbContext(options),
    IUnitOfWork
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
