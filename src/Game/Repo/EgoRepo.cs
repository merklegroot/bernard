using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Models.State;
using Game.Models.ViewModels;
using Game.Utils;

namespace Game.Repo;

public interface IEgoRepo
{
    StatusPanelViewModel GetStatusPanelViewModel();
    
    List<InventoryItem> ListInventory();
    
    InventoryItem GetInventoryItemByInstanceId(Guid instanceId);

    void AddInventoryItemByDefId(Guid manipulativeDefId);

    void RemoveInventoryItemByInstanceId(Guid inventoryInstanceId);
}

public class EgoRepo : IEgoRepo
{
    private readonly IManipulativeDefRepo _manipulativeDefRepo;
    
    public EgoRepo(IManipulativeDefRepo manipulativeDefRepo) =>
        _manipulativeDefRepo = manipulativeDefRepo;

    public StatusPanelViewModel GetStatusPanelViewModel() =>
        new()
        {
            Str = GetStr(),
            Atk = GetAtk(),
            Def = GetDef(),
            Con = GetCon(),
            CurrentHp = GetCurrentHp(),
            MaxHp = GetMaxHp(),
            Gold = GetGold()
        };
    
    private int GetCon() =>
        GameStateContainer.GameState.PlayerState.Constitution;
    
    private int GetStr() =>
        GameStateContainer.GameState.PlayerState.Strength;

    private int GetAtk()
    {
        var atkFromEquipment = 
            ListInventory()
                .Where(item => item.IsEquipped)
                .Select(item => _manipulativeDefRepo.Get(item.ManipulativeDefId).Atk)
                .Sum();
            
        return GameStateContainer.GameState.PlayerState.Strength + atkFromEquipment;
    }
    
    private int GetDef()
    {
        var defFromEquipment = 
            ListInventory()
                .Where(item => item.IsEquipped)
                .Select(item => _manipulativeDefRepo.Get(item.ManipulativeDefId).Def)
                .Sum();
        
        return GameStateContainer.GameState.PlayerState.Constitution
               + defFromEquipment;
    }

    private int GetCurrentHp() =>
        GameStateContainer.GameState.PlayerState.CurrentHp;
    
    private int GetMaxHp() =>
        CreatureUtility.GetMaxHp(GetCon());
    
    private int GetGold() =>
        GameStateContainer.GameState.PlayerState.Gold;

    public List<InventoryItem> ListInventory()
    {
        return GameStateContainer.GameState.PlayerState.Inventory;
    }

    public InventoryItem GetInventoryItemByInstanceId(Guid instanceId)
    {
        return GameStateContainer.GameState.PlayerState.Inventory
            .First(item => item.Id == instanceId);
    }

    public void RemoveInventoryItemByInstanceId(Guid inventoryInstanceId)
    {
        var matchingIndex = GameStateContainer.GameState.PlayerState.Inventory
            .FindIndex(item => item.Id == inventoryInstanceId);

        if (matchingIndex < 0)
            throw new ApplicationException($"Inventory item not found with instance id {inventoryInstanceId}");
        
        GameStateContainer.GameState.PlayerState.Inventory.RemoveAt(matchingIndex);        
    }

    public void AddInventoryItemByDefId(Guid manipulativeDefId)
    {
        GameStateContainer.GameState.PlayerState.Inventory.Add(
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                ManipulativeDefId = manipulativeDefId,
                IsEquipped = false
            });
    }
}