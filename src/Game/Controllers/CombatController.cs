using System;
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
        EventBus.Instance.CombatPlayerAttack += OnPlayerAttack;
    }

    private void OnInitiateCombat()
    {
        _combatRepo.InitCombat();
        EventBus.Instance.EmitSignal(EventBus.SignalName.CombatChanged);
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Combat);
    }

    private void OnPlayerAttack()
    {
        const int damage = 1;
        
        var combatViewModel = _combatRepo.GetCombatViewModel();
        var updatedHp = Math.Max(combatViewModel.Mob.MobHp - damage, 0);

        if (updatedHp <= 0)
        {
            _combatRepo.KillMob();    
        }
        else
        {
            _combatRepo.SetMobHp(updatedHp);
        }

        EventBus.Instance.EmitSignal(EventBus.SignalName.CombatChanged);
    }
}