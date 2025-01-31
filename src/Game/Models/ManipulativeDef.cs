using System.Text.Json.Serialization;

namespace Game.Models;

public record ManipulativeDef : IHasId
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("imageRes")]
    public string ImageRes { get; init; }

    [JsonPropertyName("isWeapon")]
    public bool IsWeapon { get; init; }
}