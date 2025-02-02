using System;
using System.Text.Json.Serialization;

namespace Game.Models;

public record RoomExit
{
    [JsonPropertyName("direction")]
    public Direction Direction { get; init; }

    [JsonPropertyName("destinationId")]
    public Guid DestinationId { get; init; }
}