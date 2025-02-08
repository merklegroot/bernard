using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using Game.Models;
using Game.Repo;
using Microsoft.Extensions.DependencyInjection;

public partial class RoomExitsPanel : Panel
{
	private Button _northButton;
	private Button _eastButton;
	private Button _southButton;
	private Button _westButton;

	private IRoomDefRepo _roomDefRepo;

	public override void _Ready()
	{
		_northButton = GetNode<Button>("GridContainer/NorthButton");
		_eastButton = GetNode<Button>("GridContainer/EastButton");
		_southButton = GetNode<Button>("GridContainer/SouthButton");
		_westButton = GetNode<Button>("GridContainer/WestButton");
		
		_northButton.Pressed += () => OnDirectionButtonPressed(Direction.North);
		_eastButton.Pressed += () => OnDirectionButtonPressed(Direction.East);
		_southButton.Pressed += () => OnDirectionButtonPressed(Direction.South);
		_westButton.Pressed += () => OnDirectionButtonPressed(Direction.West);
		
		_roomDefRepo = GlobalContainer.Host.Services.GetRequiredService<IRoomDefRepo>();
		EventBus.Instance.RoomChanged += OnRoomChanged;
		UpdateDisplay();
	}

	private void OnRoomChanged()
	{
		UpdateDisplay();
	}

	private void UpdateDisplay()
	{
		var roomDef = _roomDefRepo.Get(GameStateContainer.GameState.RoomId);

		UpdateExits(roomDef.Exits);
	}

	private void OnDirectionButtonPressed(Direction direction)
	{
		EventBus.Instance.EmitSignal(EventBus.SignalName.ExitRoom, (int)direction);
	}

	private void UpdateExits(List<RoomExit> exits)
	{
		_northButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.North);
		_eastButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.East);
		_southButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.South);
		_westButton.Disabled = exits.All(roomExit => roomExit.Direction != Direction.West);
	}
}
