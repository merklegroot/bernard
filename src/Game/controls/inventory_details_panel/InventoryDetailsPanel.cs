using System;
using Game.Models;
using Game.Repo;
using Godot;

public partial class InventoryDetailsPanel : Panel
{
	private readonly ManipulativeDefRepo _manipulativeDefRepo = new();
	private readonly RoomStateRepo _roomStateRepo = new();

	private Label _titleLabel;
	private Label _label;
	private Button _closeButton;
	private	Button _dropButton;
	private Button _pickupButton;

	private int _itemIndex;
	private InventoryItemSelectionSource _source;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_dropButton = GetNode<Button>("HBoxContainer/DropButton");
		_pickupButton = GetNode<Button>("HBoxContainer/PickupButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		
		EventBus.Instance.InventoryItemSelctedFlexible += OnInventoryItemSelectedFlexible;
	}
	
	private void OnInventoryItemSelectedFlexible(string data)
	{
		var inventoryItemSelectionData = (InventoryItemSelectionData)data;

		_dropButton.Visible = inventoryItemSelectionData.Source == InventoryItemSelectionSource.Inventory;
		_pickupButton.Visible = inventoryItemSelectionData.Source == InventoryItemSelectionSource.Room;

		if (inventoryItemSelectionData.Source == InventoryItemSelectionSource.Inventory)
		{
			ProcessInventoryItemSelected(inventoryItemSelectionData.Index);
			return;
		}

		if (inventoryItemSelectionData.Source == InventoryItemSelectionSource.Room)
		{
			ProcessRoomItemSelected(inventoryItemSelectionData.Index);
			return;
		}
		
		throw new ApplicationException($"Unexpected source: {inventoryItemSelectionData.Source}");
	}

	private void ProcessInventoryItemSelected(int inventoryItemIndex)
	{
		_itemIndex = inventoryItemIndex;
		_source = InventoryItemSelectionSource.Inventory;
		var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];
		
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.Id);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = matchingManipulativeDef.Name;

		Visible = true;
	}

	private void ProcessRoomItemSelected(int itemIndex)
	{
		_itemIndex = itemIndex;
		_source = InventoryItemSelectionSource.Inventory;
		var roomState = _roomStateRepo.Get(GameStateContainer.GameState.RoomId);
		
		var manipulativeId = roomState.ManipulativeIds[itemIndex];
		
		var matchingManipulativeDef = _manipulativeDefRepo.Get(manipulativeId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = matchingManipulativeDef.Name;

		Visible = true;
	}

	private void OnCloseButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
	}

	private void OnDropButtonPressed()
	{
		if (_source != InventoryItemSelectionSource.Inventory)
			throw new ApplicationException($"Unexpected source: {_source}");

		EventBus.Instance.EmitSignal(EventBus.SignalName.DropInventoryItem, _itemIndex);
	}
	
	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.InventoryItemSelctedFlexible -= OnInventoryItemSelectedFlexible;
		}
	}
}
