namespace Game.Models.State;

public record CombatState
{
    public CombatStage Stage { get; set; } = CombatStage.None;
    

    public MobState Mob { get; set; }
}