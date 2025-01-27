using Game.Models;
using Godot;

public partial class StatusPanel : Panel
{
	private const string HealthLabelPath = "HBoxContainer/HealthLabel";
	private const string GoldLabelPath = "HBoxContainer/GoldLabel";

	private Label _healthLabel;
	private Label _goldLabel;
	
	public override void _Ready()
	{
		_healthLabel = GetNode<Label>(HealthLabelPath);
		_goldLabel = GetNode<Label>(GoldLabelPath);

		EventBus.Instance.StatusChanged += OnStatusChanged;
		
		UpdateStatus();
	}
	
	private void OnStatusChanged()
	{
		UpdateStatus();
	}
	
	private void UpdateStatus()
	{
		_healthLabel.Text = $"Health: {GameStateContainer.GameState.Health}";
		_goldLabel.Text = $"Gold: {GameStateContainer.GameState.Gold}";
	}
}
