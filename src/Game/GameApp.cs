using Game.Controllers;
using Godot;

public partial class GameApp : Node
{
	public override void _Ready()
	{
		var controllers = new IController[]
		{
			new PanelController(),
			new InventoryController(),
			new RoomController()
		};

		foreach (var controller in controllers)
		{
			controller.Register();
		}
	}
}
