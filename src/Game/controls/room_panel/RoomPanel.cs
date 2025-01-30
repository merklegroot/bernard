using System;
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
		_manipulativesContainer = GetNode<HFlowContainer>("ManipulativesPanel/ManipulativeContainer");

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

	private readonly List<Action> _handlers = new();
	
	private void UpdateManipulatives(List<string> manipulativeIds)
	{
		var children = _manipulativesContainer.GetChildren();
		for (var index = 0; index < children.Count; index++)
		{
			var child = children[index];
			
			if (child is not Button button)
				continue;
			
			button.Pressed -= _handlers[index];
			button.QueueFree();
		}
		
		_handlers.Clear();
		for(var i = 0; i < manipulativeIds.Count; i++)
		{
			var manipulativeId = manipulativeIds[i];
			
			var manipulativeDef = _manipulativeDefRepo.Get(manipulativeId);
			var button = new Button
			{
				Text = manipulativeDef.Name,
				CustomMinimumSize = new Vector2(100, 30),
				SizeFlagsHorizontal = SizeFlags.ShrinkCenter
			};

			var scopeIndex = i;
			var handler = new Action(() => OnManipulativeButtonPressed(scopeIndex));
			button.Pressed += handler;

			_handlers.Add(handler);
			_manipulativesContainer.AddChild(button);
		}
	}

	private void OnManipulativeButtonPressed(int roomItemIndex)
	{
		var selectionData = (string)new InventoryItemSelectionData
			{ Source = InventoryItemSelectionSource.Room, Index = roomItemIndex }; 
			
		EventBus.Instance.EmitSignal(EventBus.SignalName.InventoryItemSelctedFlexible, selectionData);
		
		GD.Print($"roomItemIndex: {roomItemIndex}");
	}

	private string GetManipulativeDescription(string manipulativeId)
	{
		var manipulative = _manipulativeDefRepo.Get(manipulativeId);
		return $"{manipulative.Name} ({manipulative.Id})";
	}
}
