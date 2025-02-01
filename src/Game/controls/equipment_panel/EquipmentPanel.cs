using System.Linq;
using Game.Models;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _container;
	private Label _weaponSlot;

	private IManipulativeDefRepo _manipulativeDefRepo;
	
	public override void _Ready()
	{
		_manipulativeDefRepo = GlobalContainer.Host.Services.GetRequiredService<IManipulativeDefRepo>();
		
		_weaponSlot = GetNode<Label>("VBoxContainer/WeaponSlot"); 
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
				var manipulativeDef = _manipulativeDefRepo.Get(inventoryItem.ManipulativeId);
				return new
				{
					inventoryItem,
					manipulativeDef
				};
			})
			.ToList();

		var weapon = 
			combo.FirstOrDefault(item => item.manipulativeDef.IsWeapon);
		
		var weaponName = weapon != null
			? weapon.manipulativeDef.Name
			: "None";

		_weaponSlot.Text = $"Weapon: {weaponName}";
	}
} 
