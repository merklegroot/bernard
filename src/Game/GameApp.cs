using Game.Models;
using Godot;

public partial class GameApp : Node
{
    public override void _Ready()
    {
        GD.Print("GameApp initialized");

        EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
    }

    public void OnDropInventoryItem(int inventoryItemIndex)
    {
        GD.Print($"Drop inventoryItemIndex: {inventoryItemIndex}");
        
        GameStateContainer.GameState.Inventory.RemoveAt(inventoryItemIndex);
        
        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
    }
}