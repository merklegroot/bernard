using System;
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
    
    private void OnPickupRoomItem(string instanceIdText)
    {
        GD.Print($"Pickup instanceId: {instanceIdText}");
        var manipulativeInstanceId = Guid.Parse(instanceIdText);
        
        var manipulativeInstance = _roomStateRepo.GetManipulativeByInstanceId(GameStateContainer.GameState.RoomId, manipulativeInstanceId);
        
        _roomStateRepo.RemoveManipulativeByInstanceId(GameStateContainer.GameState.RoomId, manipulativeInstanceId);
        _roomStateRepo.RemoveManipulativeByDefId(GameStateContainer.GameState.RoomId, manipulativeInstance.ManipulativeDefId);
        _inventoryStateRepo.AddManipulaltiveByDefId(manipulativeInstance.ManipulativeDefId);
		
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