using System;
using System.Collections.Generic;
using Game.Models;
using Game.Models.State;
using Game.Utils;

namespace Game.Constants;

public static class InitialGameState
{
    public static GameState Get()
    {
        const int initialCon = 2;
        var currentHp = CreatureUtility.GetMaxHp(initialCon);
        
        return new GameState
        {
            PlayerState = new PlayerState
            {
                RoomId = KnownRooms.StartingRoomId,
                CurrentHp = currentHp,
                Gold = 70,
                Strength = 2,
                Constitution = initialCon,
                Inventory = InitialInventory()
            },
            CurrentMainPanel = PanelEnum.Room,
            CombatState = new CombatState()
        };
    }

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