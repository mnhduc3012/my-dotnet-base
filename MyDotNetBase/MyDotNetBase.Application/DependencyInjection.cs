using Microsoft.Extensions.DependencyInjection;
using MyDotNetBase.Application.Behaviors;
using System.Reflection;

namespace MyDotNetBase.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // 1. Đăng ký MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // 2. Đăng ký tất cả validators trong assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // 3. Đăng ký pipeline behavior
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
