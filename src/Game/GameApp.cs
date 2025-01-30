using Game.Constants;
using Game.Models;
using Godot;
using Game.Repo;

public partial class GameApp : Node
{
	private readonly RoomStateRepo _roomStateRepo = new();
	private readonly InventoryStateRepo _inventoryStateRepo = new();

	public override void _Ready()
	{
		EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
		EventBus.Instance.InventoryItemSelctedFlexible += OnInventoryItemSelectedFlexible;
		EventBus.Instance.CloseInventoryDetails += OnCloseInventoryDetails;
		EventBus.Instance.PickupRoomItem += OnPickupRoomItem;
	}

	public void OnDropInventoryItem(int inventoryItemIndex)
	{
		GD.Print($"Drop inventoryItemIndex: {inventoryItemIndex}");

		var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];
		_inventoryStateRepo.RemoveIndex(inventoryItemIndex);

		GD.Print($"Adding manipulative {inventoryItem.ManipulativeId} to room {GameStateContainer.GameState.RoomId}");
		_roomStateRepo.AddManipulative(GameStateContainer.GameState.RoomId, inventoryItem.ManipulativeId);

		EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
		EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
		
		CloseInventoryDetails();
	}

	private void OnPickupRoomItem(int roomItemIndex)
	{
		GD.Print($"Pickup roomItemIndex: {roomItemIndex}");
		
		var roomState = _roomStateRepo.Get(GameStateContainer.GameState.RoomId);
		var manipulativeId = roomState.ManipulativeIds[roomItemIndex];

		_roomStateRepo.RemoveManipulative(GameStateContainer.GameState.RoomId, manipulativeId);
		_inventoryStateRepo.AddManipulaltive(manipulativeId);
		
		EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
		EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
		
		CloseInventoryDetails();
	}

	private void OnInventoryItemSelectedFlexible(string data)
	{
		SetMainPanel(PanelEnum.InventoryDetails);
		EventBus.Instance.EmitSignal(EventBus.SignalName.MainPanelChanged);
	}

	private void SetMainPanel(PanelEnum panelEnum)
	{
		GameStateContainer.GameState.CurrentMainPanel = panelEnum;
	}
	
	private void OnCloseInventoryDetails()
	{
		CloseInventoryDetails();
	}
	
	private void CloseInventoryDetails()
	{
		GameStateContainer.GameState.CurrentMainPanel = PanelEnum.Room;
		EventBus.Instance.EmitSignal(EventBus.SignalName.MainPanelChanged);
	}
}
