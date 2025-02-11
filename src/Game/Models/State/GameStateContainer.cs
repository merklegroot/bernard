using Game.Constants;

namespace Game.Models.State;

public static class GameStateContainer
{
    public static GameState GameState { get; } = InitialGameState.Get();
}