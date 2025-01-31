namespace Game.Models;

public record InventoryItem : ManipulativeInstance
{
    public bool IsEquiped { get; init; }
}