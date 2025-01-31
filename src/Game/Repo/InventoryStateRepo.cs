using System.Collections.Generic;
using Game.Models;

namespace Game.Repo;

public interface IInventoryStateRepo
{
    List<InventoryItem> List();

    void RemoveIndex(int inventoryIndex);

    void AddManipulaltive(string manipulativeId);
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

    public void AddManipulaltive(string manipulativeId)
    {
        GameStateContainer.GameState.Inventory.Add(
            new InventoryItem
            {
                ManipulativeId = manipulativeId,
                IsEquiped = false
            });
    }
}