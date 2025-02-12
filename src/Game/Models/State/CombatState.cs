namespace Game.Models.State;

public record CombatState
{
    public string MobDefId { get; set; }
    
    public string MobName { get; set; }
    
    public string MobImageRes { get; set; }
    
    public int MobConstitution { get; set; }
    
    public int MobMaxHp { get; set; }
    
    public int MobHp { get; set; }
    
    public int MobStrength { get; set; }

    public int MobAttack { get; set; }
}