using System;
using System.Collections.Generic;
using Game.Models;

namespace Game.Repo;

public interface IInventoryStateRepo
{
    List<InventoryItem> List();

    void AddManipulaltive(Guid manipulativeDefId);

    void RemoveByInstanceId(Guid inventoryInstanceId);
}

public class InventoryStateRepo : IInventoryStateRepo
{
    public List<InventoryItem> List()
    {
        return GameStateContainer.GameState.Inventory;
    }

    public void RemoveIndex(int inventoryIndex)
    {
        GameStateContainer.GameState.Inventory.RemoveAt(inventoryIndex);
    }

    public void RemoveByInstanceId(Guid inventoryInstanceId)
    {
        var matchingIndex = GameStateContainer.GameState.Inventory
            .FindIndex(item => item.Id == inventoryInstanceId);

        if (matchingIndex < 0)
            throw new ApplicationException($"Inventory item not found with instance id {inventoryInstanceId}");
        
        GameStateContainer.GameState.Inventory.RemoveAt(matchingIndex);        
    }

    public void AddManipulaltive(Guid manipulativeDefId)
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