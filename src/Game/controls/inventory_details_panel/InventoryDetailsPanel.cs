using System;
using Game.Models;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class InventoryDetailsPanel : Panel
{
	private IManipulativeDefRepo _manipulativeDefRepo;
	private IRoomStateRepo _roomStateRepo;
	private TextureRect _itemIcon;
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");

	private Label _titleLabel;
	private Label _label;
	private Button _closeButton;
	private	Button _dropButton;
	private Button _pickupButton;
	private Button _equipButton;

	private InventoryItemSelectionData _itemSelection;

	public override void _Ready()
	{
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		_roomStateRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomStateRepo>();
		
		_itemIcon = GetNode<TextureRect>("ItemIcon");
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_dropButton = GetNode<Button>("HBoxContainer/DropButton");
		_pickupButton = GetNode<Button>("HBoxContainer/PickupButton");
		_equipButton = GetNode<Button>("HBoxContainer/EquipButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		_pickupButton.Pressed += OnPickupButtonPressed;
		_equipButton.Pressed += OnEquipButtonPressed;
		
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
		var inventoryItem = GameStateContainer.GameState.Inventory[_itemSelection.Index];
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = $"{matchingManipulativeDef.Name}\n" + 
					 (inventoryItem.IsEquipped ? "(Equipped)" : "(Not Equipped)");

		_equipButton.Visible = matchingManipulativeDef.IsWeapon;
		_equipButton.Text = inventoryItem.IsEquipped ? "Unequip" : "Equip";
		
		// Set the icon
		_itemIcon.Texture = !string.IsNullOrWhiteSpace(matchingManipulativeDef.ImageRes)
			? GD.Load<Texture2D>(matchingManipulativeDef.ImageRes)
			: _defaultTexture;

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

		EventBus.Instance.EmitSignal(EventBus.SignalName.DropInventoryItem, _itemSelection.Index);
	}	

	private void OnPickupButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Room)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		EventBus.Instance.EmitSignal(EventBus.SignalName.PickupRoomItem, _itemSelection.Index);
	}

	private void OnEquipButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.EquipItem, (string)_itemSelection);
	}

	public override void _ExitTree()
	{
		if (EventBus.Instance == null)
			return;
		
		EventBus.Instance.InventoryItemSelectedFlexible -= OnInventoryItemSelectedFlexible;
	}
}
