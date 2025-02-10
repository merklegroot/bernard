using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Utils;

namespace Game.Repo;

public interface IMobDefRepo
{
    List<MobDef> List();
    
    MobDef Get(string id);
}

public class MobDefRepo : IMobDefRepo
{
    private const string ResourcePath = "res://data/mob-defs.yml";

    private readonly IResourceReader _resourceReader;

    public MobDefRepo(IResourceReader resourceReader) =>
        _resourceReader = resourceReader;

    private static List<MobDef> _mobDefs;

    public List<MobDef> List()
    {
        if (_mobDefs != null) return _mobDefs;
        var contents = _resourceReader.Read(ResourcePath);
        _mobDefs = GameSerializer.DeserializeYml<List<MobDef>>(contents);

        return _mobDefs;
    }
    
    public MobDef Get(string id) =>
        List().FirstOrDefault(item =>
            string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase));
}