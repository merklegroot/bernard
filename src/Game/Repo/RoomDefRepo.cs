using System;
using System.Linq;
using Game.Models;

namespace Game.Repo;

public interface IRoomDefRepo // : IResourceEntityListRepo<RoomDef>
{
    RoomDef Get(string id);
}

public class RoomDefRepo : ResourceListRepo<RoomDef>, IRoomDefRepo
{
    protected override string ResourcePath => "res://data/room-defs.json";
    protected override string Key => "room-defs";

    public RoomDef Get(string id) =>
        List().FirstOrDefault(item =>
            string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
}