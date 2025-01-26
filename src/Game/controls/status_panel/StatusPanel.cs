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
	}
	
	public void SetHealth(int value) =>
		_healthLabel.Text = $"Health: {value}";

	public void SetGold(int value) =>
		_goldLabel.Text = $"Gold: {value}";
}
