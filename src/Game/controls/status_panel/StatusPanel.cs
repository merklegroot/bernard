using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class StatusPanel : Panel
{
	private const string StrLabelPath = "HBoxContainer/StrLabel";
	private const string ConLabelPath = "HBoxContainer/ConLabel";
	private const string AtkLabelPath = "HBoxContainer/AtkLabel";
	private const string DefLabelPath = "HBoxContainer/DefLabel";
	private const string HealthLabelPath = "HBoxContainer/HealthLabel";
	private const string GoldLabelPath = "HBoxContainer/GoldLabel";

	private Label _healthLabel;
	private Label _goldLabel;
	private Label _atkLabel;
	private Label _defLabel;
	private Label _strLabel;
	private Label _conLabel;

	private IEgoRepo _egoRepo;
	
	public override void _Ready()
	{
		_egoRepo = GlobalContainer.Host.Services.GetRequiredService<IEgoRepo>();

		_strLabel = GetNode<Label>(StrLabelPath);
		_atkLabel = GetNode<Label>(AtkLabelPath);
		_conLabel = GetNode<Label>(ConLabelPath);
		_defLabel = GetNode<Label>(DefLabelPath);
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
		var viewModel = _egoRepo.GetStatusPanelViewModel();

		_strLabel.Text = $"Str: {viewModel.Str}";
		_atkLabel.Text = $"Atk: {viewModel.Atk}";
		_defLabel.Text = $"Def: {viewModel.Def}";
		_conLabel.Text = $"Con: {viewModel.Con}";
		_healthLabel.Text = $"HP: {viewModel.CurrentHp} of {viewModel.MaxHp}";
		_goldLabel.Text = $"Gold: {viewModel.Gold}";
	}
}
