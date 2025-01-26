using System.Collections.Generic;

public static class InitialGameState
{
    public static GameState Get() =>
        new()
        {
            Health = 25,
            Gold = 70,
            Inventory = new List<InventoryItem>
            {
                new InventoryItem { Id = KnownManipulatives.Torch },
                new InventoryItem { Id = KnownManipulatives.Bread },
                new InventoryItem { Id = KnownManipulatives.Knife },
            }
        };
}