using Game.Models;

namespace Game.Repo;

public class RoomDefRepo : ResourceEntityListRepo<RoomDef>
{
    protected override string ResourcePath => "res://data/room-defs.json";
    protected override string Key => "room-defs";
}