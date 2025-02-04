using System.Linq;
using Game.Models;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _container;
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
		_container = GetNode<VBoxContainer>("VBoxContainer");
		
		EventBus.Instance.InventoryChanged += OnInventoryChanged;
		
		UpdateDisplay();
	}

	private void OnInventoryChanged()
	{
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		var equippedItems = GameStateContainer.GameState.Inventory.Where(item => item.IsEquipped).ToList();

		var combo = equippedItems.Select(inventoryItem =>
			{
				var manipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeDefId);
				return new
				{
					inventoryItem,
					manipulativeDef
				};
			})
			.ToList();

		var weapon = 
			combo.FirstOrDefault(item => 
				item.manipulativeDef.IsWeapon);
		
		var armor = 
			combo.FirstOrDefault(item => 
				item.manipulativeDef.IsArmor);
		
		var weaponName = weapon != null
			? weapon.manipulativeDef.Name
			: "None";

		_weaponSlot.Text = $"Weapon: {weaponName}";
		
		var helmet = 
			combo.FirstOrDefault(item => 
				item.manipulativeDef.IsHelmet);
		
		AddHelmetInfo(helmet?.inventoryItem);
		
		var armorName = armor != null
			? armor.manipulativeDef.Name
			: "None";
		
		_armorSlot.Text = $"Armor: {armorName}";
		
	}

	private void AddHelmetInfo(InventoryItem inventoryItem)
	{
		foreach (var child in _helmetContainer.GetChildren())
		{
			child.QueueFree();
		}

		var label = new Label { Text = "Helmet:" };
		_helmetContainer.AddChild(label);

		if (inventoryItem == null)
		{
			_helmetContainer.AddChild(new Label { Text = "None" });
			return;
		}
		
		var selectionData = new InventoryItemSelectionData(
			InventoryItemSelectionSource.Inventory,
			inventoryItem.Id,
			inventoryItem.ManipulativeDefId);
		
		var button = new ManipulativeButton
		{
			SelectionDataText = selectionData,
			CustomMinimumSize = new Vector2(150, 30),
			SizeFlagsHorizontal = SizeFlags.ShrinkCenter
		};
		
		// _helmetContainer.AddChild(new Label { Text = manipulativeDef.Name });
		_helmetContainer.AddChild(button);
	}
} 
