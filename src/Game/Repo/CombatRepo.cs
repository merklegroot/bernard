using System;
using Game.Constants;
using Game.Models;
using Game.Models.State;
using Game.Models.ViewModels;
using Game.Utils;

namespace Game.Repo;

public interface ICombatRepo
{
	CombatViewModel GetCombatViewModel();

	void InitCombat();
	
	void SetMobHp(int hp);
	
	void KillMob();
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

		var mobViewModel = combatState.Mob != null
			? new MobViewModel
			{
				MobName = combatState.Mob.MobName,
				MobImageRes = combatState.Mob.MobImageRes,
				MobConstitution = combatState.Mob.MobConstitution,
				MobHp = combatState.Mob.MobHp,
				MobMaxHp = combatState.Mob.MobMaxHp,
				MobStrength = combatState.Mob.MobStrength,
				MobAttack = combatState.Mob.MobAttack
			}
			: null;
		
		return new CombatViewModel
		{
			Stage = combatState.Stage,
			Mob = mobViewModel
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
			Stage = CombatStage.Combat,
			Mob = new MobState
			{
				MobDefId = mobDef.Id,
				MobName = mobDef.Name,
				MobImageRes = mobDef.ImageAsset,
				MobConstitution = constitution,
				MobHp = maxHp,
				MobMaxHp = maxHp,
				MobStrength = strength,
				MobAttack = attack
			}
		};
	}

	public void SetMobHp(int hp)
	{
		GameStateContainer.GameState.CombatState.Mob.MobHp = hp;
	}

	public void KillMob()
	{
		GameStateContainer.GameState.CombatState.Mob = null;
		GameStateContainer.GameState.CombatState.Stage = CombatStage.Loot;
	}
}
