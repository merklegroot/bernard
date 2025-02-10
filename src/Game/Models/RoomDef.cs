using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace Game.Models;

public record RoomDef : IHasId<string>
{
    [YamlMember(Alias = "id")]
    [JsonPropertyName("id")]
    public string Id { get; init; }
    
    [YamlMember(Alias = "name")]
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [YamlMember(Alias = "description")]
    [JsonPropertyName("description")]
    public string Description { get; init;}

    [YamlMember(Alias = "exits")]
    [JsonPropertyName("exits")]
    public List<RoomExit> Exits { get; init; } = new();
}