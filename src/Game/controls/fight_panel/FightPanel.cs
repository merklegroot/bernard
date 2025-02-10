using Godot;
using Game.Events;
using Game.Constants;

public partial class FightPanel : Panel
{
    private Button _closeButton;
    
    public override void _Ready()
    {
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