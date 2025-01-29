using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Models;
using Game.Repo;
using Godot;

public partial class RoomPanel : Panel
{
	private Label _roomIdLabel;
	private Label _descriptionLabel;
	private HFlowContainer _manipulativesContainer;

	private readonly RoomDefRepo _roomDefRepo = new();
	private readonly RoomStateRepo _roomStateRepo = new();
	private readonly ManipulativeDefRepo _manipulativeDefRepo = new();

	public override void _Ready()
	{
		_roomIdLabel = GetNode<Label>("RoomIdLabel");
		_descriptionLabel = GetNode<Label>("DescriptionLabel");
		_manipulativesContainer = GetNode<HFlowContainer>("ManipulativesPanel/HFlowContainer");

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
		_roomIdLabel.Text = $"Room ID: {currentRoomId}";
		
		var roomDef = _roomDefRepo.Get(currentRoomId);
		var roomState = _roomStateRepo.Get(currentRoomId);

		var descriptionBuilder = new StringBuilder();
		descriptionBuilder.AppendLine($"Room ID: {currentRoomId}");
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

		UpdateManipulatives(roomState.ManipulativeIds);
	}

	private void UpdateManipulatives(List<string> manipulativeIds)
	{
		// Clear existing buttons
		foreach (var child in _manipulativesContainer.GetChildren())
		{
			child.QueueFree();
		}
		
		// Add a button for each manipulative in the room
		foreach (var manipulativeId in manipulativeIds)
		{
			var manipulativeDef = _manipulativeDefRepo.Get(manipulativeId);
			var button = new Button
			{
				Text = manipulativeDef.Name,
				CustomMinimumSize = new Vector2(0, 30),
				SizeFlagsHorizontal = Control.SizeFlags.Fill
			};
			
			_manipulativesContainer.AddChild(button);
		}
	}

	private string GetManipulativeDescription(string manipulativeId)
	{
		var manipulative = _manipulativeDefRepo.Get(manipulativeId);
		return $"{manipulative.Name} ({manipulative.Id})";
	}
}
