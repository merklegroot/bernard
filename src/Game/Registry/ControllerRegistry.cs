using Microsoft.Extensions.DependencyInjection;
using Game.Controllers;
using Godot;

namespace Game.Registry;

public static class ControllerRegistry
{
    public static IServiceCollection RegisterControllers(this IServiceCollection collection)
    {
        var controllerTypes = ControllerUtil.GetControllerTypes();

        GD.Print($"Assembly has {controllerTypes.Count} controllers");

        foreach (var controllerType in controllerTypes)
        {
            collection.AddScoped(controllerType);
        }

        return collection;
    }
}