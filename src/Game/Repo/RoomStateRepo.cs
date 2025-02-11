using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Models.State;

namespace Game.Repo;

public interface IRoomStateRepo
{
    RoomState Get(string roomId);

    void AddManipulative(string roomId, Guid manipulativeDefId);

    void RemoveManipulativeByDefId(string roomId, Guid manipulativeDefId);
    
    void RemoveManipulativeByInstanceId(string roomId, Guid manipulativeInstanceId);
    
    ManipulativeInstance GetManipulativeByInstanceId(string roomId, Guid manipulativeInstanceId);
}

public class RoomStateRepo : IRoomStateRepo
{
    private static Dictionary<string, RoomState> _roomStates = new(StringComparer.OrdinalIgnoreCase);
    private readonly IRoomDefRepo _roomDefRepo;

    public RoomStateRepo(IRoomDefRepo roomDefRepo)
    {
        _roomDefRepo = roomDefRepo;
    }
    
    public RoomState Get(string id)
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

    public void AddManipulative(string roomId, Guid manipulativeDefId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeInstances.Add(new ManipulativeInstance
        {
            Id = Guid.NewGuid(),
            ManipulativeDefId = manipulativeDefId
        });
    }

    public void RemoveManipulativeByDefId(string roomId, Guid manipulativeDefId)
    {
        var roomState = Get(roomId);
        var matchingIndex = roomState.ManipulativeInstances
            .FindIndex(item => item.ManipulativeDefId == manipulativeDefId);
            
        if (matchingIndex < 0)
            throw new ApplicationException($"Cannot find manipulative in room {roomId} with manipulativeDefId {manipulativeDefId}");
        
        roomState.ManipulativeInstances.RemoveAt(matchingIndex);
    }

    public void RemoveManipulativeByInstanceId(string roomId, Guid manipulativeInstanceId)
    {
        var roomState = Get(roomId);
        var matchingIndex = roomState.ManipulativeInstances
            .FindIndex(item => item.Id == manipulativeInstanceId);
            
        if (matchingIndex < 0)
            throw new ApplicationException($"Cannot find manipulative in room {roomId} with manipulativeInstanceId {manipulativeInstanceId}");
        
        roomState.ManipulativeInstances.RemoveAt(matchingIndex);
    }
    
    public ManipulativeInstance GetManipulativeByInstanceId(string roomId, Guid manipulativeInstanceId)
    {
        var room = Get(roomId);
        return room.ManipulativeInstances
            .First(item => item.Id == manipulativeInstanceId);
    }
}
