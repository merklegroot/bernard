using Microsoft.Extensions.DependencyInjection;
using System;
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
    
    public static T Resolve<T>() where T : class
    {
        if (_serviceProvider == null)
        {
            ConfigureServices();
        }
        
        return _serviceProvider.GetRequiredService<T>();
    }
    
    public static void InjectDependencies(MainScene scene)
    {
        if (_serviceProvider == null)
        {
            ConfigureServices();
        }
        
        // Get required services
        var altTextRepo = _serviceProvider.GetRequiredService<IAltTextRepo>();
        var gameState = _serviceProvider.GetRequiredService<GameState>();
        
        // Use reflection to set private fields
        var type = typeof(MainScene);
        type.GetField("_altTextRepo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(scene, altTextRepo);
        
        type.GetField("_gameState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(scene, gameState);
    }
}