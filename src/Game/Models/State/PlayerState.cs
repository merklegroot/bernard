using System.Collections.Generic;

namespace Game.Models.State;

public record PlayerState : ICreature
{
    public string RoomId { get; set; }
    public int Gold { get; set; }
    public int Strength { get; set; }
    public int Constitution { get; set; }
    public int CurrentHp { get; set; }
    public List<InventoryItem> Inventory { get; set; }
}