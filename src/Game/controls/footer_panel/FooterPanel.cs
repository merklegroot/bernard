using Game.Constants;
using Godot;
using Game.Events;

public partial class FooterPanel : Panel
{
    private Button _fightButton;
    
    public override void _Ready()
    {
        _fightButton = GetNode<Button>("FightButton");
        _fightButton.Pressed += OnFightButtonPressed;
    }

    private void OnFightButtonPressed()
    {
        GD.Print("Fight button pressed!");
        EventBus.Instance.EmitSignal(EventBus.SignalName.InitiateCombat);
        // EventBus.Instance.EmitSignal(EventBus.SignalName.SetMainPanel, (int)PanelEnum.Combat);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        _fightButton.Pressed -= OnFightButtonPressed;
    }
} 