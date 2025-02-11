namespace Game.Models.State;

public record MobState
{
    public string MobDefId { get; init; }
    
    public string Name { get; init; }
    
    public int CurrentHp { get; init; }
    
    public int MaxHp { get; init; }
    
    public string ImageAsset { get; init; }
}
