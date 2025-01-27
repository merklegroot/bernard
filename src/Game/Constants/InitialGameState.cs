using System.Collections.Generic;
using Game.Constants;

public static class InitialGameState
{
    public static GameState Get() =>
        new()
        {
            RoomId = KnownRooms.StartingRoomId,
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