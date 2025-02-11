using Godot;
using Game.Events;
using Game.Constants;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;

public partial class CombatPanel : Panel
{
    private Button _closeButton;
    private Label _mobNameLabel;
    private TextureRect _mobImage;
    private ICombatRepo _combatRepo;
    
    public override void _Ready()
    {
        _combatRepo = GlobalContainer.Host.Services.GetRequiredService<ICombatRepo>();
        
        _closeButton = GetNode<Button>("VBoxContainer/HBoxContainer/CloseButton");
        _mobNameLabel = GetNode<Label>("VBoxContainer/MobContainer/MobName");
        _mobImage = GetNode<TextureRect>("VBoxContainer/MobContainer/MobImage");
        
        _closeButton.Pressed += OnCloseButtonPressed;
        
        EventBus.Instance.CombatChanged += OnCombatChanged;
        
        UpdateDisplay();
    }

    private void OnCombatChanged()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        var viewModel = _combatRepo.GetCombatViewModel();
        
        _mobNameLabel.Text = viewModel.MobName;
        _mobImage.Texture = !string.IsNullOrWhiteSpace(viewModel.MobImageRes)
            ? GD.Load<Texture2D>(viewModel.MobImageRes)
            : null;
    }

    private void OnCloseButtonPressed()
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Room);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _closeButton.Pressed -= OnCloseButtonPressed;
    }
} 