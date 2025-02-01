using Microsoft.Extensions.DependencyInjection;
using Game.Repo;

namespace Game.Registry;

public static class GameRegistry
{
    public static IServiceCollection RegisterGame(this IServiceCollection collection) =>
        collection
            .RegisterMassTransit()
            .RegisterControllers()
            .AddScoped<IRoomDefRepo, RoomDefRepo>()
            .AddScoped<IRoomStateRepo, RoomStateRepo>()
            .AddScoped<IInventoryStateRepo, InventoryStateRepo>()
            .AddScoped<IManipulativeDefRepo, ManipulativeDefRepo>();
}