using Microsoft.Extensions.DependencyInjection;
using Game.Controllers;

namespace Game.Registry;

public static class ControllerRegistry
{
    public static IServiceCollection RegisterControllers(this IServiceCollection collection)
    {
        var controllerTypes = ControllerUtil.GetControllerTypes();
        foreach (var controllerType in controllerTypes)
        {
            collection.AddScoped(controllerType);
        }

        return collection;
    }
}