using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _vboxContainer;
	private HBoxContainer _helmetContainer;
	private Label _weaponSlot;
	private Label _armorSlot;

	private IManipulativeDefRepo _manipulativeDefRepo;
	
	public override void _Ready()
	{
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		
		_weaponSlot = GetNode<Label>("VBoxContainer/WeaponSlot");
		_armorSlot = GetNode<Label>("VBoxContainer/ArmorSlot");
		_helmetContainer = GetNode<HBoxContainer>("VBoxContainer/HelmetSlot");
		_vboxContainer = GetNode<VBoxContainer>("VBoxContainer");
		
		Game.Events.EventBus.Instance.InventoryChanged += OnInventoryChanged;
		
		UpdateDisplay();
	}

	private void OnInventoryChanged()
	{
		UpdateDisplay();
	}

	private record ArmorInfo
	{
		public ArmorInfo()
		{
		}

		public ArmorInfo(Func<ManipulativeDef, bool> isOfType, string labelText)
		{
			IsOfType = isOfType;
			LabelText = labelText;
		}

		public Func<ManipulativeDef, bool> IsOfType { get; init; }
		public string LabelText { get; init; }
	}

	private class ButtonHandlerCombo
	{
		public Button Button { get; init; }
		public Action Handler { get; init; }
	}
	
	private readonly List<ButtonHandlerCombo> _buttonHandlerCombos = new();

	private void CleanDisplay()
	{
		foreach (var buttonHandlerCombo in _buttonHandlerCombos)
		{
			buttonHandlerCombo.Button.Pressed -= buttonHandlerCombo.Handler;
		}
		
		_buttonHandlerCombos.Clear();
		
		foreach (var child in _vboxContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	private class InventoryItemAndManipulativeDef
	{
		public InventoryItem InventoryItem { get; init; }
		public ManipulativeDef ManipulativeDef { get; init; }
	}

	private static readonly List<ArmorInfo> ArmorInfos = new()
	{
		new ArmorInfo(isOfType: def => def.IsWeapon, labelText: "Weapon:"),
		new ArmorInfo(isOfType: def => def.IsHelmet, labelText: "Helmet:"),
		new ArmorInfo(isOfType: def => def.IsArmor, labelText: "Armor:")
	};
	
	private void UpdateDisplay()
	{
		CleanDisplay();
		
		var equippedItems = GameStateContainer.GameState.Inventory.Where(item => item.IsEquipped).ToList();

		var combo = equippedItems.Select(inventoryItem =>
			{
				var manipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeDefId);
				return new InventoryItemAndManipulativeDef
				{
					InventoryItem = inventoryItem,
					ManipulativeDef = manipulativeDef
				};
			})
			.ToList();

		foreach (var info in ArmorInfos)
		{
			AddArmorInfo(info, combo);
		}
	}

	private void AddArmorInfo(ArmorInfo info, List<InventoryItemAndManipulativeDef> combo)
	{
		var container = new HBoxContainer();
		container.LayoutMode = 2;

		_vboxContainer.AddChild(container);

		var label = new Label { Text = info.LabelText };
		container.AddChild(label);
		
		var equippedItemOfType =
			combo.FirstOrDefault(item =>
				item.ManipulativeDef != null &&
				info.IsOfType(item.ManipulativeDef));

		if (equippedItemOfType == null)
		{
			container.AddChild(new Label { Text = "None" });
			return;
		}

		var selectionData = new InventoryItemSelectionData(
			InventoryItemSelectionSource.Inventory,
			equippedItemOfType.InventoryItem.Id,
			equippedItemOfType.InventoryItem.ManipulativeDefId);

		var button = new ManipulativeButton
		{
			SelectionDataText = selectionData,
			CustomMinimumSize = new Vector2(150, 30),
			SizeFlagsHorizontal = SizeFlags.ShrinkCenter
		};

		var handler = new Action(() =>
		{
			Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.InventoryItemSelectedFlexible,
				(string)selectionData);
		});

		button.Pressed += handler;

		_buttonHandlerCombos.Add(new ButtonHandlerCombo
		{
			Button = button,
			Handler = handler
		});

		container.AddChild(button);
	}

	public override void _ExitTree()
	{
		CleanDisplay();
	}
} 
