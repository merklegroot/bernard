using Game.Models;
using Godot;

public partial class InventoryDetailsPanel : Panel
{
	private readonly ManipulativeDefRepo _manipulativeDefRepo = new();

	private Label _titleLabel;
	private Label _label;
	private Button _closeButton;
	private	Button _dropButton;

	private int _inventoryItemIndex;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_dropButton = GetNode<Button>("HBoxContainer/DropButton");
		
		_closeButton.Pressed += OnCloseButtonPressed;
		_dropButton.Pressed += OnDropButtonPressed;
		
		EventBus.Instance.InventoryItemSelected += OnInventoryItemSelected;
	}
	
	private void OnInventoryItemSelected(int inventoryItemIndex)
	{
		_inventoryItemIndex = inventoryItemIndex;

		var inventoryItem = GameStateContainer.GameState.Inventory[inventoryItemIndex];
		
		var matchingManipulativeDef = _manipulativeDefRepo.Get(inventoryItem.Id);

		_titleLabel.Text = matchingManipulativeDef.Name;
		_label.Text = matchingManipulativeDef.Name;

		Visible = true;
	}

	private void OnCloseButtonPressed()
	{
		Visible = false;
	}

	private void OnDropButtonPressed()
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.DropInventoryItem, _inventoryItemIndex);
		Visible = false;
	}
	
	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.InventoryItemSelected -= OnInventoryItemSelected;
		}
	}
}
