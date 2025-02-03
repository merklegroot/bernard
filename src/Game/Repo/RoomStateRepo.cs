using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;

namespace Game.Repo;

public interface IRoomStateRepo
{
    RoomState Get(Guid roomId);

    void AddManipulative(Guid roomId, Guid manipulativeDefId);

    void RemoveManipulativeByDefId(Guid roomId, Guid manipulativeDefId);
    
    void RemoveManipulativeByInstanceId(Guid roomId, Guid manipulativeInstanceId);
    
    ManipulativeInstance GetManipulativeByInstanceId(Guid roomId, Guid manipulativeInstanceId);
}

public class RoomStateRepo : IRoomStateRepo
{
    private static Dictionary<Guid, RoomState> _roomStates = new();
    private readonly IRoomDefRepo _roomDefRepo;

    public RoomStateRepo(IRoomDefRepo roomDefRepo)
    {
        _roomDefRepo = roomDefRepo;
    }
    
    public RoomState Get(Guid id)
    {
        if (!_roomStates.ContainsKey(id))
        {
            _roomStates[id] = new RoomState
            {
                RoomId = id,
                ManipulativeInstances = new List<ManipulativeInstance>()
            };
        }

        return _roomStates[id];
    }

    public void AddManipulative(Guid roomId, Guid manipulativeDefId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeInstances.Add(new ManipulativeInstance
        {
            Id = Guid.NewGuid(),
            ManipulativeDefId = manipulativeDefId
        });
    }

    public void RemoveManipulativeByDefId(Guid roomId, Guid manipulativeDefId)
    {
        var roomState = Get(roomId);
        var matchingIndex = roomState.ManipulativeInstances
            .FindIndex(item => item.ManipulativeDefId == manipulativeDefId);
            
        if (matchingIndex < 0)
            throw new ApplicationException($"Cannot find manipulative in room {roomId} with manipulativeDefId {manipulativeDefId}");
        
        roomState.ManipulativeInstances.RemoveAt(matchingIndex);
    }

    public void RemoveManipulativeByInstanceId(Guid roomId, Guid manipulativeInstanceId)
    {
        var roomState = Get(roomId);
        var matchingIndex = roomState.ManipulativeInstances
            .FindIndex(item => item.Id == manipulativeInstanceId);
            
        if (matchingIndex < 0)
            throw new ApplicationException($"Cannot find manipulative in room {roomId} with manipulativeInstanceId {manipulativeInstanceId}");
        
        roomState.ManipulativeInstances.RemoveAt(matchingIndex);
    }
    
    public ManipulativeInstance GetManipulativeByInstanceId(Guid roomId, Guid manipulativeInstanceId)
    {
        var room = Get(roomId);
        return room.ManipulativeInstances
            .First(item => item.Id == manipulativeInstanceId);
    }
}
