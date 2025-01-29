using Game.Constants;
using Game.Models;
using Godot;
using Game.Repo;

public partial class GameApp : Node
{
	private readonly RoomStateRepo _roomStateRepo = new();

	public override void _Ready()
	{
		GD.Print("GameApp initialized");

		EventBus.Instance.DropInventoryItem += OnDropInventoryItem;
		EventBus.Instance.InventoryItemSelected += OnInventoryItemSelected;
		EventBus.Instance.CloseInventoryDetails += OnCloseInventoryDetails;
	}

	public void OnDropInventoryItem(int inventoryItemIndex)
	{
		GD.Print($"Drop inventoryItemIndex: {inventoryItemIndex}");

		var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];
		GameStateContainer.GameState.Inventory.RemoveAt(inventoryItemIndex);

		GD.Print($"Adding manipualive {inventoryItem.Id} to room {GameStateContainer.GameState.RoomId}");
		_roomStateRepo.AddManipulative(GameStateContainer.GameState.RoomId, inventoryItem.Id);

		EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryChanged);
		EventBus.Instance.EmitSignal(EventBus.SignalName.RoomChanged);
		
		CloseInventoryDetails();
	}

	public void OnInventoryItemSelected(int inventoryItemIndex)
	{
		GD.Print($"Inventory item selected | InventoryItemIndex: {inventoryItemIndex}");
		
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
