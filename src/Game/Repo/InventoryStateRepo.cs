using System;
using System.Collections.Generic;
using Game.Models;

namespace Game.Repo;

public interface IInventoryStateRepo
{
    List<InventoryItem> List();

    void RemoveIndex(int inventoryIndex);

    void AddManipulaltive(Guid manipulativeDefId);
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

    public void AddManipulaltive(Guid manipulativeDefId)
    {
        GameStateContainer.GameState.Inventory.Add(
            new InventoryItem
            {
                ManipulativeDefId = manipulativeDefId,
                IsEquipped = false
            });
    }
}