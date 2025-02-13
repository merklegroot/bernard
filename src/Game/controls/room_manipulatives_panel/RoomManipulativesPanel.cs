using System;
using System.Collections.Generic;
using Game.Models;
using Game.Models.State;
using Game.Repo;
using Godot;
using Microsoft.Extensions.DependencyInjection;

public partial class RoomManipulativesPanel : Panel
{
	private IRoomStateRepo _roomStateRepo;
	private readonly List<Action> _handlers = new();

	private HFlowContainer _manipulativesContainer;
	public override void _Ready()
	{
		_roomStateRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomStateRepo>();
		
		_manipulativesContainer = GetNode<HFlowContainer>("VBoxContainer/BodyContainer/ItemContainer");
		Game.Events.EventBus.Instance.RoomChanged += OnRoomChanged;
		
		UpdateDisplay();
	}

	private void OnRoomChanged()
	{
		UpdateDisplay();
	}
	
	private void UpdateDisplay()
	{
		var currentRoomId = GameStateContainer.GameState.PlayerState.RoomId;
		var roomState = _roomStateRepo.Get(currentRoomId);
		
		UpdateManipulatives(roomState.ManipulativeInstances);
	}
	
	private void UpdateManipulatives(List<ManipulativeInstance> manipulativeInstances)
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

		if (manipulativeInstances.Count == 0)
		{
			var label = new Label { Text = "None" };
			_manipulativesContainer.AddChild(label);
			return;
		}
		
		for (var i = 0; i < manipulativeInstances.Count; i++)
		{
			var manipulativeInstance = manipulativeInstances[i];

			var selectionData = new InventoryItemSelectionData(
				InventoryItemSelectionSource.Room,
				manipulativeInstance.Id,
				manipulativeInstance.ManipulativeDefId);
			
			var button = new ManipulativeButton
			{
				SelectionDataText = selectionData,
				CustomMinimumSize = new Vector2(150, 30),
				SizeFlagsHorizontal = SizeFlags.ShrinkCenter
			};

			var handler = new Action(() => OnManipulativeButtonPressed(selectionData));
			button.Pressed += handler;

			_handlers.Add(handler);
			_manipulativesContainer.AddChild(button);
		}
	}

	private void OnManipulativeButtonPressed(InventoryItemSelectionData selectionData)
	{
		Game.Events.EventBus.Instance.EmitSignal(Game.Events.EventBus.SignalName.InventoryItemSelectedFlexible, (string)selectionData);
		
		GD.Print($"OnManipulativeButtonPressed: {(string)selectionData}");
	}
}
