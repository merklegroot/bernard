using System.Text.Json.Serialization;

namespace Game.Models;

public class RoomDef : IHasId
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
}