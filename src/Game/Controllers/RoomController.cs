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
    private readonly IEgoRepo _iiEgoRepo;

    public RoomController(IRoomDefRepo roomDefRepo, IRoomStateRepo roomStateRepo, IEgoRepo iiEgoRepo)
    {
        _roomDefRepo = roomDefRepo;
        _roomStateRepo = roomStateRepo;
        _iiEgoRepo = iiEgoRepo;
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
        _iiEgoRepo.AddInventoryItemByDefId(manipulativeInstance.ManipulativeDefId);
		
        EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.CloseInventoryDetails);
    }
	
    private void OnExitRoom(int directionId)
    {
        var direction = (Direction)directionId;
        
        GD.Print($"Exit room direction: {direction}");
        
        var currentRoomDef = _roomDefRepo.Get(GameStateContainer.GameState.RoomId);
        var matchingExit = currentRoomDef.Exits.FirstOrDefault(queryExit => queryExit.Direction == direction);
        if (matchingExit == null)
        {
            GD.Print($"No exit in direction: {direction}");
            return;
        }

        GameStateContainer.GameState.RoomId = matchingExit.DestinationId;
		
        EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
    }
}