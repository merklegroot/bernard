using Godot;

public partial class EquipmentPanel : Panel
{
	private VBoxContainer _container;
	
	public override void _Ready()
	{
		_container = GetNode<VBoxContainer>("VBoxContainer");
	}
} 
