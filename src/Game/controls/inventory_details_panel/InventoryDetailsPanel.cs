using System;
using System.Text;
using Game.Models;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class InventoryDetailsPanel : Panel
{
	private IManipulativeDefRepo _manipulativeDefRepo;
	private IRoomStateRepo _roomStateRepo;
	private IEgoRepo _iiEgoRepo;
	
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
		_iiEgoRepo = GlobalContainer.Host.Services.GetRequiredService<IEgoRepo>();
		
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
		var inventoryItem = _iiEgoRepo.GetInventoryItemByInstanceId(_itemSelection.ManipulativeInstanceId);
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeDefId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		var labelDescriptionBuilder = new StringBuilder()
			.AppendLine(matchingManipulativeDef.Name)
			.AppendLine(inventoryItem.IsEquipped ? "(Equipped)" : "(Not Equipped)")
			.AppendLine($"IsWeapon: {matchingManipulativeDef.IsWeapon}")
			.AppendLine($"IsHelmet: {matchingManipulativeDef.IsHelmet}")
			.AppendLine($"IsArmor: {matchingManipulativeDef.IsArmor}");

		if (matchingManipulativeDef.Atk != 0)
		{
			labelDescriptionBuilder.AppendLine($"Atk: {matchingManipulativeDef.Atk}");
		}
		
		if (matchingManipulativeDef.Def != 0)
		{
			labelDescriptionBuilder.AppendLine($"Def: {matchingManipulativeDef.Def}");
		}

		_label.Text = labelDescriptionBuilder.ToString();

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

	private bool IsEquipment(ManipulativeDef item) =>
		item.IsWeapon || item.IsArmor || item.IsHelmet;

	private void ProcessRoomItemSelected()
	{
		GD.Print($"ProcessRoomItemSelected: {_itemSelection}");
		
		var manipulativeInstance = _roomStateRepo.GetManipulativeByInstanceId(GameStateContainer.GameState.RoomId, _itemSelection.ManipulativeInstanceId);
		var manipulativeDefId = manipulativeInstance.ManipulativeDefId;
		var matchingManipulativeDef = _manipulativeDefRepo.Get(manipulativeDefId);

		_titleLabel.Text = matchingManipulativeDef.Name;
		
		var labelDescriptionBuilder = new StringBuilder()
			.AppendLine(matchingManipulativeDef.Name);
		
		if (matchingManipulativeDef.Atk > 0)
		{
			labelDescriptionBuilder.AppendLine($"Atk: {matchingManipulativeDef.Atk}");
		}

		_label.Text = labelDescriptionBuilder.ToString();

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
}
