using System.Collections.Generic;

namespace Game.Repo;

public class RoomStateRepo
{
    private static Dictionary<string, RoomState> _roomStates = new();
    private IRoomDefRepo _roomDefRepo;

    public RoomStateRepo() : this(new RoomDefRepo()){}

    public RoomStateRepo(IRoomDefRepo roomDefRepo)
    {
        _roomDefRepo = roomDefRepo;
    }
    
    public RoomState Get(string id)
    {
        if (!_roomStates.ContainsKey(id))
        {
            var roomDef = _roomDefRepo.Get(id);

            _roomStates[id] = new RoomState
            {
                RoomId = id,
                ManipulativeIds = new List<string>()
            };
        }

        return _roomStates[id];
    }

    public void AddManipulative(string roomId, string manipulativeId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeIds.Add(manipulativeId);
    }

    public void RemoveManipulative(string roomId, string manipulativeId)
    {
        var roomState = Get(roomId);
        roomState.ManipulativeIds.Remove(manipulativeId);
    }
}