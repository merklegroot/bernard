using System.Text.Json.Serialization;
using Game.Converters;

namespace Game.Models;

[JsonConverter(typeof(DirectionConverter))]
public enum Direction
{
    Invalid = 0,
    North = 1,
    East = 2,
    South = 3,
    West = 4
}