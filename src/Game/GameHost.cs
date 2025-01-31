using Microsoft.Extensions.DependencyInjection;
using System;
using Game.Constants;
using Game.Models;

namespace Game;

public class GameHost
{
    private static IServiceProvider _serviceProvider;
    
    public static void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddScoped<IAltTextRepo, AltTextRepo>();
        var gameState = InitialGameState.Get();
        services.AddSingleton(gameState);
        
        GameStateContainer.GameState = gameState;
        
        _serviceProvider = services.BuildServiceProvider();
    }
}