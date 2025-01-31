using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Controllers;

public static class ControllerUtil
{
    public static List<Type> GetControllerTypes()
    {
        var controllerTypes = typeof(ControllerUtil).Assembly
            .GetTypes().ToArray()
            .Where(t => typeof(IController).IsAssignableFrom(t) && 
                        !t.IsInterface && 
                        !t.IsAbstract)
            .ToList();
        
        return controllerTypes;
    }
}