namespace Game.Models.State;

public record CombatState
{
    public string MobDefId { get; init; }
    
    public string MobName { get; init; }
    
    public string MobImageRes { get; init; }
    
    public int MobConstitution { get; init; }
    
    public int MobMaxHp { get; init; }
    
    public int MobHp { get; init; }
    
    public int MobStrength { get; init; }

    public int MobAttack { get; init; }
}