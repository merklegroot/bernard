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

    private static List<ManipulativeInstance> InitialInventory() => 
        new()
        {
            new ManipulativeInstance { Id = KnownManipulatives.Torch },
            new ManipulativeInstance { Id = KnownManipulatives.Bread },
            new ManipulativeInstance { Id = KnownManipulatives.Knife },
        };
}