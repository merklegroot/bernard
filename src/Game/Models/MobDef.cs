using YamlDotNet.Serialization;

namespace Game.Models;

public record MobDef
{
    [YamlMember(Alias = "id")]
    public string Id { get; init; }
    
    [YamlMember(Alias = "name")]
    public string Name { get; init; }

    [YamlMember(Alias = "imageAsset")]
    public string ImageAsset { get; init;}
}