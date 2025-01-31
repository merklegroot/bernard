using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Game.Controllers;
using Godot;

namespace Game.Registry;

public static class GameRegistry
{
    public static IServiceCollection RegisterGame(this IServiceCollection collection)
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