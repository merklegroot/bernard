using Game.Models;
using Godot;

public partial class StatusPanel : Panel
{
	private const string AtkLabelPath = "HBoxContainer/AtkLabel";
	private const string HealthLabelPath = "HBoxContainer/HealthLabel";
	private const string GoldLabelPath = "HBoxContainer/GoldLabel";

	private Label _healthLabel;
	private Label _goldLabel;
	private Label _atkLabel;
	
	public override void _Ready()
	{
		_atkLabel = GetNode<Label>(AtkLabelPath);
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
		_atkLabel.Text = $"Atk: {GameStateContainer.GameState.Atk}";
		_healthLabel.Text = $"Health: {GameStateContainer.GameState.Health}";
		_goldLabel.Text = $"Gold: {GameStateContainer.GameState.Gold}";
	}
}
