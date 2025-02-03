using System;
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
            Str = 1,
            CurrentMainPanel = PanelEnum.Room,
            Inventory = InitialInventory()
        };

    private static List<InventoryItem> InitialInventory() => 
        new()
        {
            new InventoryItem
            {
                Id = Guid.NewGuid(), 
                ManipulativeDefId = KnownManipulatives.Torch
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(), 
                ManipulativeDefId = KnownManipulatives.Bread
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(), 
                ManipulativeDefId = KnownManipulatives.Knife,
                IsEquipped = false
            },
            new InventoryItem
            {
                Id = Guid.NewGuid(), 
                ManipulativeDefId = KnownManipulatives.IronHelmet,
                IsEquipped = false
            },
        };
}