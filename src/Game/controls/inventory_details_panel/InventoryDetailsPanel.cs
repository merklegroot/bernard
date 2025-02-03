using System;
using Game.Models;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class InventoryDetailsPanel : Panel
{
	private IManipulativeDefRepo _manipulativeDefRepo;
	private IRoomStateRepo _roomStateRepo;
	private IInventoryStateRepo _inventoryStateRepo;
	
	private TextureRect _itemIcon;
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");

	private Label _titleLabel;
	private Label _label;
	private Button _closeButton;
	private	Button _dropButton;
	private Button _pickupButton;
	private Button _equipButton;
	private Button _unequipButton;

	private InventoryItemSelectionData _itemSelection;

	public override void _Ready()
	{
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		_roomStateRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomStateRepo>();
		_inventoryStateRepo = GlobalContainer.Host.Services.GetRequiredService<IInventoryStateRepo>();
		
		_itemIcon = GetNode<TextureRect>("ItemIcon");
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_dropButton = GetNode<Button>("HBoxContainer/DropButton");
		_pickupButton = GetNode<Button>("HBoxContainer/PickupButton");
		_equipButton = GetNode<Button>("HBoxContainer/EquipButton");
		_unequipButton = GetNode<Button>("HBoxContainer/UnequipButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		_pickupButton.Pressed += OnPickupButtonPressed;
		_equipButton.Pressed += OnEquipButtonPressed;
		_unequipButton.Pressed += OnUnequipButtonPressed;
		
		EventBus.Instance.InventoryItemSelectedFlexible += OnInventoryItemSelectedFlexible;
	}
	
	private void OnInventoryItemSelectedFlexible(string data)
	{
		GD.Print($"OnInventoryItemSelectedFlexible: {data}");
		
		_itemSelection = data;

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
		var inventoryItem = _inventoryStateRepo.GetByInstanceId(_itemSelection.ManipulativeInstanceId);
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeDefId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = $"{matchingManipulativeDef.Name}\n" + 
					 (inventoryItem.IsEquipped ? "(Equipped)" : "(Not Equipped)");

		_equipButton.Visible = matchingManipulativeDef.IsWeapon && !inventoryItem.IsEquipped;
		_unequipButton.Visible = matchingManipulativeDef.IsWeapon && inventoryItem.IsEquipped;
		
		_itemIcon.Texture = !string.IsNullOrWhiteSpace(matchingManipulativeDef.ImageRes)
			? GD.Load<Texture2D>(matchingManipulativeDef.ImageRes)
			: _defaultTexture;

		Visible = true;
	}

	private void ProcessRoomItemSelected()
	{
		GD.Print($"ProcessRoomItemSelected: {_itemSelection}");
		
		var roomState = _roomStateRepo.Get(GameStateContainer.GameState.RoomId);
		var manipulativeInstance = _roomStateRepo.GetManipulativeByInstanceId(GameStateContainer.GameState.RoomId, _itemSelection.ManipulativeInstanceId);
		var manipulativeDefId = manipulativeInstance.ManipulativeDefId;
		
		// var manipulativeId = roomState.ManipulativeInstances[_itemSelection.Index];
		
		var matchingManipulativeDef = _manipulativeDefRepo.Get(manipulativeDefId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = matchingManipulativeDef.Name;

		_equipButton.Visible = matchingManipulativeDef.IsWeapon;
		
		// Set the icon
		_itemIcon.Texture = !string.IsNullOrWhiteSpace(matchingManipulativeDef.ImageRes)
			? GD.Load<Texture2D>(matchingManipulativeDef.ImageRes)
			: _defaultTexture;

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

		EventBus.Instance.EmitSignal(EventBus.SignalName.DropInventoryItem, _itemSelection.ManipulativeInstanceId.ToString());
	}	

	private void OnPickupButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Room)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		EventBus.Instance.EmitSignal(EventBus.SignalName.PickupRoomItem, _itemSelection.ManipulativeInstanceId.ToString());
	}

	private void OnEquipButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.EquipItem, (string)_itemSelection);
	}

	private void OnUnequipButtonPressed()
	{
		GD.Print("InventoryDetailsPanel: OnUnequipButtonPressed");
		EventBus.Instance.EmitSignal(EventBus.SignalName.UnequipItem, (string)_itemSelection);
	}

	public override void _ExitTree()
	{
		if (EventBus.Instance == null)
			return;
		
		EventBus.Instance.InventoryItemSelectedFlexible -= OnInventoryItemSelectedFlexible;
	}
}
