namespace Game.Models.State;

public record CombatState
{
    public string MobDefId { get; init; }
    
    public string MobName { get; init; }
    
    public string MobImageRes { get; init; }
}