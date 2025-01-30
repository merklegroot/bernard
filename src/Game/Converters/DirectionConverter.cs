using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Game.Models;

namespace Game.Converters;

public class DirectionConverter : JsonConverter<Direction>
{
    public override Direction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()?.ToLower();
        return value switch
        {
            "n" => Direction.North,
            "e" => Direction.East,
            "s" => Direction.South,
            "w" => Direction.West,
            _ => throw new JsonException($"Invalid direction: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, Direction value, JsonSerializerOptions options)
    {
        var serializedValue = value switch
        {
            Direction.North => "n",
            Direction.East => "e",
            Direction.South => "s",
            Direction.West => "w",
            _ => throw new JsonException("Invalid direction")
        };
        
        writer.WriteStringValue(serializedValue);
    }
}