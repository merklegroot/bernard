using Game.Constants;
using Game.Models;
using Godot;

public partial class GameApp : Node
{
    public override void _Ready()
    {
        GD.Print("GameApp initialized");

        EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
        EventBus.Instance.InventoryItemSelected += OnInventoryItemSelected;
    }

    public void OnDropInventoryItem(int inventoryItemIndex)
    {
        GD.Print($"Drop inventoryItemIndex: {inventoryItemIndex}");
        GameStateContainer.GameState.Inventory.RemoveAt(inventoryItemIndex);
        
        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
    }

    public void OnInventoryItemSelected(int inventoryItemIndex)
    {
        GD.Print($"Inventory item selected | InventoryItemIndex: {inventoryItemIndex}");
        GameStateContainer.GameState.MainPanel = PanelEnum.InventoryDetails;
        
        GD.Print($"Main panel set to {GameStateContainer.GameState.MainPanel}");
        EventBus.Instance.EmitSignal(EventBus.SignalName.MainPanelChanged);
    }
}
