using System;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
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
        GameStateContainer.GameState.Con;
    
    private int GetStr() =>
        GameStateContainer.GameState.Str;

    private int GetAtk()
    {
        var atkFromEquipment = 
            ListInventory()
                .Where(item => item.IsEquipped)
                .Select(item => _manipulativeDefRepo.Get(item.ManipulativeDefId).Atk)
                .Sum();
            
        return GameStateContainer.GameState.Str + atkFromEquipment;
    }
    
    private int GetDef()
    {
        var defFromEquipment = 
            ListInventory()
                .Where(item => item.IsEquipped)
                .Select(item => _manipulativeDefRepo.Get(item.ManipulativeDefId).Def)
                .Sum();
        
        return GameStateContainer.GameState.Con
               + defFromEquipment;
    }

    private int GetCurrentHp() =>
        GameStateContainer.GameState.CurrentHp;
    
    private int GetMaxHp() =>
        CharacterUtil.GetMaxHp(GetCon());
    
    private int GetGold() =>
        GameStateContainer.GameState.Gold;

    public List<InventoryItem> ListInventory()
    {
        return GameStateContainer.GameState.Inventory;
    }

    public InventoryItem GetInventoryItemByInstanceId(Guid instanceId)
    {
        return GameStateContainer.GameState.Inventory
            .First(item => item.Id == instanceId);
    }

    public void RemoveInventoryItemByInstanceId(Guid inventoryInstanceId)
    {
        var matchingIndex = GameStateContainer.GameState.Inventory
            .FindIndex(item => item.Id == inventoryInstanceId);

        if (matchingIndex < 0)
            throw new ApplicationException($"Inventory item not found with instance id {inventoryInstanceId}");
        
        GameStateContainer.GameState.Inventory.RemoveAt(matchingIndex);        
    }

    public void AddInventoryItemByDefId(Guid manipulativeDefId)
    {
        GameStateContainer.GameState.Inventory.Add(
            new InventoryItem
            {
                Id = Guid.NewGuid(),
                ManipulativeDefId = manipulativeDefId,
                IsEquipped = false
            });
    }
}