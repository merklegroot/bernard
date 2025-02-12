using Game.Constants;
using Game.Events;
using Game.Repo;

namespace Game.Controllers;

public class CombatController : IController
{
    private readonly ICombatRepo _combatRepo;
    
    public CombatController(ICombatRepo combatRepo) =>
        _combatRepo = combatRepo;
    
    public void Register()
    {
        EventBus.Instance.InitiateCombat += OnInitiateCombat;
    }

    private void OnInitiateCombat()
    {
        _combatRepo.InitCombat();
        EventBus.Instance.EmitSignal(EventBus.SignalName.CombatChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Combat);
    }
}