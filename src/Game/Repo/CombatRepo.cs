using System;
using System.Linq;
using Game.Constants;
using Game.Models.State;
using Game.Models.ViewModels;
using Game.Utils;

namespace Game.Repo;

public interface ICombatRepo
{
    CombatViewModel GetCombatViewModel();

    void InitCombat();
    
    void SetMobHp(int hp);
}

public class CombatRepo : ICombatRepo
{
    private readonly IMobDefRepo _mobDefRepo;
    private readonly Random _random = new Random();

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
            MobConstitution = combatState.MobConstitution,
            MobHp = combatState.MobHp,
            MobMaxHp = combatState.MobMaxHp,
            MobStrength = combatState.MobStrength,
            MobAttack = combatState.MobAttack
        };
    }

    public void InitCombat()
    {
        var allMobDefs = _mobDefRepo.List();
        var mobDef = allMobDefs[_random.Next(0, allMobDefs.Count)];

        var constitution = Math.Max(mobDef.Constitution, GameConstants.MinConstitution);
        var maxHp = CreatureUtility.GetMaxHp(constitution);
        
        var strength = Math.Max(mobDef.Strength, GameConstants.MinStrength);
        
        // TODO: Factor in equipped items
        var attack = strength;
        
        GameStateContainer.GameState.CombatState = new CombatState
        {
            MobDefId = mobDef.Id,
            MobName = mobDef.Name,
            MobImageRes = mobDef.ImageAsset,
            MobConstitution = constitution,
            MobHp = maxHp,
            MobMaxHp = maxHp,
            MobStrength = strength,
            MobAttack = attack
        };
    }

    public void SetMobHp(int hp)
    {
        GameStateContainer.GameState.CombatState.MobHp = hp;
    }
}