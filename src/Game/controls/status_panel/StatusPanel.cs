using Game.Models;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class StatusPanel : Panel
{
	private const string AtkLabelPath = "HBoxContainer/AtkLabel";
	private const string HealthLabelPath = "HBoxContainer/HealthLabel";
	private const string GoldLabelPath = "HBoxContainer/GoldLabel";

	private Label _healthLabel;
	private Label _goldLabel;
	private Label _atkLabel;

	private IEgoRepo _egoRepo;
	
	public override void _Ready()
	{
		_egoRepo = GlobalContainer.Host.Services.GetRequiredService<IEgoRepo>();

		_atkLabel = GetNode<Label>(AtkLabelPath);
		_healthLabel = GetNode<Label>(HealthLabelPath);
		_goldLabel = GetNode<Label>(GoldLabelPath);
		
		EventBus.Instance.StatusChanged += OnStatusChanged;
		EventBus.Instance.InventoryChanged += OnInventoryChanged;
		
		UpdateStatus();
	}

	private void OnInventoryChanged()
	{
		UpdateStatus();
	}
	
	private void OnStatusChanged()
	{
		UpdateStatus();
	}
	
	private void UpdateStatus()
	{
		var atk = _egoRepo.GetAtk();

		_atkLabel.Text = $"Atk: {atk}";
		_healthLabel.Text = $"Health: {GameStateContainer.GameState.Health}";
		_goldLabel.Text = $"Gold: {GameStateContainer.GameState.Gold}";
	}
}
