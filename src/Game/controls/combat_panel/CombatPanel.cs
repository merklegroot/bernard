using Godot;
using Game.Events;
using Game.Constants;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

public partial class CombatPanel : Panel
{
    private Button _closeButton;
    private Button _attackButton;
    private Label _mobNameLabel;
    private TextureRect _mobImage;
    private Label _debugLabel;
    private ICombatRepo _combatRepo;
    
    public override void _Ready()
    {
        _combatRepo = GlobalContainer.Host.Services.GetRequiredService<ICombatRepo>();
        
        _closeButton = GetNode<Button>("VBoxContainer/FooterContainer/CloseButton");

        // _attackButton = GetNode<Button>("VBoxContainer/ButtonContainer/AttackButton");
        _mobNameLabel = GetNode<Label>("VBoxContainer/MobContainer/MobName");
        _mobImage = GetNode<TextureRect>("VBoxContainer/BodyContainer/MobContainer/MobImage");
        _debugLabel = GetNode<Label>("VBoxContainer/DebugLabel");
        //
        _closeButton.Pressed += OnCloseButtonPressed;
        // _attackButton.Pressed += OnAttackButtonPressed;

        EventBus.Instance.CombatChanged += OnCombatChanged;
        
        UpdateDisplay();
    }

    private void OnAttackButtonPressed()
    {
        GD.Print("Attack button pressed!");
        EventBus.Instance.EmitSignal(EventBus.SignalName.CombatPlayerAttack);
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
            
        _debugLabel.Text = JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { WriteIndented = true });
    }

    private void OnCloseButtonPressed()
    {
        EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Room);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _closeButton.Pressed -= OnCloseButtonPressed;
        // _attackButton.Pressed -= OnAttackButtonPressed;
        EventBus.Instance.CombatChanged -= OnCombatChanged;
    }
} 