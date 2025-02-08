using System.Reflection;
using Game.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Registry;

public static class MediatrRegistry
{
    public static IServiceCollection RegisterMediatr(this IServiceCollection services)
    {
        services.AddSingleton<EgoState>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}