using Godot;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _container;
	
	public override void _Ready()
	{
		_container = GetNode<VBoxContainer>("VBoxContainer");
		
		// Create and add three example labels
		AddEquipmentLabel("Weapon Slot");
		AddEquipmentLabel("Armor Slot");
		AddEquipmentLabel("Accessory Slot");
	}
	
	private void AddEquipmentLabel(string text)
	{
		var label = new Label
		{
			Text = text,
			HorizontalAlignment = HorizontalAlignment.Left,
			CustomMinimumSize = new Vector2(0, 40)
		};
		
		_container.AddChild(label);
	}
} 
