using System;
using System.Collections.Generic;

namespace Game.Repo;

public interface IRoomStateRepo
{
    RoomState Get(Guid roomId);

    void AddManipulative(Guid roomId, Guid manipulativeId);

    void RemoveManipulative(Guid roomId, Guid manipulativeId);
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
            var roomDef = _roomDefRepo.Get(id);

            _roomStates[id] = new RoomState
            {
                RoomId = id,
                ManipulativeIds = new List<Guid>()
            };
        }

        return _roomStates[id];
    }

    public void AddManipulative(Guid roomId, Guid manipulativeId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeIds.Add(manipulativeId);
    }

    public void RemoveManipulative(Guid roomId, Guid manipulativeId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeIds.Remove(manipulativeId);
    }
}