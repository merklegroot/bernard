using Godot;

namespace Game.Events;

public partial class EventBus : Node
{
	[Signal]
	public delegate void InventoryItemSelectedFlexibleEventHandler(string data);

	[Signal]
	public delegate void DropInventoryItemEventHandler(string manipulativeInstanceId);
	
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
	public delegate void PickupRoomItemEventHandler(string manipulativeInstanceId);
	
	[Signal]
	public delegate void ExitRoomEventHandler(int directionId);
	
	[Signal]
	public delegate void SetMainPanelEventHandler(int panelId);
	
	[Signal]
	public delegate void EquipItemEventHandler(string data);

	[Signal]
	public delegate void UnequipItemEventHandler(string data);

	[Signal]
	public delegate void InitiateCombatEventHandler();
	
	[Signal]
	public delegate void CombatChangedEventHandler();

	public static EventBus Instance { get; private set; } = new();
	
	public override void _Ready()
	{
		Instance = this;
	}
}
