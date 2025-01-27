namespace Game.Models;

public static class GameStateContainer
{
    public static GameState GameState { get; set; } = InitialGameState.Get();
}