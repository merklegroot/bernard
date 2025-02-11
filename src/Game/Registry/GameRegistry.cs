using Microsoft.Extensions.DependencyInjection;
using Game.Repo;

namespace Game.Registry;

public static class GameRegistry
{
    public static IServiceCollection RegisterGame(this IServiceCollection collection) =>
        collection
            .RegisterMassTransit()
            .RegisterControllers()
            .AddScoped<ICombatRepo, CombatRepo>()
            .AddScoped<IMobDefRepo, MobDefRepo>()
            .AddScoped<IResourceReader, ResourceReader>()
            .AddScoped<IRoomDefRepo, RoomDefRepo>()
            .AddScoped<IRoomStateRepo, RoomStateRepo>()
            .AddScoped<IEgoRepo, EgoRepo>()
            .AddScoped<IManipulativeDefRepo, ManipulativeDefRepo>();
}