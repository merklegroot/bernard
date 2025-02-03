using System.Linq;
using Game.Models;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _container;
	private Label _weaponSlot;
	private Label _helmetSlot;
	private Label _armorSlot;

	private IManipulativeDefRepo _manipulativeDefRepo;
	
	public override void _Ready()
	{
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		
		_weaponSlot = GetNode<Label>("VBoxContainer/WeaponSlot");
		_helmetSlot = GetNode<Label>("VBoxContainer/HelmetSlot");
		_armorSlot = GetNode<Label>("VBoxContainer/ArmorSlot");
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
		
		var helmet = 
			combo.FirstOrDefault(item => 
				item.manipulativeDef.IsHelmet);
		
		var weaponName = weapon != null
			? weapon.manipulativeDef.Name
			: "None";

		_weaponSlot.Text = $"Weapon: {weaponName}";
		
		var helmetName = helmet != null
			? helmet.manipulativeDef.Name
			: "None";

		_helmetSlot.Text = $"Helmet: {helmetName}";
		
		var armorName = armor != null
			? armor.manipulativeDef.Name
			: "None";
		
		_armorSlot.Text = $"Armor: {armorName}";
	}
} 
