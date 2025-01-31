using System.Collections.Generic;
using Game.Models;

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
            new InventoryItem { ManipulativeId = KnownManipulatives.Torch },
            new InventoryItem { ManipulativeId = KnownManipulatives.Bread },
            new InventoryItem { ManipulativeId = KnownManipulatives.Knife, IsEquipped = false },
        };
}