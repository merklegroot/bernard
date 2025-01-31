using System.Collections.Generic;
using Game.Models;

namespace Game.Repo;

public interface IInventoryStateRepo
{
    List<ManipulativeInstance> List();

    void RemoveIndex(int inventoryIndex);

    void AddManipulaltive(string manipulativeId);
}

public class InventoryStateRepo : IInventoryStateRepo
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
                ManipulativeId = manipulativeId
            });
    }
}