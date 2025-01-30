using System.Text.Json;
using Game.Models;
using Shouldly;

namespace GameTests.Converters;

public class DirectionConverterTests
{
    [Theory]
    [InlineData(Direction.North, "\"n\"")]
    [InlineData(Direction.East, "\"e\"")]
    [InlineData(Direction.South, "\"s\"")]
    [InlineData(Direction.West, "\"w\"")]
    public void Should_serialize(Direction direction, string serializedValue) =>
        JsonSerializer.Serialize(direction)
            .ShouldBe(serializedValue);

    [Fact]
    public void Should_not_serialize_invalid() =>
        new Action(() => JsonSerializer.Serialize(Direction.Invalid))
            .ShouldThrow<JsonException>();
    
    [Theory]
    [InlineData("\"n\"", Direction.North)]
    [InlineData("\"N\"", Direction.North)]
    [InlineData("\"e\"", Direction.East)]
    [InlineData("\"E\"", Direction.East)]
    [InlineData("\"s\"", Direction.South)]
    [InlineData("\"S\"", Direction.South)]
    [InlineData("\"w\"", Direction.West)]
    [InlineData("\"W\"", Direction.West)]
    public void Should_deserialize(string serializedValue, Direction direction) =>
        JsonSerializer.Deserialize<Direction>(serializedValue)
            .ShouldBe(direction);
    
    [Theory]
    [InlineData("")]
    [InlineData("meh")]
    public void Should_not_deserialize_invalid(string serializedValue)
    {
        new Action(() => { JsonSerializer.Deserialize<Direction>(serializedValue); })
            .ShouldThrow<JsonException>();
    }

    [Fact]
    public void Should_not_deserialize_null() =>
        new Action(() => { JsonSerializer.Deserialize<Direction>(((string?)null)!); })
            .ShouldThrow<ArgumentNullException>();
}