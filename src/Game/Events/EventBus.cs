using Godot;

public partial class EventBus : Node
{
    [Signal]
    public delegate void InventoryItemSelctedFlexibleEventHandler(string data);

    [Signal]
    public delegate void DropInventoryItemEventHandler(int inventoryItemIndex);
    
    [Signal]
    public delegate void InventoryChangedEventHandler();
    
    [Signal]
    public delegate void StatusChangedEventHandler();
    
    [Signal]
    public delegate void MainPanelChangedEventHandler();
    
    [Signal]
    public delegate void CloseInventoryDetailsEventHandler();
    
    [Signal]
    public delegate void RoomChangedEventHandler();

    [Signal]
    public delegate void PickupRoomItemEventHandler(int roomItemIndex);
    
    public static EventBus Instance { get; private set; } = new();
    
    public override void _Ready()
    {
        Instance = this;
    }
}