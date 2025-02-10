using System.Text.Json.Serialization;
using Game.Converters;
using YamlDotNet.Serialization;

namespace Game.Models;

public record RoomExit
{
    [YamlMember(Alias = "direction")]
    [JsonPropertyName("direction")]
    [YamlConverter(typeof(DirectionYamlConverter))]
    public Direction Direction { get; init; }
    
    [YamlMember(Alias = "destinationId")]
    [JsonPropertyName("destinationId")]
    public string DestinationId { get; init; }
}