using Game.Models;
using Godot;

public partial class InventoryDetailsPanel : Panel
{
	private readonly ManipulativeDefRepo _manipulativeDefRepo = new();

	private Label _titleLabel;
	private Label _label;
	private Button _closeButton;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_titleLabel = GetNode<Label>("TitleLabel");
		_closeButton = GetNode<Button>("HBoxContainer/CloseButton");
		_closeButton.Pressed += OnCloseButtonPressed;

		EventBus.Instance.InventoryItemSelected += OnInventoryItemSelected;
	}
	
	private void OnInventoryItemSelected(int inventoryItemIndex)
	{
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
	
	public override void _ExitTree()
	{
		if (EventBus.Instance != null)
		{
			EventBus.Instance.InventoryItemSelected -= OnInventoryItemSelected;
		}
	}
}
