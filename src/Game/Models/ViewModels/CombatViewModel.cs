namespace Game.Models.ViewModels;

public record CombatViewModel
{
    public CombatStage Stage { get; init; }
    
    public MobViewModel Mob { get; init; }
}