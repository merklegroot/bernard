using Game.Events;
using Game.Models.State;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class RoomDisplayPanel : GamePanel
{
    private Label _descriptionLabel;
    private IRoomDefRepo _roomDefRepo;

    public override void _Ready()
    {
        base._Ready();
        
        _descriptionLabel = GetNode<Label>("VBoxContainer/BodyContainer/DescriptionLabel");
        _roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();

        UpdateDisplay();
        EventBus.Instance.RoomChanged += OnRoomChanged;
    }

    private void OnRoomChanged()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var currentRoomId = GameStateContainer.GameState.PlayerState.RoomId;
        var roomDef = _roomDefRepo.Get(currentRoomId);

        Title = string.IsNullOrWhiteSpace(roomDef.Name) ? "Room" : roomDef.Name;
        _descriptionLabel.Text = roomDef.Description;
    }

    public override void _ExitTree()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.RoomChanged -= OnRoomChanged;
        }
    }
} 