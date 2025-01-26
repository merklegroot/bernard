using System.Text.Json.Serialization;

public record ManipulativeDef
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("isWeapon")]
    public bool IsWeapon { get; init; }
}