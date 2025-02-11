using System.Linq;
using Godot;
using Game.Events;
using Game.Constants;
using Game.Models;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;

public partial class FightPanel : Panel
{
    private Button _closeButton;
    private Label _mobNameLabel;
    private TextureRect _mobImage;
    private IMobDefRepo _mobDefRepo;
    
    public override void _Ready()
    {
        _mobDefRepo = GlobalContainer.Host.Services.GetRequiredService<IMobDefRepo>();
        
        _closeButton = GetNode<Button>("VBoxContainer/HBoxContainer/CloseButton");
        _mobNameLabel = GetNode<Label>("VBoxContainer/MobContainer/MobName");
        _mobImage = GetNode<TextureRect>("VBoxContainer/MobContainer/MobImage");
        
        _closeButton.Pressed += OnCloseButtonPressed;

        UseMob(_mobDefRepo.List().First());
    }

    private void UseMob(MobDef mobDef)
    {
        _mobNameLabel.Text = mobDef.Name;
        _mobImage.Texture = GD.Load<Texture2D>(mobDef.ImageAsset);
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