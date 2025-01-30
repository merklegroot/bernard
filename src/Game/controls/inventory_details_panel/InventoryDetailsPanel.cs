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

	private InventoryItemSelectionData _itemSelection;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_dropButton = GetNode<Button>("HBoxContainer/DropButton");
		_pickupButton = GetNode<Button>("HBoxContainer/PickupButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		_pickupButton.Pressed += OnPickupButtonPressed;
		
		EventBus.Instance.InventoryItemSelctedFlexible += OnInventoryItemSelectedFlexible;
	}
	
	private void OnInventoryItemSelectedFlexible(string data)
	{
		GD.Print($"OnInventoryItemSelectedFlexible: {data}");
		
		_itemSelection = (InventoryItemSelectionData)data;

		_dropButton.Visible = _itemSelection.Source == InventoryItemSelectionSource.Inventory;
		_pickupButton.Visible = _itemSelection.Source == InventoryItemSelectionSource.Room;

		if (_itemSelection.Source == InventoryItemSelectionSource.Inventory)
		{
			ProcessInventoryItemSelected();
			return;
		}

		if (_itemSelection.Source == InventoryItemSelectionSource.Room)
		{
			ProcessRoomItemSelected();
			return;
		}
		
		throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");
	}

	private void ProcessInventoryItemSelected()
	{
		var inventoryItem = GameStateContainer.GameState.Inventory[_itemSelection.Index];
		
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = matchingManipulativeDef.Name;

		Visible = true;
	}

	private void ProcessRoomItemSelected()
	{
		GD.Print($"ProcessRoomItemSelected: {_itemSelection.Index}");
		
		var roomState = _roomStateRepo.Get(GameStateContainer.GameState.RoomId);
		
		var manipulativeId = roomState.ManipulativeIds[_itemSelection.Index];
		
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
		if (_itemSelection.Source != InventoryItemSelectionSource.Inventory)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		EventBus.Instance.EmitSignal(EventBus.SignalName.DropInventoryItem, _itemSelection.Index);
	}	

	private void OnPickupButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Room)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		EventBus.Instance.EmitSignal(EventBus.SignalName.PickupRoomItem, _itemSelection.Index);
	}

	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.InventoryItemSelctedFlexible -= OnInventoryItemSelectedFlexible;
		}
	}
}
