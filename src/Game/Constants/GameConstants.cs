using System.Collections.Generic;

namespace Game.Constants;

public static class GameConstants
{
    public const string NothingPanelName = "NothingPanel";
    
    public static readonly Dictionary<PanelEnum, string> PanelNameDictionary = new()
    {
        { PanelEnum.Nothing, NothingPanelName },
        { PanelEnum.Room, "RoomPanel" },
        { PanelEnum.InventoryDetails, "InventoryDetailsPanel" },
        { PanelEnum.Combat, "CombatPanel" },
    };

    public const int MinConstitution = 1;
    
    public const int MinStrength = 1;
}