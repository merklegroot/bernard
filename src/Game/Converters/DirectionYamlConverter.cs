using System;
using Game.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Game.Converters;

public class DirectionYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Direction);

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer nestedObjectDeserializer)
    {
        var scalar = parser.Consume<Scalar>();
        return scalar.Value?.ToLower() switch
        {
            "n" => Direction.North,
            "e" => Direction.East,
            "s" => Direction.South,
            "w" => Direction.West,
            _ => throw new YamlException($"Invalid direction: {scalar.Value}")
        };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer nestedObjectSerializer)
    {
        if (value == null)
            throw new YamlException("Cannot write null direction");
            
        var direction = (Direction)value;
        var stringValue = direction switch
        {
            Direction.North => "n",
            Direction.East => "e",
            Direction.South => "s",
            Direction.West => "w",
            _ => throw new YamlException("Invalid direction")
        };
        emitter.Emit(new Scalar(stringValue));
    }
}