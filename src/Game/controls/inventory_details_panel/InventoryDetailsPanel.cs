using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Models;
using Game.Models.State;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class InventoryDetailsPanel : GamePanel
{
	private IManipulativeDefRepo _manipulativeDefRepo;
	private IRoomStateRepo _roomStateRepo;
	private IEgoRepo _egoRepo;
	
	private TextureRect _itemIcon;
	private readonly Texture2D _defaultTexture = GD.Load<Texture2D>("res://assets/Pixel Art Icon Pack - RPG/Misc/Gear.png");

	private Label _titleLabel;
	private Label _itemNameLabel;
	private Label _label;
	private Button _closeButton;
	private	Button _dropButton;
	private Button _pickupButton;
	private Button _equipButton;
	private Button _unequipButton;

	private InventoryItemSelectionData _itemSelection;

	public override void _Ready()
	{
		base._Ready();
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		_roomStateRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomStateRepo>();
		_egoRepo = GlobalContainer.Host.Services.GetRequiredService<IEgoRepo>();
		
		_itemIcon = GetNode<TextureRect>("VBoxContainer/BodyContainer/HBoxContainer/LeftColumn/ItemIcon");
		_itemNameLabel = GetNode<Label>("VBoxContainer/BodyContainer/HBoxContainer/LeftColumn/ItemName");
		_label = GetNode<Label>("VBoxContainer/BodyContainer/HBoxContainer/RightColumn/DescriptionLabel");
		_titleLabel = GetNode<Label>("VBoxContainer/TitleLabel");
		_closeButton = GetNode<Button>("ButtonsContainer/CloseButton");
		_dropButton = GetNode<Button>("ButtonsContainer/DropButton");
		_pickupButton = GetNode<Button>("ButtonsContainer/PickupButton");
		_equipButton = GetNode<Button>("ButtonsContainer/EquipButton");
		_unequipButton = GetNode<Button>("ButtonsContainer/UnequipButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		_pickupButton.Pressed += OnPickupButtonPressed;
		_equipButton.Pressed += OnEquipButtonPressed;
		_unequipButton.Pressed += OnUnequipButtonPressed;
		
		Game.Events.EventBus.Instance.InventoryItemSelectedFlexible += OnInventoryItemSelectedFlexible;
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
		var inventoryItem = _egoRepo.GetInventoryItemByInstanceId(_itemSelection.ManipulativeInstanceId);
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeDefId);

		_titleLabel.Text = "Item Details";
		var equippedText = inventoryItem.IsEquipped
			? "(Equipped)"
			: string.Empty;
		
		_itemNameLabel.Text = $"{matchingManipulativeDef.Name} {equippedText}".Trim();
		
		var labelDescriptionBuilder = new StringBuilder();
		labelDescriptionBuilder.AppendLine(GetEquipmentTypeText(matchingManipulativeDef));

		if (matchingManipulativeDef.Atk != 0)
		{
			labelDescriptionBuilder.AppendLine($"Attack: {matchingManipulativeDef.Atk}");
		}
		
		if (matchingManipulativeDef.Def != 0)
		{
			labelDescriptionBuilder.AppendLine($"Defense: {matchingManipulativeDef.Def}");
		}

		_label.Text = labelDescriptionBuilder.ToString().TrimEnd();

		_equipButton.Visible =
			_itemSelection.Source == InventoryItemSelectionSource.Inventory
			&& IsEquipment(matchingManipulativeDef)
			&& !inventoryItem.IsEquipped;
		
		_unequipButton.Visible = 
			_itemSelection.Source == InventoryItemSelectionSource.Inventory
			&& IsEquipment(matchingManipulativeDef)
			&& inventoryItem.IsEquipped;
		
		_itemIcon.Texture = !string.IsNullOrWhiteSpace(matchingManipulativeDef.ImageRes)
			? GD.Load<Texture2D>(matchingManipulativeDef.ImageRes)
			: _defaultTexture;

		Visible = true;
	}

	private string GetEquipmentTypeText(ManipulativeDef manipulativeDef)
	{
		var equipmentTypes = new List<(bool isType, string typeName)>
		{
			(manipulativeDef.IsWeapon, "Weapon"),
			(manipulativeDef.IsHelmet, "Helmet"),
			(manipulativeDef.IsArmor, "Armor"),
		}
		.Where(equipmentType => equipmentType.isType)
		.Select(equipmentType => equipmentType.typeName)
		.ToList();

		var typeTexts = equipmentTypes.Any()
			? $"Type: {string.Join(", ", equipmentTypes)}"
			: "Type: Other";

		return typeTexts;
	}

	private bool IsEquipment(ManipulativeDef item) =>
		item.IsWeapon || item.IsArmor || item.IsHelmet;

	private void ProcessRoomItemSelected()
	{
		GD.Print($"ProcessRoomItemSelected: {_itemSelection}");
		
		var manipulativeInstance = _roomStateRepo.GetManipulativeByInstanceId(
			GameStateContainer.GameState.PlayerState.RoomId, 
			_itemSelection.ManipulativeInstanceId
		);
		var manipulativeDefId = manipulativeInstance.ManipulativeDefId;
		var matchingManipulativeDef = _manipulativeDefRepo.Get(manipulativeDefId);

		_titleLabel.Text = "Item Details";
		_itemNameLabel.Text = matchingManipulativeDef.Name;
		
		var labelDescriptionBuilder = new StringBuilder();
		var typeText = GetEquipmentTypeText(matchingManipulativeDef);
		labelDescriptionBuilder.AppendLine($"Type: {typeText}");
		
		if (matchingManipulativeDef.Atk > 0)
			labelDescriptionBuilder.AppendLine($"Attack: {matchingManipulativeDef.Atk}");
		
		if (matchingManipulativeDef.Def > 0)
			labelDescriptionBuilder.AppendLine($"Defense: {matchingManipulativeDef.Def}");

		_label.Text = labelDescriptionBuilder.ToString().Trim();

		_equipButton.Visible = false;
		_unequipButton.Visible = false;
		
		_itemIcon.Texture = !string.IsNullOrWhiteSpace(matchingManipulativeDef.ImageRes)
			? GD.Load<Texture2D>(matchingManipulativeDef.ImageRes)
			: _defaultTexture;

		Visible = true;
	}

	private void OnCloseButtonPressed()
	{
		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.CloseInventoryDetails);
	}

	private void OnDropButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Inventory)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.DropInventoryItem, _itemSelection.ManipulativeInstanceId.ToString());
	}	

	private void OnPickupButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Room)
			throw new ApplicationException($"Unexpected source: {_itemSelection.Source}");

		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.PickupRoomItem, _itemSelection.ManipulativeInstanceId.ToString());
	}

	private void OnEquipButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Inventory)
			return;
		
		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.EquipItem, (string)_itemSelection);
	}

	private void OnUnequipButtonPressed()
	{
		if (_itemSelection.Source != InventoryItemSelectionSource.Inventory)
			return;
		
		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.UnequipItem, (string)_itemSelection);
	}

	public override void _ExitTree()
	{
		if (Game.Events.EventBus.Instance == null)
			return;
		
		Game.Events.EventBus.Instance.InventoryItemSelectedFlexible -= OnInventoryItemSelectedFlexible;
	}

	public void ShowDetails(string description)
	{
		_label.Text = description;
	}
}
