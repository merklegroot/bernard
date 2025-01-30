using System;
using System.Collections.Generic;
using Game.Models;
using Game.Repo;
using Godot;

public partial class RoomManipulativesPanel : Panel
{
    private readonly ManipulativeDefRepo _manipulativeDefRepo = new();
    private readonly RoomStateRepo _roomStateRepo = new();
    private readonly List<Action> _handlers = new();

    private HFlowContainer _manipulativesContainer;
    public override void _Ready()
    {
        _manipulativesContainer = GetNode<HFlowContainer>("ManipulativeContainer");
        EventBus.Instance.RoomChanged += OnRoomChanged;
    }
    
    private void OnRoomChanged()
    {
	    var currentRoomId = GameStateContainer.GameState.RoomId;
	    var roomState = _roomStateRepo.Get(currentRoomId);
	    
	    UpdateManipulatives(roomState.ManipulativeIds);
    }
    
	private void UpdateManipulatives(List<string> manipulativeIds)
	{
		var children = _manipulativesContainer.GetChildren();
		for (var index = 0; index < children.Count; index++)
		{
			var child = children[index];

			if (child is Button button)
				button.Pressed -= _handlers[index];

			child.QueueFree();
		}
		
		_handlers.Clear();

		if (manipulativeIds.Count == 0)
		{
			var label = new Label { Text = "None" };
			_manipulativesContainer.AddChild(label);
			return;
		}
		
		for (var i = 0; i < manipulativeIds.Count; i++)
		{
			var scopeIndex = i;
			var manipulativeId = manipulativeIds[i];			
			
			var manipulativeDef = _manipulativeDefRepo.Get(manipulativeId);
			var button = new Button
			{
				Text = manipulativeDef.Name,
				CustomMinimumSize = new Vector2(100, 30),
				SizeFlagsHorizontal = SizeFlags.ShrinkCenter
			};

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