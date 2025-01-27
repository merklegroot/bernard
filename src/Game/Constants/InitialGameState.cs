using System.Collections.Generic;

namespace Game.Constants;

public static class InitialGameState
{
    public static GameState Get() =>
        new()
        {
            RoomId = KnownRooms.StartingRoomId,
            Health = 25,
            Gold = 70,
            CurrentMainPanel = PanelEnum.Room,
            Inventory = InitialInventory()
        };

    private static List<InventoryItem> InitialInventory() => 
        new()
        {
            new InventoryItem { Id = KnownManipulatives.Torch },
            new InventoryItem { Id = KnownManipulatives.Bread },
            new InventoryItem { Id = KnownManipulatives.Knife },
        };
}