using Game;
using Game.Models;
using Godot;

public partial class MainScene : Control
{
	private const string RpgTextBoxPath = "VSplitContainer/RpgText";
	private const string StatusPanelPath = "StatusPanel";
	private const string InventoryListPanelPath = "InventoryListPanel";
	private const string ButtonPath = "Button";
	private const string ToastPath = "Toast";
	
	private readonly IAltTextRepo _altTextRepo;
	private readonly GameState _gameState;

	private StatusPanel _statusPanel;
	private InventoryListPanel _inventoryListPanel;
	private Button _button;
	private Toast _toast;
	
	// Default constructor for Godot
	public MainScene()
	{
		GameHost.InjectDependencies(this);
	}

	// Constructor for testing
	public MainScene(IAltTextRepo altTextRepo, GameState gameState)
	{
		_altTextRepo = altTextRepo;
		_gameState = gameState;
	}
	
	public override void _Ready()
	{
		_statusPanel = GetNode<StatusPanel>(StatusPanelPath);
		_inventoryListPanel = GetNode<InventoryListPanel>(InventoryListPanelPath);		
		_button = GetNode<Button>(ButtonPath);
		_toast = GetNode<Toast>(ToastPath);

		_button.Pressed += OnButtonPressed;
		EventBus.Instance.InventoryItemSelected += OnInventoryItemSelected;
		
		SetRandomText();
	}
	
	private void SetRandomText()
	{
		var rpgText = GetNode<Control>(RpgTextBoxPath);
		var texts = _altTextRepo.List();
		rpgText.Set("text", texts[GD.RandRange(0, texts.Count - 1)]);
	}

	private void OnButtonPressed()
	{
		SetRandomText();

		_gameState.Health += 1;
		EventBus.Instance.EmitSignal(EventBus.SignalName.StatusChanged);
		
		_toast.Show("Health increased!");
	}

	private void OnInventoryItemSelected(int inventoryItemIndex)
	{
		_toast.Show($"Selected item: {inventoryItemIndex}");
	}
}
