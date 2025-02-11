using System.Linq;
using Game.Models.State;
using Game.Models.ViewModels;

namespace Game.Repo;

public interface ICombatRepo
{
    CombatViewModel GetCombatViewModel();

    void InitCombat();
}

public class CombatRepo : ICombatRepo
{
    private readonly IMobDefRepo _mobDefRepo;

    public CombatRepo(IMobDefRepo mobDefRepo)
    {
        _mobDefRepo = mobDefRepo;
    }
    
    public CombatViewModel GetCombatViewModel()
    {
        var combatState = GameStateContainer.GameState.CombatState;
        
        return new CombatViewModel
        {
            MobName = combatState.MobName,
            MobImageRes = combatState.MobImageRes,
        };
    }

    public void InitCombat()
    {
        var mobDef = _mobDefRepo.List().First();
        GameStateContainer.GameState.CombatState = new CombatState
        {
            MobDefId = mobDef.Id,
            MobName = mobDef.Name,
            MobImageRes = mobDef.ImageAsset
        };
    }
}