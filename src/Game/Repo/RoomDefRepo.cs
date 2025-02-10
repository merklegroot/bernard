using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Utils;

namespace Game.Repo;

public interface IRoomDefRepo
{
    List<RoomDef> List();
    
    RoomDef Get(string id);
}

public class RoomDefRepo : IRoomDefRepo
{
    private const string ResourcePath = "res://data/room-defs.yml";

    private readonly IResourceReader _resourceReader;

    public RoomDefRepo(IResourceReader resourceReader) =>
        _resourceReader = resourceReader;

    private static List<RoomDef> _roomDefs;

    public List<RoomDef> List()
    {
        if (_roomDefs != null) return _roomDefs;
        var contents = _resourceReader.Read(ResourcePath);
        _roomDefs = GameSerializer.DeserializeYml<List<RoomDef>>(contents);

        return _roomDefs;
    }
    
    public RoomDef Get(string id) =>
        List().FirstOrDefault(item =>
            string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
}