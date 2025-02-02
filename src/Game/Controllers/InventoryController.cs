using System;
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
        EventBus.Instance.EquipItem += OnEquipItem;
        EventBus.Instance.UnequipItem += OnUnequipItem;
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

        GD.Print($"Adding manipulative {inventoryItem.ManipulativeDefId} to room {GameStateContainer.GameState.RoomId}");
        _roomStateRepo.AddManipulative(GameStateContainer.GameState.RoomId, inventoryItem.ManipulativeDefId);

        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
    }
    
    private void OnInventoryItemSelectedFlexible(string data)
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.InventoryDetails);
    }

    private void OnEquipItem(string data) =>
        EquipOrUnequip(data, true);
    
    private void OnUnequipItem(string data) =>
        EquipOrUnequip(data, false);

    private void EquipOrUnequip(string data, bool shouldEquip)
    {
        GD.Print($"EquipOrUnequip: {data}");
        
        var selectionData = (InventoryItemSelectionData)data;
        
        if (selectionData.Source != InventoryItemSelectionSource.Inventory)
            throw new ApplicationException($"Unexpected source: {selectionData.Source}");

        var inventoryItemIndex = selectionData.Index;
        var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];

        inventoryItem.IsEquipped = shouldEquip;
        
        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
    }
}