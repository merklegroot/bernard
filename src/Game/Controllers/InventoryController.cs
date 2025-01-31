using Game.Constants;
using Game.Models;
using Game.Repo;
using Godot;

namespace Game.Controllers;

// ReSharper disable once UnusedType.Global
public class InventoryController : IController
{
    private readonly IRoomStateRepo _roomStateRepo;
    private readonly IInventoryStateRepo _inventoryStateRepo;

    public InventoryController(IRoomStateRepo roomStateRepo, IInventoryStateRepo inventoryStateRepo)
    {
        _roomStateRepo = roomStateRepo;
        _inventoryStateRepo = inventoryStateRepo;
    }
    
    public void Register()
    {
        EventBus.Instance.CloseInventoryDetails += OnCloseInventoryDetails;
        EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
        EventBus.Instance.InventoryItemSelectedFlexible += OnInventoryItemSelectedFlexible;
    }
    
    private void OnCloseInventoryDetails()
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Room);
    }
    
    private void OnDropInventoryItem(int inventoryItemIndex)
    {
        GD.Print($"Drop inventoryItemIndex: {inventoryItemIndex}");

        var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];
        _inventoryStateRepo.RemoveIndex(inventoryItemIndex);

        GD.Print($"Adding manipulative {inventoryItem.ManipulativeId} to room {GameStateContainer.GameState.RoomId}");
        _roomStateRepo.AddManipulative(GameStateContainer.GameState.RoomId, inventoryItem.ManipulativeId);

        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
    }
    
    private void OnInventoryItemSelectedFlexible(string data)
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.InventoryDetails);
    }
}