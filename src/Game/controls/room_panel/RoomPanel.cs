using System.Linq;
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
    private readonly ManipulativeDefRepo _manipulativeDefRepo = new();

    public override void _Ready()
    {
        _roomIdLabel = GetNode<Label>("RoomIdLabel");
        _descriptionLabel = GetNode<Label>("DescriptionLabel");
        UpdateDisplay();

        EventBus.Instance.RoomChanged += OnRoomChanged;
    }

    private void OnRoomChanged()
    {
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
        descriptionBuilder.AppendLine("Manipulatives:");
        if (!roomState.ManipulativeIds.Any())
        {
            descriptionBuilder.AppendLine("  None");
        }
        
        foreach (var manipulativeId in roomState.ManipulativeIds)
        {
            descriptionBuilder.AppendLine($"  {GetManipulativeDescription(manipulativeId)}");    
        }
        
        _descriptionLabel.Text = descriptionBuilder.ToString();
    }

    private string GetManipulativeDescription(string manipulativeId)
    {
        var manipulative = _manipulativeDefRepo.Get(manipulativeId);
        return $"{manipulative.Name} ({manipulative.Id})";
    }
}