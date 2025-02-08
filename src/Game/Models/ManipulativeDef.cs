using System;
using System.Text.Json.Serialization;

namespace Game.Models;

public record ManipulativeDef : IHasId<Guid>
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("imageRes")]
    public string ImageRes { get; init; }

    [JsonPropertyName("isWeapon")]
    public bool IsWeapon { get; init; }
    
    [JsonPropertyName("isHelmet")]
    public bool IsHelmet { get; init; }
    
    [JsonPropertyName("isArmor")]
    public bool IsArmor { get; init; }
    
    [JsonPropertyName("atk")]
    public int Atk { get; init; }
    
    [JsonPropertyName("def")]
    public int Def { get; init; }
}