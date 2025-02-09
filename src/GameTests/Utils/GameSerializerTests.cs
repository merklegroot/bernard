using Game.Utils;
using Xunit.Abstractions;
using Shouldly;

namespace GameTests.Utils;

public class GameSerializerTests(ITestOutputHelper outputHelper)
{
    private record SomeModel
    {
        public string? Name { get; init; }
    }

    [Fact]
    public void Should_serialize_collection_toml()
    {
        var contents = GameSerializer.SerializeToml(
            new List<SomeModel>
            {
                new()
                {
                    Name = "testing"
                }
            });
        
        outputHelper.WriteLine(contents);
    }

    [Fact]
    public void Should_serialize_and_deserialize_toml()
    {
        var original = new SomeModel
        {
            Name = "testing"
        };
        
        var contents = GameSerializer.SerializeToml(original);
        
        var deserialized = GameSerializer.DeserializeToml<SomeModel>(contents);
        
        deserialized.Name.ShouldBe(original.Name);
    }

    [Fact]
    public void With_collection()
    {
        var original = new List<SomeModel>
        {
            new()
            {
                Name = "testing"
            }
        };
        
        var contents = GameSerializer.SerializeToml(original);
        outputHelper.WriteLine(contents);
        
        var deserialized =GameSerializer.DeserializeToml<List<SomeModel>>(contents);
        
        deserialized.Single().Name.ShouldBe(original.Single().Name);
    }
}
