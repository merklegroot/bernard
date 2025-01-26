using Godot;

public partial class EventBus : Node
{
    [Signal]
    public delegate void InventoryItemSelectedEventHandler(int inventoryItemIndex);
    
    public static EventBus Instance { get; private set; } = new();
    
    public override void _Ready()
    {
        Instance = this;
    }
}