using System;
using System.Linq;
using Game.Constants;
using Game.Models;
using Game.Repo;
using Godot;

namespace Game.Controllers;

// ReSharper disable once UnusedType.Global
public class InventoryController : IController
{
	private readonly IRoomStateRepo _roomStateRepo;
	private readonly IEgoRepo _iiEgoRepo;

	public InventoryController(IRoomStateRepo roomStateRepo, IEgoRepo iiEgoRepo)
	{
		_roomStateRepo = roomStateRepo;
		_iiEgoRepo = iiEgoRepo;
	}
	
	public void Register()
	{
		Events.EventBus.Instance.CloseInventoryDetails += OnCloseInventoryDetails;
		Events.EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
		Events.EventBus.Instance.InventoryItemSelectedFlexible += OnInventoryItemSelectedFlexible;
		Events.EventBus.Instance.EquipItem += OnEquipItem;
		Events.EventBus.Instance.UnequipItem += OnUnequipItem;
	}
	
	private void OnCloseInventoryDetails()
	{
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.SetMainPanel, (int)PanelEnum.Room);
	}

	private void OnDropInventoryItem(string manipulativeInstanceIdText)
	{
		var manipulativeInstanceId = Guid.Parse(manipulativeInstanceIdText);
		
		GD.Print($"Drop manipulativeInstanceId: {manipulativeInstanceId}");

		var inventoryItem = _iiEgoRepo.GetInventoryItemByInstanceId(manipulativeInstanceId);
		_iiEgoRepo.RemoveInventoryItemByInstanceId(manipulativeInstanceId);

		GD.Print($"Adding manipulative {inventoryItem.ManipulativeDefId} to room {GameStateContainer.GameState.RoomId}");
		_roomStateRepo.AddManipulative(GameStateContainer.GameState.RoomId, inventoryItem.ManipulativeDefId);

		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.InventoryChanged);
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.RoomChanged);
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.CloseInventoryDetails);
	}
	
	private void OnInventoryItemSelectedFlexible(string data)
	{
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.SetMainPanel, (int)PanelEnum.InventoryDetails);
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
		
		var instanceId = selectionData.ManipulativeInstanceId;
		var inventoryItem = GameStateContainer.GameState.Inventory
			.First(item => item.Id == instanceId);

		inventoryItem.IsEquipped = shouldEquip;
		
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.InventoryChanged);
		Events.EventBus.Instance.EmitSignal(Events.EventBus.SignalName.CloseInventoryDetails);
	}
}
