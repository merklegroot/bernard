using Game.Models;

namespace Game.Repo;

public interface IRoomDefRepo : IResourceEntityListRepo<RoomDef>
{
}

public class RoomDefRepo : ResourceEntityListRepo<RoomDef>, IRoomDefRepo
{
    protected override string ResourcePath => "res://data/room-defs.json";
    protected override string Key => "room-defs";
}