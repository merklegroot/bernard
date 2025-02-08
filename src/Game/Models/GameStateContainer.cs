using Game.Constants;

namespace Game.Models;

public static class GameStateContainer
{
    public static GameState GameState { get; } = InitialGameState.Get();
}