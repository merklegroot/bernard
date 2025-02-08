using System;
using System.Text.Json.Serialization;

namespace Game.Models;

public interface IHasId<TIdType>
{
    [JsonPropertyName("id")]
    public TIdType Id { get; init; }
}