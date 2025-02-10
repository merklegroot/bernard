using Godot;
using Game.Events;
using Game.Constants;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;

public partial class FightPanel : Panel
{
    private Button _closeButton;
    private IMobDefRepo _mobDefRepo;
    
    public override void _Ready()
    {
        _mobDefRepo = GlobalContainer.Host.Services.GetRequiredService<IMobDefRepo>();
        
        _closeButton = GetNode<Button>("VBoxContainer/HBoxContainer/CloseButton");
        _closeButton.Pressed += OnCloseButtonPressed;
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