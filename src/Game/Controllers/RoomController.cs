using System.Linq;
using Game.Models;
using Godot;
using Game.Repo;

namespace Game.Controllers;

// ReSharper disable once UnusedType.Global
public class RoomController : IController
{
    private readonly IRoomDefRepo _roomDefRepo;
    private readonly IRoomStateRepo _roomStateRepo;
    private readonly IInventoryStateRepo _inventoryStateRepo;

    public RoomController(IRoomDefRepo roomDefRepo, IRoomStateRepo roomStateRepo, IInventoryStateRepo inventoryStateRepo)
    {
        _roomDefRepo = roomDefRepo;
        _roomStateRepo = roomStateRepo;
        _inventoryStateRepo = inventoryStateRepo;
    }
    
    public void Register()
    {
        EventBus.Instance.PickupRoomItem += OnPickupRoomItem;
        EventBus.Instance.ExitRoom += OnExitRoom;
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