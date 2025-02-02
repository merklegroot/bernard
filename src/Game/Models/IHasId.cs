using System;
using System.Text.Json.Serialization;

namespace Game.Models;

public interface IHasId
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
}