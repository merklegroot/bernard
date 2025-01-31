using System.Linq;
using Game.Constants;
using Game.Controllers;
using Game.Models;
using Godot;
using Game.Repo;

public partial class GameApp : Node
{
	private readonly RoomDefRepo _roomDefRepo = new();
	private readonly RoomStateRepo _roomStateRepo = new();
	private readonly InventoryStateRepo _inventoryStateRepo = new();

	public override void _Ready()
	{
		EventBus.Instance.PickupRoomItem += OnPickupRoomItem;
		EventBus.Instance.ExitRoom += OnExitRoom;

		var controllers = new IController[]
		{
			new PanelController(),
			new InventoryController()
		};

		foreach (var controller in controllers)
		{
			controller.Register();
		}
	}
	
	private void OnPickupRoomItem(int roomItemIndex)
	{
		GD.Print($"Pickup roomItemIndex: {roomItemIndex}");
		
		var roomState = _roomStateRepo.Get(GameStateContainer.GameState.RoomId);
		var manipulativeId = roomState.ManipulativeIds[roomItemIndex];

		_roomStateRepo.RemoveManipulative(GameStateContainer.GameState.RoomId, manipulativeId);
		_inventoryStateRepo.AddManipulaltive(manipulativeId);
		
		EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
		EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
		EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
	}

	
	private void OnExitRoom(int directionId)
	{
		GD.Print($"GDApp - Exit room direction: {directionId}");
		
		var direction = (Direction)directionId;
		var currentRoomDef = _roomDefRepo.Get(GameStateContainer.GameState.RoomId);
		var matchingExit = currentRoomDef.Exits.Single(queryExit => queryExit.Direction == direction);

		GameStateContainer.GameState.RoomId = matchingExit.DestinationId;
		
		EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
	}
}
