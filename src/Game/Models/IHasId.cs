using System.Text.Json.Serialization;

namespace Game.Models;

public interface IHasId
{
    [JsonPropertyName("id")]
    public string Id { get; init; }
}