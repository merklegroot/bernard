using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Game.Models;

public record RoomDef : IHasId
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init;}

    [JsonPropertyName("exits")]
    public List<RoomExit> Exits { get; init; } = new();
}