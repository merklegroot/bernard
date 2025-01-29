using System.Text;
using Game.Models;
using Game.Repo;
using Godot;

public partial class RoomPanel : Panel
{
    private Label _roomIdLabel;
    private Label _descriptionLabel;

    private readonly RoomDefRepo _roomDefRepo = new();
    private readonly RoomStateRepo _roomStateRepo = new();

    public override void _Ready()
    {
        _roomIdLabel = GetNode<Label>("RoomIdLabel");
        _descriptionLabel = GetNode<Label>("DescriptionLabel");
        UpdateDisplay();

        EventBus.Instance.RoomChanged += OnRoomChanged;
    }

    private void OnRoomChanged()
    {
        GD.Print("Room changed");

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var currentRoomId = GameStateContainer.GameState.RoomId;
        var roomDef = _roomDefRepo.Get(currentRoomId);
        var roomState = _roomStateRepo.Get(currentRoomId);

        var descriptionBuilder = new StringBuilder();
        descriptionBuilder.AppendLine($"Room ID: {GameStateContainer.GameState.RoomId}");
        descriptionBuilder.AppendLine($"Room Name: {roomDef.Name}");
        descriptionBuilder.AppendLine($"Manipulatives: {string.Join(", ", roomState.ManipulativeIds)}");
        
        _descriptionLabel.Text = descriptionBuilder.ToString();
    }
}