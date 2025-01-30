using System.Collections.Generic;
using Game.Models;

namespace Game.Repo;

public class InventoryStateRepo
{
    public List<ManipulativeInstance> List()
    {
        return GameStateContainer.GameState.Inventory;
    }

    public void RemoveIndex(int inventoryIndex)
    {
        GameStateContainer.GameState.Inventory.RemoveAt(inventoryIndex);
    }

    public void AddManipulaltive(string manipulativeId)
    {
        GameStateContainer.GameState.Inventory.Add(
            new ManipulativeInstance
            {
                ManipulativeId = manipulativeId,
            });
    }
}