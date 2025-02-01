namespace Game.Models;

public record InventoryItem : ManipulativeInstance
{
    public bool IsEquipped { get; set; }
}