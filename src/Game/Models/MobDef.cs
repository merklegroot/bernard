using YamlDotNet.Serialization;

namespace Game.Models;

public record MobDef : ICreature
{
    [YamlMember(Alias = "id")]
    public string Id { get; init; }
    
    [YamlMember(Alias = "name")]
    public string Name { get; init; }

    [YamlMember(Alias = "img")]
    public string ImageAsset { get; init; }

    [YamlMember(Alias = "con")]
    public int Constitution { get; init; }
    
    [YamlMember(Alias = "str")]
    public int Strength { get; init; }
}