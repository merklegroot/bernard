using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Registry;

public static class MassTransitRegistry
{
    public static IServiceCollection RegisterMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
                    
            // Add consumers here
            // x.AddConsumer<YourConsumer>();
                    
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        
        return services;
    }
}